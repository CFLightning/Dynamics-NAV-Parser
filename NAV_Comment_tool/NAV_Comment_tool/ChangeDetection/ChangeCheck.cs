using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace NAV_Comment_tool.fileSplitter
{
    public static class ChangeCheck
    {
        enum Marks { BEGIN, END, OTHER };

        static private Regex[] DefinePatterns()
        {
            string lineFrontComment = @" *// *";        // BEGIN,AND
            string lineBackComment = @".*\S+.\// *";    // OTHER

            List<string> beginPatternParts = new List<string>();
            beginPatternParts.Add(@"<-+ *(IT/)?(?<mod>[A-Z0-9\.]+)");
            beginPatternParts.Add(@"-+< *(IT/)?(?<mod>[A-Z0-9\.]+)");
            beginPatternParts.Add(@"(IT/)?(?<mod>[A-Z0-9\.]+) *begin");
            beginPatternParts.Add(@"(IT/)?(?<mod>[A-Z0-9\.]+) */S");
            //beginPatternParts.Add...
            string beginPattern = "(" + lineFrontComment + beginPatternParts[0] + ")";
            for (int i = 1; i < beginPatternParts.Count; i++)
                beginPattern += "|(" + lineFrontComment + beginPatternParts[i] + ")";
            
            List<string> endPatternParts = new List<string>();
            endPatternParts.Add(@">-+ *(IT/)?(?<mod>[A-Z0-9\.]+)");
            endPatternParts.Add(@"-+> *(IT/)?(?<mod>[A-Z0-9\.]+)");
            endPatternParts.Add(@"(IT/)?(?<mod>[A-Z0-9\.]+) *end");
            endPatternParts.Add(@"(IT/)?(?<mod>[A-Z0-9\.]+) */E");
            //endPatternParts.Add...
            string endPattern = "(" + lineFrontComment + endPatternParts[0] + ")";
            for (int i = 1; i < endPatternParts.Count; i++)
                endPattern += "|(" + lineFrontComment + endPatternParts[i] + ")";

            List<string> otherPatternParts = new List<string>();
            //otherPatternParts.Add(@"[0-9]{2}\.[0-9]{2}\.[0-9]{4}");
            otherPatternParts.Add(@"(?<mod>[A-Z0-9\.]+)");
            //otherPatternParts.Add...
            string otherPattern = "(" + lineBackComment + otherPatternParts[0] + ")";
            for (int i = 1; i < otherPatternParts.Count; i++)
                otherPattern += "|(" + lineBackComment + otherPatternParts[i] + ")";


            Regex rgxBegin = new Regex(beginPattern);
            Regex rgxEnd = new Regex(endPattern);
            Regex rgxOther = new Regex(otherPattern);

            Regex[] rgx = new Regex[3];
            rgx[(int)Marks.BEGIN] = rgxBegin;
            rgx[(int)Marks.END] = rgxEnd;
            rgx[(int)Marks.OTHER] = rgxOther;
            return rgx;
        }

        static private bool FindPattern(string text)
        {
            Regex[] rgx = DefinePatterns();
            if (rgx[(int)Marks.BEGIN].IsMatch(text))
            {
                //Console.WriteLine("BEGIN\t" + text);
                return true;
            }
            else if (rgx[(int)Marks.END].IsMatch(text))
            {
                //Console.WriteLine("END\t" + text);
                return true;
            }
            else if (rgx[(int)Marks.OTHER].IsMatch(text))
            {
               // Console.WriteLine("OTHER\t" + text);
                return true;
            }
            else
            {
                //Console.WriteLine("\t" + text);
                return false;
            }

        }

        static private List<string> FindTags(string[] codeLines)
        {
            List<string> tagList = new List<string>();
            foreach (var line in codeLines)
            {
                if (line.Contains("//"))
                {
                    if (FindPattern(line))
                    {
                        tagList.Add(line);
                    }
                }
            }
            return tagList;
        }

        static private List<string> FindModsInTags(List<string> tagList)
        {
            Regex[] rgx = DefinePatterns();
            List<string> tagModList = new List<string>();
            Match match = null;

            foreach (var tag in tagList)
            {
                if (rgx[(int)Marks.BEGIN].IsMatch(tag))
                {
                    match = rgx[(int)Marks.BEGIN].Match(tag);
                    //Console.WriteLine(Marks.BEGIN + "\t" + match.Groups["mod"].Value + "\t" + tag);
                }
                else if (rgx[(int)Marks.END].IsMatch(tag))
                {
                    match = rgx[(int)Marks.END].Match(tag);
                    //Console.WriteLine(Marks.END + "\t" + match.Groups["mod"].Value + "\t" + tag);
                }
                else if (rgx[(int)Marks.OTHER].IsMatch(tag))
                {
                    match = rgx[(int)Marks.OTHER].Match(tag);
                    //Console.WriteLine(Marks.OTHER + "\t" + match.Groups["mod"].Value + "\t" + tag);
                }
                tagModList.Add(match.Groups["mod"].Value);
            }

            List<string> modList = new List<string>();
            foreach (var tag in tagModList)
            {
                if (!modList.Contains(tag))
                {
                    modList.Add(tag);
                    Console.WriteLine(tag);
                }
            }
            
            return modList;
        }

        static public List<string> GetModyficationList(string[] lines)
        {
            return FindModsInTags(FindTags(lines));
        }
    }
}

//string path = @"C:\Users\Administrator\Documents\export\18cust.txt";
//string[] objectTextLines = System.IO.File.ReadAllText(path).Replace("\r", "").Split('\n');
//string[] txt = ChangeCheck.GetModyficationList(objectTextLines).ToArray();