using System.Collections.Generic;
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

        public static void initTriggerList()
        {
            triggers = new List<string>
            {
                "OnInit",
                "OnRun",
                "OnOpen",
                "OnClose",
                "OnFind",
                "OnNext",
                "OnAfterGet",
                "OnNew",
                "OnInsert",
                "OnModify",
                "OnDelete",
                "OnQueryClose",
                "OnValidate",
                "OnLookup=",
                "OnDrillDown",
                "OnAssistEdit",
                "OnControlAddin",
                "OnAction",
                "PROCEDURE"
            };
        }

        public static bool checkIndentations()
        {
            initTriggerList();
            foreach(ObjectClass obj in ObjectClassRepository.objectRepository)
            {
                StringReader reader = new StringReader(obj.Contents);
                StringBuilder builder = new StringBuilder();
                StringWriter writer = new StringWriter(builder);
                string line;
                bool triggerFlag = false, beginFlag = false;
                int currentIndentation = 0, endsToGo = 0;

                while (null != (line = reader.ReadLine()))
                {
                    if(triggerFlag == false && beginFlag == false)
                    {
                        foreach (string flag in triggers)
                        {
                            if (line.Contains(flag))
                            {
                                triggerFlag = true;
                            }
                        }
                    }
                    if(triggerFlag == true && beginFlag == false)
                    {
                        if (line.Contains(" BEGIN\n")) //  zmienic. startswith"         begin" , liczy //" BEGIN " endswith" BEGIN" "=BEGIN"
                        {
                            beginFlag = true;
                            endsToGo++;
                            currentIndentation = line.IndexOf("BEGIN") + 2;
                        }
                    }
                    if(triggerFlag == true && beginFlag == true)
                    {
                        if (line.Contains("END")) // " end ". endswith " end" " end;"
                        {
                            if(endsToGo==1)
                            {
                                triggerFlag = false;
                                beginFlag = false;
                            }
                            endsToGo--;
                        }
                        if ( ((line.Length - line.TrimStart(' ').Length) < currentIndentation) && triggerFlag == true && beginFlag == true && !(line.Contains("BEGIN")))
                        {
                            line = new string(' ', (line.Length - line.TrimStart(' ').Length)+2) + line;
                        }
                    }
                    writer.WriteLine(line);
                }
                obj.Contents = builder.ToString();

                writer.Close();
                builder = new StringBuilder();
                writer = new StringWriter(builder);
            }

            //foreach(ObjectClass obj in ObjectClassRepository.objectRepository)
            //{
            //    Console.WriteLine(obj.Contents);
            //}

            return true;
        }
    }
}
