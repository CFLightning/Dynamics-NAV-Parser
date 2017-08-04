﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NAV_Comment_tool.ChangeDetection
{
    static class TriggerAndFunctionDetection
    {
        private static List<String> triggers;

        static TriggerAndFunctionDetection()
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

        static public bool DetectIfAnyTriggerInLine(string line)
        {
            foreach (var trigger in triggers)
            {
                if (DetectIfSpecifiedTriggerInLine(trigger, line))
                    return true;
            }
            return false;
        }

        static public bool DetectIfSpecifiedTriggerInLine(string trigger, string line)
        {
            line = line.Trim(' ');
            //if (line.StartsWith(trigger) && (line.EndsWith("=BEGIN") || line.EndsWith("=VAR")))
            if (line == trigger + "=VAR" || line == trigger + "=BEGIN")
            {
                return true;
            }
            return false;
        }

        static public string GetTriggerName(string triggerLine)
        {
            foreach (var trigger in triggers)
            {
                if (DetectIfSpecifiedTriggerInLine(trigger, triggerLine))
                {
                    return trigger;
                }
            }
            return "";
        }

    }
}
