﻿using NAV_Comment_tool.fileSplitter;
using NAV_Comment_tool.parserClass;
using NAV_Comment_tool.repositories;
using System;
using System.IO;
using System.Text;

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

        public static bool UpdateDocumentationTrigger()
        {
            foreach (ObjectClass obj in ObjectClassRepository.objectRepository)
            {
                StringReader reader = new StringReader(obj.Contents);
                StringBuilder builder = new StringBuilder();
                StringWriter writer = new StringWriter(builder);
                string line;
                bool bracketFlag = false, beginFlag = false, writing = false, documentationPrompt = false, deleteFlag = false;

                while (null != (line = reader.ReadLine()))
                {
                    if (line.StartsWith("    BEGIN"))
                    {
                        beginFlag = true;
                    }

                    if (line.StartsWith("    {") && beginFlag)
                    {
                        bracketFlag = true;
                    }

                    if(line.StartsWith("      Automated Documentation"))
                    {
                        documentationPrompt = true;
                    }

                    if (line.StartsWith("    }") && bracketFlag)
                    {
                        bracketFlag = false;
                        writing = true;
                    }

                    if (line.StartsWith("    END") && beginFlag)
                    {
                        beginFlag = false;
                    }

                    if(line.StartsWith("      #") && documentationPrompt)
                    {
                        deleteFlag = false;
                        foreach (string item in ChangeCheck.GetModyficationList(obj.Contents))
                        {
                            if (line.Contains(item)) deleteFlag = true;
                        }
                    }

                    if (writing)
                    {
                        if(!(documentationPrompt)) writer.WriteLine(Environment.NewLine + "      Automated Documentation");
                        foreach (string item in ChangeCheck.GetModyficationList(obj.Contents))
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
                        writing = false;
                    }
                    if(!(deleteFlag)) writer.WriteLine(line);
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
