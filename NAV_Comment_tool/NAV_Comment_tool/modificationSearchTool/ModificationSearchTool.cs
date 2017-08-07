using System.Collections.Generic;
using NAV_Comment_tool.repositories;
using NAV_Comment_tool.parserClass;
using NAV_Comment_tool.fileSplitter;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;

namespace NAV_Comment_tool.modificationSearchTool
{
    class ModificationSearchTool
    {
        private static List<string> tags;

        public static void initTags(ObjectClass obj)
        {
            tags = ChangeCheck.GetModyficationList(obj.Contents);
        }

        public static bool findAndSaveChanges()
        {
            foreach (ObjectClass obj in ObjectClassRepository.objectRepository)
            {
                initTags(obj);
                foreach (string modtag in tags) 
                {
                    StringReader reader = new StringReader(obj.Contents);
                    StringBuilder builder = new StringBuilder();
                    StringWriter writer = new StringWriter(builder);
                    string line, currentFlag = null; //MAYBE SUBJECT TO CHANGES
                    Regex endFlag = new Regex("");
                    ChangeClass change = new ChangeClass();
                    bool startFlag = false;
                    string trigger = "";

                    while (null != (line = reader.ReadLine()))
                    {
                        if (ChangeDetection.TriggerAndFunctionDetection.DetectIfAnyTriggerInLine(line))
                            trigger = ChangeDetection.TriggerAndFunctionDetection.GetTriggerName(line);
                        if (startFlag == true)
                        {
                            if (line.Contains(modtag) && endFlag.IsMatch(line)) //MAYBE SUBJECT TO CHANGES
                            {
                                startFlag = false;
                                if (builder.ToString() != "")
                                {
                                    change = new ChangeClass(currentFlag, builder.ToString(), "Code", trigger);
                                    //ChangeClass change = new ChangeClass(currentFlag, builder.ToString(), "Code", trigger);
                                    ChangeClassRepository.appendChange(change);
                                    obj.Changelog.Add(change);
                                }

                                writer.Close();
                                builder = new StringBuilder();
                                writer = new StringWriter(builder);
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
                                    change = new ChangeClass(modtag, line, "Code", trigger);
                                    ChangeClassRepository.appendChange(change);
                                    obj.Changelog.Add(change);
                                }
                                else if(ChangeCheck.GetTagedModyfication(line) == modtag)
                                {
                                    currentFlag = modtag;
                                    startFlag = true;
                                    endFlag = ChangeCheck.GetFittingEndPattern(line);
                                }
                            }
                            else if (line.Contains(modtag) && line.Contains("Description=") && !(line.Contains("Version List=")))
                            {
                                change = new ChangeClass(modtag, ("FieldFound Test MESSAGE" + modtag), "Field", "TEST:Field name");
                                ChangeClassRepository.appendChange(change);
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
