using System.Collections.Generic;
using NAV_Comment_tool.parserClass;
using NAV_Comment_tool.saveTool;
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
            triggers = new List<string>();
            triggers.Add("OnInit");
            triggers.Add("OnRun");
            triggers.Add("OnOpen");
            triggers.Add("OnClose");
            triggers.Add("OnFind");
            triggers.Add("OnNext");
            triggers.Add("OnAfterGet");
            triggers.Add("OnNew");
            triggers.Add("OnInsert");
            triggers.Add("OnModify");
            triggers.Add("OnDelete");
            triggers.Add("OnQueryClose");
            triggers.Add("OnValidate");
            triggers.Add("OnLookup");
            triggers.Add("OnDrillDown");
            triggers.Add("OnAssistEdit");
            triggers.Add("OnControlAddin");
            triggers.Add("OnAction");
            triggers.Add("PROCEDURE");
        }

        public static bool checkIndentations(List<ObjectClass> objList)
        {
            initTriggerList();
            foreach(ObjectClass obj in objList)
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
                        if (line.Contains("BEGIN"))
                        {
                            beginFlag = true;
                            endsToGo++;
                            currentIndentation = line.IndexOf("BEGIN") + 2;
                        }
                    }
                    if(triggerFlag == true && beginFlag == true)
                    {
                        if (line.Contains("END"))
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

            foreach(ObjectClass obj in objList)
            {
                Console.WriteLine(obj.Contents);
                SaveTool.saveToFiles(obj);
            }
            return true;
        }
    }
}
