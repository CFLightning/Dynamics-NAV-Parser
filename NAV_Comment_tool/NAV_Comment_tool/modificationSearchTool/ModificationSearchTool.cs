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
            tags = ChangeCheck.GetModyficationList(obj.Contents);
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
                    ChangeClass change = new ChangeClass();
                    bool startFlag = false;
                    int nesting = 0;
                    string trigger = "";
                    bool fieldsFlag = false;
                    string field = "";

                    while (null != (line = reader.ReadLine()))
                    {
                        if (ChangeDetection.TriggerDetection.DetectIfAnyTriggerInLine(line))
                            trigger = ChangeDetection.TriggerDetection.GetTriggerName(line);
                        
                        if (fieldsFlag == false && ChangeDetection.FieldDetection.DetectIfFieldsStartFlag(line))
                            fieldsFlag = true;

                        if (fieldsFlag == true && ChangeDetection.FieldDetection.DetectIfNextFieldFlag(line))
                            field = ChangeDetection.FieldDetection.GetNextFieldName(line);

                        if (fieldsFlag == true && ChangeDetection.FieldDetection.DetectIfFieldsEndFlag(line))
                        {
                            field = "";
                            fieldsFlag = false;
                        }

                        if (startFlag == true)
                        {
                            if (line.Contains(modtag) && endFlag.IsMatch(line)) //MAYBE SUBJECT TO CHANGES
                            {
                                if(nesting == 1)
                                {
                                    startFlag = false;
                                    if (builder.ToString() != "")
                                    {
                                        change = new ChangeClass(currentFlag, builder.ToString(), "Code", (fieldsFlag ? (field + " - ") : "") + trigger, obj.Type + " " + obj.Number + " " + obj.Name);
                                        ChangeClassRepository.AppendChange(change);
                                        obj.Changelog.Add(change);
                                    }

                                    writer.Close();
                                    builder = new StringBuilder();
                                    writer = new StringWriter(builder);
                                }
                                nesting--;
                            }
                            else if (line.Contains(modtag) && !(line.StartsWith("Description=")) && !(line.Contains("Version List=")) && line.Contains(@"//") && ChangeCheck.GetTagedModyfication(line) == modtag)
                            {
                                nesting++;
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
                                if (ChangeCheck.CheckIfTagsIsAlone(line))
                                {
                                    change = new ChangeClass(modtag, line, "Code", trigger, obj.Type + " " + obj.Number + " " + obj.Name);
                                    ChangeClassRepository.AppendChange(change);
                                    obj.Changelog.Add(change);
                                }
                                else if(ChangeCheck.GetTagedModyfication(line) == modtag)
                                {
                                    currentFlag = modtag;
                                    startFlag = true;
                                    nesting++;
                                    endFlag = ChangeCheck.GetFittingEndPattern(line);
                                }
                            }
                            else if (line.Contains(modtag) && line.Contains("Description=") && !(line.Contains("Version List=")))
                            {
                                string fieldContent = ChangeDetection.FieldDetection.GetFieldDescription(line);
                                change = new ChangeClass(modtag, fieldContent, "Field", field, obj.Type + " " + obj.Number + " " + obj.Name);
                                ChangeClassRepository.AppendChange(change);
                                obj.Changelog.Add(change);
                            }
                        }
                    }
                }
            }
            return true;
        }
    }
}
