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
            string lineFrontComment = @" *// *";        // BEGIN,AND // brak  koment////
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

        static public bool CheckIfTagInLine(string text)
        {
            Regex[] rgx = DefinePatterns();
            if (rgx[(int)Marks.BEGIN].IsMatch(text))
                return true;
            else if (rgx[(int)Marks.END].IsMatch(text))
                return true;
            else if (rgx[(int)Marks.OTHER].IsMatch(text))
                return true;
            else
                return false;
        }

        static private List<string> FindTags(string[] codeLines)
        {
            List<string> tagList = new List<string>();
            foreach (var line in codeLines)
            {
                if (line.Contains("//"))
                {
                    if (CheckIfTagInLine(line))
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
                    Console.WriteLine(Marks.BEGIN + "\t" + match.Groups["mod"].Value + "\t" + tag);
                }
                else if (rgx[(int)Marks.END].IsMatch(tag))
                {
                    match = rgx[(int)Marks.END].Match(tag);
                    Console.WriteLine(Marks.END + "\t" + match.Groups["mod"].Value + "\t" + tag);
                }
                else if (rgx[(int)Marks.OTHER].IsMatch(tag))
                {
                    match = rgx[(int)Marks.OTHER].Match(tag);
                    Console.WriteLine(Marks.OTHER + "\t" + match.Groups["mod"].Value + "\t" + tag);
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

        static public string CheckTagedModyfication(string codeLine)
        {
            Regex[] rgx = DefinePatterns();

            if (rgx[(int)Marks.BEGIN].IsMatch(codeLine))
            {
                return rgx[(int)Marks.BEGIN].Match(codeLine).Groups["mod"].Value;
            }
            else if (rgx[(int)Marks.END].IsMatch(codeLine))
            {
                return rgx[(int)Marks.END].Match(codeLine).Groups["mod"].Value;
            }
            else if (rgx[(int)Marks.OTHER].IsMatch(codeLine))
            {
                return rgx[(int)Marks.OTHER].Match(codeLine).Groups["mod"].Value;
            }
            return "";
        }

        static public List<string> GetModyficationList(string code)
        {
            string[] codeLines = code.Replace("\r", "").Split('\n');
            return FindModsInTags(FindTags(codeLines));
        }

        static public List<string> GetTagList(string code)
        {
            string[] codeLines = code.Replace("\r", "").Split('\n');
            return FindTags(codeLines);
        }

    }
}

//string path = @"C:\Users\Administrator\Documents\export\18cust.txt";
//string[] objectTextLines = System.IO.File.ReadAllText(path).Replace("\r", "").Split('\n');
//string[] txt = ChangeCheck.GetModyficationList(objectTextLines).ToArray();