using System.Collections.Generic;
using System.Text.RegularExpressions;
using NAV_Comment_tool.ChangeDetection;
using System.Linq;

namespace NAV_Comment_tool.fileSplitter
{
    public static class ChangeCheck
    {
        static Regex[] tagPatterns;
        static List<Regex[]> tagPairPattern;
        static string modNo;

        static ChangeCheck()
        {
            modNo = @"(?<mod>[A-Z0-9\._-]+)";
            tagPatterns = new Regex[3];
            tagPairPattern = new List<Regex[]>();
            DefinePatternsNEW();
        }

        enum Marks { BEGIN, END, OTHER };

        static private Regex[] DefinePatternsNEW()
        {
            string lineFrontComment = @" *// *";        // BEGIN,AND // brak  koment//// .*(\S^(//))
            string lineBackComment = @" *[^\s/{2}]+.*// *";    // OTHER .*\S+.\

            List<Regex[]> PatternList = new List<Regex[]>();
            
            List<string> beginPatternParts = new List<string>();
            beginPatternParts.Add(@"<-+ *(IT/)?" + modNo + @"($| )");
            //beginPatternParts.Add(@"-+< *(IT/)?" + modNo);
            beginPatternParts.Add(@"(IT/)?" + modNo + " *begin($| )");
            beginPatternParts.Add(@"(IT/)?" + modNo + " */S($| )");

            List<string> endPatternParts = new List<string>();
            endPatternParts.Add(@"-+> *(IT/)?" + modNo + @"($| )");
            //endPatternParts.Add(@">-+ *(IT/)?" + modNo + "");
            endPatternParts.Add(@"(IT/)?" + modNo + " *end($| )");
            endPatternParts.Add(@"(IT/)?" + modNo + " */E($| )");

            List<string> otherPatternParts = new List<string>();
            otherPatternParts.Add(@"" + modNo + @" [^A-Z0-9\.]*$");

            Regex rgxBegin, rgxEnd, rgxOther;
            Regex[] rgxPair;

            if (beginPatternParts.Count == endPatternParts.Count)
                for (int i = 0; i < beginPatternParts.Count; i++)
                {
                    rgxBegin = new Regex(lineFrontComment + beginPatternParts[i]);
                    rgxEnd = new Regex(lineFrontComment + endPatternParts[i]);
                    rgxPair = new Regex[2];
                    rgxPair[(int)Marks.BEGIN] = rgxBegin;
                    rgxPair[(int)Marks.END] = rgxEnd;
                    tagPairPattern.Add(rgxPair);
                }

            string endPattern = "(" + lineFrontComment + endPatternParts[0] + ")";
            for (int i = 1; i < endPatternParts.Count; i++)
                endPattern += "|(" + lineFrontComment + endPatternParts[i] + ")";

            string beginPattern = "(" + lineFrontComment + beginPatternParts[0] + ")";
            for (int i = 1; i < beginPatternParts.Count; i++)
                beginPattern += "|(" + lineFrontComment + beginPatternParts[i] + ")";

            string otherPattern = "(" + lineBackComment + otherPatternParts[0] + ")";
            for (int i = 1; i < otherPatternParts.Count; i++)
                otherPattern += "|(" + lineBackComment + otherPatternParts[i] + ")"; // CHANGEEE

            rgxBegin = new Regex(beginPattern);
            rgxEnd = new Regex(endPattern);
            rgxOther = new Regex(otherPattern);
            
            tagPatterns[(int)Marks.BEGIN] = rgxBegin;
            tagPatterns[(int)Marks.END] = rgxEnd;
            tagPatterns[(int)Marks.OTHER] = rgxOther;

            return tagPatterns;
        }

        static public Regex GetFittingEndPattern(string beginTagLine)
        {
            foreach (var patternPair in tagPairPattern)
            {
                if (patternPair[(int)Marks.BEGIN].IsMatch(beginTagLine))
                {
                    string patternString = patternPair[(int)Marks.END].ToString();
                    string mod = GetTagedModyfication(beginTagLine);
                    patternString = patternString.Replace(modNo, mod);
                    return new Regex(patternString);
                }
            }
            return new Regex(@"");
        }

        static public bool CheckIfTagInLine(string text)
        {
            if (tagPatterns[(int)Marks.BEGIN].IsMatch(text))
                return true;
            else if (tagPatterns[(int)Marks.END].IsMatch(text))
                return true;
            else if (tagPatterns[(int)Marks.OTHER].IsMatch(text))
                return true;
            else
                return false;
        }

        static public bool CheckIfTagsIsAlone(string tagLine)
        {
            if (tagPatterns[(int)Marks.OTHER].IsMatch(tagLine))
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
            List<string> tagModList = new List<string>();
            Match match = null;

            foreach (var tag in tagList)
            {
                if (tagPatterns[(int)Marks.BEGIN].IsMatch(tag))
                {
                    match = tagPatterns[(int)Marks.BEGIN].Match(tag);
                }
                else if (tagPatterns[(int)Marks.END].IsMatch(tag))
                {
                    match = tagPatterns[(int)Marks.END].Match(tag);
                }
                else if (tagPatterns[(int)Marks.OTHER].IsMatch(tag))
                {
                    match = tagPatterns[(int)Marks.OTHER].Match(tag);
                }

                tagModList.Add(match.Groups["mod"].Value);
            }

            List<string> modList = new List<string>();
            foreach (var tag in tagModList)
            {
                if (!modList.Contains(tag))
                {
                    modList.Add(tag);
                    //Console.WriteLine(tag);
                }
            }
            
            return modList;
        }

        static public string GetTagedModyfication(string tagLine)
        {
            if (tagPatterns[(int)Marks.BEGIN].IsMatch(tagLine))
            {
                return tagPatterns[(int)Marks.BEGIN].Match(tagLine).Groups["mod"].Value;
            }
            else if (tagPatterns[(int)Marks.END].IsMatch(tagLine))
            {
                return tagPatterns[(int)Marks.END].Match(tagLine).Groups["mod"].Value;
            }
            else if (tagPatterns[(int)Marks.OTHER].IsMatch(tagLine))
            {
                return tagPatterns[(int)Marks.OTHER].Match(tagLine).Groups["mod"].Value;
            }
            return "";
        }

        static public List<string> GetModyficationList(string code)
        {
            string[] codeLines = code.Replace("\r", "").Split('\n');
            List<string> ret = FindModsInTags(FindTags(codeLines));
            return ret.Union(GetFieldDescriptionTagList(code)).ToList();
        }

        static public List<string> GetTagList(string code)
        {
            string[] codeLines = code.Replace("\r", "").Split('\n');
            return FindTags(codeLines);
        }

        static public List<string> GetFieldDescriptionTagList(string code)
        {
            string[] codeLines = code.Replace("\r", "").Split('\n');
            List<string> tagList = new List<string>();

            int i = 0;
            while (i < codeLines.Length - 1)
            {
                while (!FieldDetection.DetectIfFieldsStartFlag(codeLines[i]) && i < codeLines.Length - 1)
                    i++;
                while (!FieldDetection.DetectIfFieldsEndFlag(codeLines[i]) && i < codeLines.Length - 1)
                {
                    if (codeLines[i].Contains("Description="))
                    {
                        string fieldDescription = ChangeDetection.FieldDetection.GetFieldDescription(codeLines[i]);
                        tagList.AddRange(fieldDescription.Split(',').ToList());
                    }
                    i++;
                }
            }

            List<string> modList = new List<string>();
            foreach (var tag in tagList)
            {
                if (!modList.Contains(tag))
                {
                    modList.Add(tag);
                }
            }
            return tagList;
        }
    }
}

//string path = @"C:\Users\Administrator\Documents\export\18cust.txt";
//string[] objectTextLines = System.IO.File.ReadAllText(path).Replace("\r", "").Split('\n');
//string[] txt = ChangeCheck.GetModyficationList(objectTextLines).ToArray();