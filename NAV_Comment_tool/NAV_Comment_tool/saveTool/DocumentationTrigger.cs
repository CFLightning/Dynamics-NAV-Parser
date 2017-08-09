using NAV_Comment_tool.fileSplitter;
using NAV_Comment_tool.parserClass;
using NAV_Comment_tool.repositories;
using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Linq;

namespace NAV_Comment_tool.saveTool
{
    class DocumentationTrigger
    {
        /*  Nr zmiany
         *      -nowe pole (nazwy pól?)
         *      -nowy kod  (trigger?)
         *      
         *  Nr zmiany
         *      -nowe pole
         *      
         *      itd....
         */


        private static string UpdateVersionList(string codeLine, ObjectClass obj)
        {
            List<string> versionList = new List<string>();
            versionList = codeLine.Substring(codeLine.IndexOf('=') + 1).Split(',').ToList();
            versionList[versionList.Count - 1] = versionList.Last().Substring(0, versionList.Last().Length - 1);
            versionList = versionList.Union(TagDetection.GetModyficationList(obj.Contents)).ToList();

            string versionString = codeLine.Remove(codeLine.IndexOf("=") + 1);
            versionString += string.Join(",", versionList.ToArray()) + ";";

            return versionString;
        }

        public static bool UpdateDocumentationTrigger()
        {
            foreach (ObjectClass obj in ObjectClassRepository.objectRepository)
            {
                StringReader reader = new StringReader(obj.Contents);
                StringBuilder builder = new StringBuilder();
                StringWriter writer = new StringWriter(builder);
                string line;
                bool bracketFlag = false, beginFlag = false, writing = false, documentationPrompt = false, deleteFlag = false, addBrackets = false, bracketsExist = false;

                while (null != (line = reader.ReadLine()) && line != "  PROPERTIES")
                {
                    if (line.Contains("Version List="))
                        line = UpdateVersionList(line, obj);
                    writer.WriteLine(line);
                }

                while (null != (line = reader.ReadLine()))
                {
                    if (line.StartsWith("    BEGIN"))
                    {
                        beginFlag = true;
                    }

                    if (line.StartsWith("    {") && beginFlag)
                    {
                        bracketFlag = true;
                        bracketsExist = true;
                    }

                    if(line.StartsWith("      Automated Documentation"))
                    {
                        documentationPrompt = true;
                    }

                    if (line.StartsWith("    }") && bracketFlag)
                    {
                        bracketFlag = false;
                        deleteFlag = false;
                        writing = true;
                        bracketsExist = true;
                    }

                    if (line.StartsWith("    END") && beginFlag && bracketsExist)
                    {
                        beginFlag = false;
                        //deleteFlag = false;
                    }

                    if (line.StartsWith("    END.") && beginFlag && !bracketsExist)
                    {
                        //Console.WriteLine("HELLO");
                        writing = true;
                        addBrackets = true;
                        deleteFlag = false;
                        //bracketsExist = true;
                        beginFlag = false;
                        //continue;
                    }

                    if (line.StartsWith("      #") && documentationPrompt)
                    {
                        deleteFlag = false;
                        foreach (string item in TagDetection.GetModyficationList(obj.Contents))
                        {
                            if (line.Contains(item)) deleteFlag = true;
                        }
                    }

                    if (writing)
                    {
                        if (addBrackets)
                        {
                            writer.WriteLine("    {");
                        }
                        if (!(documentationPrompt) && obj.Changelog.Count > 0 && !addBrackets)
                        {
                            writer.WriteLine(Environment.NewLine + "      Automated Documentation");
                        }
                        else if (!(documentationPrompt) && obj.Changelog.Count > 0 && addBrackets)
                        {
                            writer.WriteLine("      Automated Documentation");
                        }
                        foreach (string item in TagDetection.GetModyficationList(obj.Contents))
                        {
                            int actionCounter = 0;
                            writer.WriteLine("      #" + item + "#");
                            foreach (ChangeClass change in obj.Changelog)
                            {
                                if (change.ChangelogCode == item && change.ChangeType != "Action")
                                {
                                    writer.WriteLine("      - New " + change.ChangeType + ": " + change.Location);
                                }

                                if (change.ChangeType == "Action" && change.ChangelogCode == item) actionCounter++;
                            }

                            if(actionCounter == 1)
                            {
                                writer.WriteLine("      - New Action");
                            }
                            else if(actionCounter > 1)
                            {
                                writer.WriteLine("      - New Actions");
                            }
                        }
                        if (addBrackets) writer.WriteLine("    }");
                        addBrackets = false;
                        writing = false;
                    }
                    if (!(deleteFlag)) writer.WriteLine(line);
                }

                obj.Contents = builder.ToString();

                writer.Close();
                builder = new StringBuilder();
                writer = new StringWriter(builder);
            }
            return true;
        }
    }
}
