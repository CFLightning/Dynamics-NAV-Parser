using System.Collections.Generic;
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
            return match.Groups["FieldName"].Value.Trim(' ');
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

        static public string GetFieldDescription(string codeLine)
        {
            string fieldDescription = codeLine.Substring((codeLine.IndexOf("Description=") + "Description=".Length));
            fieldDescription = fieldDescription.Remove(fieldDescription.Length - 1).Trim(' ');
            return fieldDescription;
        }
    }
}
