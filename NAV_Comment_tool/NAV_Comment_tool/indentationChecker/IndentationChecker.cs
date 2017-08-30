﻿using System.Collections.Generic;
using NAV_Comment_tool.parserClass;
using NAV_Comment_tool.repositories;
using System.IO;
using System;
using System.Text;

namespace NAV_Comment_tool.indentationChecker
{
    class IndentationChecker
    {
        private static List<String> triggers;

        public static void InitTriggerList()
        {
            triggers = new List<string>
            {
                "OnInit",
                "OnRun",
                "OnOpenPage",
                "OnClosePage",
                "OnFindRecord",
                "OnNextRecord",
                "OnAfterGetRecord",
                "OnNewRecord",
                "OnInsert",
                "OnModify",
                "OnDelete",
                "OnInsertRecord",
                "OnModifyRecord",
                "OnDeleteRecord",
                "OnQueryClosePage",
                "OnAfterGetCurrRecord",
                "OnPreDataItem",
                "OnPostDataItem",
                "OnValidate",
                "OnLookup",
                "OnDrillDown",
                "OnAssistEdit",
                "OnControlAddin",
                "OnAction",
                "PROCEDURE"
            };
        }

        public static bool CheckIndentations()
        {
            InitTriggerList();
            foreach(ObjectClass obj in ObjectClassRepository.objectRepository)
            {
                StringReader reader = new StringReader(obj.Contents);
                StringBuilder builder = new StringBuilder();
                StringWriter writer = new StringWriter(builder);
                string line, check = "";
                bool triggerFlag = false, beginFlag = false, isBegin = false;
                int currentIndentation = 0, endsToGo = 0;

                while (null != (line = reader.ReadLine()))
                {
                    if(triggerFlag == false && beginFlag == false)
                    {
                        check = line.Trim(' '); // Diffrent variable to forbid the program from indenting Trigger=BEGIN lines
                        if (ChangeDetection.TriggerDetection.DetectIfAnyTriggerInLine(line) && !(line.EndsWith("=BEGIN")))
                        {
                            triggerFlag = true;
                        }
                        else if (ChangeDetection.TriggerDetection.DetectIfAnyTriggerInLine(line) && line.EndsWith("=BEGIN")) // line.Contains(flag) 
                        {
                            triggerFlag = true;
                            beginFlag = true;
                            endsToGo++;
                            currentIndentation = line.IndexOf("BEGIN") + 2;
                        }
                    }
                    if(triggerFlag == true && beginFlag == false)
                    {
                        if (line.Contains(" BEGIN ") || line.EndsWith(" BEGIN")) // || line.Contains("=BEGIN")
                        {
                            beginFlag = true;
                            isBegin = true;
                            endsToGo++;
                            currentIndentation = line.IndexOf("BEGIN") + 2;
                        }
                    }
                    if(triggerFlag == true && beginFlag == true)
                    {
                        if (line.Contains(" END ") || line.Contains(" END;") || line.EndsWith(" END")) 
                        {
                            if(endsToGo==1)
                            {
                                triggerFlag = false;
                                beginFlag = false;
                            }
                            endsToGo--;
                        }
                        if ( ((line.Length - line.TrimStart(' ').Length) < currentIndentation) && triggerFlag == true && beginFlag == true && !(line.Contains(check)) && !(isBegin)) // && !(line.Contains("BEGIN")
                        {
                            string indenter = string.Empty.PadLeft(currentIndentation);
                            line = indenter + line.TrimStart(' '); 
                        }
                    }
                    writer.WriteLine(line);
                    isBegin = false;
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
