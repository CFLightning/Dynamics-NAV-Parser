using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace NAV_Comment_tool.fileSplitter
{
    class ChangeCheck
    {
        string[] codeLines;

        enum Marks { BEGIN, END };

        public ChangeCheck(string[] lines)
        {
            codeLines = lines;
        }

        private Regex[] DefinePatterns()
        {
            Regex rgxBegin = new Regex(@"((<-+)|(-+<) +(IT/)?[A-Z0-9]+)|((IT/)?[A-Z0-9]+ *(/S)|(begin))"); // <--  IT/FX21   --<  IT/FX21   IT/MF100/S   MF100/S    MF100 begin
            Regex rgxEnd = new Regex(@"((>-+)|(-+>) +(IT/)?[A-Z0-9]+)|((IT/)?[A-Z0-9]+ *(/E)|(end))");

            Regex[] rgx = new Regex[2];
            rgx[(int)Marks.BEGIN] = rgxBegin;
            rgx[(int)Marks.END] = rgxEnd;
            return rgx;
        }

        private void FindPattern(string text)
        {
            Regex[] rgx = DefinePatterns();
            if (rgx[(int)Marks.BEGIN].IsMatch(text))
            {
                Console.WriteLine("BEGIN" + text);
            }
            else if (rgx[(int)Marks.END].IsMatch(text))
            {
                Console.WriteLine("END  " + text);
            }
            else
                Console.WriteLine(text);
        }

        public string[] FindComments()
        {
            List<string> commentedLines = new List<string>();
            foreach (var line in codeLines)
            {
                if (line.Contains("//"))
                {
                    int i = 0;
                    while (line[i] == ' ')
                    {
                        i++;
                    }
                    if (line[i] == '/' && line[i + 1] == '/')
                    {
                        commentedLines.Add(line);
                        FindPattern(line);
                    }
                }
            }
            return commentedLines.ToArray();
        }
    }
}
