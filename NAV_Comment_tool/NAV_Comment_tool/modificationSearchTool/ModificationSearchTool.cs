using System.Collections.Generic;
using NAV_Comment_tool.repositories;
using NAV_Comment_tool.parserClass;
using NAV_Comment_tool.fileSplitter;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using System.Linq;

namespace NAV_Comment_tool.modificationSearchTool
{
    class ModificationSearchTool
    {
        private static List<string> tags;

        public static void InitTags(ObjectClass obj)
        {
            tags = TagDetection.GetModyficationList(obj.Contents);
        }

        public static bool FindAndSaveChanges()
        {
            foreach (ObjectClass obj in ObjectClassRepository.objectRepository)
            {
                InitTags(obj);
                foreach (string modtag in tags) 
                {
                    StringReader reader = new StringReader(obj.Contents);
                    StringBuilder builder = new StringBuilder();
                    StringWriter writer = new StringWriter(builder);
                    string line, currentFlag = null; //MAYBE SUBJECT TO CHANGES
                    Regex endFlag = new Regex("");
                    string nestedFlag = "";
                    //Regex[] endFlags = new Regex[3];
                    ChangeClass change = new ChangeClass();
                    bool startFlag = false;
                    int nesting = 0;
                    string trigger = "";
                    bool fieldFlag = false, actionFlag = false, controlFlag = false, openControlFlag = false;
                    string fieldName = "", sourceExpr = "", description = "", fieldContent = "";

                    while (null != (line = reader.ReadLine()))
                    {
                        if (ChangeDetection.TriggerDetection.DetectIfAnyTriggerInLine(line))
                            trigger = ChangeDetection.TriggerDetection.GetTriggerName(line);

                        // Flags
                        if (obj.Type == "Table")
                        {
                            if (fieldFlag == false && ChangeDetection.FlagDetection.DetectIfFieldsStartFlag(line))
                                fieldFlag = true;
                            else if (fieldFlag == true && ChangeDetection.FlagDetection.DetectIfFieldsEndFlag(line))
                                fieldFlag = false;
                            else if (fieldFlag == true && ChangeDetection.FlagDetection.DetectIfNextFieldFlag(line))
                            {
                                fieldName = ChangeDetection.FlagDetection.GetNextFieldName(line);
                                fieldContent = ChangeDetection.FlagDetection.GetNextFieldNumber(line);
                            }
                        }
                        else if (obj.Type == "Page")
                        {
                            if (actionFlag == false && ChangeDetection.FlagDetection.DetectIfActionStartFlag(line))
                                actionFlag = true;
                            else if (actionFlag == true && ChangeDetection.FlagDetection.DetectIfActionEndFlag(line))
                            {
                                actionFlag = false;
                                controlFlag = true;
                            }
                            else if (controlFlag == true && ChangeDetection.FlagDetection.DetectIfControlEndFlag(line))
                                controlFlag = false;
                        }

                    if (startFlag == true)
                        {
                            if (line.Contains(modtag) && endFlag.IsMatch(line)) //Problem jest ze strzałkami i zagnieżdżeniami !
                            {
                                if (nesting == 1) // IF DODANY <-----
                                {
                                    startFlag = false;
                                    if (builder.ToString() != "")
                                    {
                                        change = new ChangeClass(currentFlag, builder.ToString(), "Code", (fieldFlag ? (fieldName + " - ") : "") + trigger, obj.Type + " " + obj.Number + " " + obj.Name);
                                        ChangeClassRepository.AppendChange(change);
                                        obj.Changelog.Add(change);
                                    }

                                    writer.Close();
                                    builder = new StringBuilder();
                                    writer = new StringWriter(builder);
                                }
                                nesting--; // NESTING DODANE <-----
                            }
                            else if (line.EndsWith(nestedFlag))
                            {
                                nesting++; // DODANE <-----
                            }
                            else
                            {
                                writer.WriteLine(line);
                            }
                        }
                        else if (startFlag == false)
                        {
                            if (line.Contains(modtag) && !(line.StartsWith("Description=")) && !(line.Contains("Version List=")) && line.Contains(@"//"))
                            {
                                if (TagDetection.CheckIfTagsIsAlone(line))
                                {
                                    change = new ChangeClass(modtag, line, "Code", (fieldFlag ? (fieldName + " - ") : "") + trigger, obj.Type + " " + obj.Number + " " + obj.Name);
                                    ChangeClassRepository.AppendChange(change);
                                    obj.Changelog.Add(change);
                                }
                                else if(TagDetection.GetTagedModyfication(line) == modtag && TagDetection.CheckIfBeginTagInLine(line))
                                {
                                    currentFlag = modtag;
                                    startFlag = true;
                                    nesting++; // ZMIENIONE <-----
                                    endFlag = TagDetection.GetFittingEndPattern(line);
                                    nestedFlag = line.Trim(' ');
                                }
                            }
                            else if (obj.Type == "Table")
                            {
                                //if (line.Contains(modtag) && line.Contains("Description=") && !(line.Contains("Version List=")))
                                if (line.Contains("Description=") && TagDetection.GetDescriptionTagList(line).Contains(modtag) && !(line.Contains("Version List=")))
                                {
                                    //ChangeDetection.FlagDetection.GetDescription(line).Replace("IT/", "");
                                    change = new ChangeClass(modtag, fieldContent, "Field", fieldName, obj.Type + " " + obj.Number + " " + obj.Name);
                                    ChangeClassRepository.AppendChange(change);
                                    obj.Changelog.Add(change);
                                }
                            }
                            else if (obj.Type == "Page")
                            {
                                if (actionFlag)
                                {
                                    if (line.Contains(modtag) && line.Contains("Description="))
                                    {
                                        description = ChangeDetection.FlagDetection.GetDescription(line);
                                        change = new ChangeClass(modtag, "", "Action", "", "");
                                        ChangeClassRepository.AppendChange(change);
                                        obj.Changelog.Add(change);
                                    }
                                }
                                else if (controlFlag)
                                {
                                    if (line.Contains(modtag) && line.Contains("Description="))
                                    {
                                        description = ChangeDetection.FlagDetection.GetDescription(line);
                                        openControlFlag = true;
                                    }
                                    else if (openControlFlag && line.Contains("SourceExpr="))
                                    {
                                        sourceExpr = ChangeDetection.FlagDetection.GetSourceExpr(line);
                                        change = new ChangeClass(modtag, "", "Control", sourceExpr, "");
                                        ChangeClassRepository.AppendChange(change);
                                        obj.Changelog.Add(change);
                                        openControlFlag = false;
                                    }
                                }
                                
                            }
                            
                            
                        }
                    }
                }
            }
            return true;
        }
    }
}
