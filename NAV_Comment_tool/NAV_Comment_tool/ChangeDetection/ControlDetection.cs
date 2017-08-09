using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace NAV_Comment_tool.ChangeDetection
{
    class ControlDetection
    {
        static Regex rgxControl;

        static ControlDetection()
        {
            rgxControl = new Regex(@"^\s*{[^;]*;[^;]*;Field\s*;$");
        }

        static public bool DetectIfNextControlFlag(string codeLine)
        {
            return rgxControl.IsMatch(codeLine);
        }

        static public bool DetectIfControlStartFlag(string codeLine)
        {
            if (codeLine == "  CONTROLS") //  table || page
                return true;
            else
                return false;
        }

        static public bool DetectIfControlEndFlag(string codeLine)
        {
            if (codeLine == "  CODE")   //  table || page
                return true;
            else
                return false;
        }

        static public string GetControlSourceExpr(string codeLine)
        {
            string SourceExpr = codeLine.Substring((codeLine.IndexOf("SourceExpr=") + "SourceExpr=".Length));
            SourceExpr = SourceExpr.Remove(SourceExpr.Length - 1).Trim(' ');
            return SourceExpr;
        }
    }
}
