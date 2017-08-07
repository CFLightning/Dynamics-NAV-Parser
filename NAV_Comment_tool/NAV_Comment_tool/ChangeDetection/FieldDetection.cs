using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace NAV_Comment_tool.ChangeDetection
{
    class FieldDetection
    {
        static Regex rgx;

        static FieldDetection()
        {
            rgx = new Regex(@"^\s*{[^;]*;[^;]*;(?<FieldName>[^;]*)\s*;[^;]*;.*=");
        }

        static public bool DetectIfNextFieldFlag(string codeLine)
        {
            return rgx.IsMatch(codeLine);
        }

        static public string GetNextFieldName(string codeLine)
        {
            Match match = rgx.Match(codeLine);
            return match.Groups["FieldName"].Value;
        }

        static public bool DetectIfFieldsStartFlag(string codeLine)
        {
            if (codeLine == "  FIELDS")
                return true;
            else
                return false;
        }

        static public bool DetectIfFieldsEndFlag(string codeLine)
        {
            if (codeLine == "  KEYS")
                return true;
            else
                return false;
        }
    }
}
