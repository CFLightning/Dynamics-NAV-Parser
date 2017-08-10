using NAV_Comment_tool.parserClass;
using NAV_Comment_tool.repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace NAV_Comment_tool.saveTool
{
    class DocumentationExport
    {
        enum Types { TableData, Table, Form, Report, Dataport, Codeunit, XMLport, MenuSuite, Page }; // Add actual numbers for each of the parameters

        private static Dictionary<string, string> mappingDictionary = new Dictionary<string, string>();

        private static void InitDictionary()
        {
            var dictionaryLines = File.ReadLines(@"C:\Users\Administrator\Documents\Exported example objects\mapping.csv");
            mappingDictionary = dictionaryLines.Select(line => line.Split(';')).ToDictionary(data => data[0], data => data[1]);
            //Console.WriteLine(mappingDictionary["FX01"]);
        }

        public static string GenerateDocumentationFile()
        {
            Types result;
            int lineAmount = 1;
            InitDictionary();

            Regex lineChecker = new Regex(".*#.*#.*");
            Regex blockChecker = new Regex(".*#.*#$");
            string documentation = "";
            
            StringBuilder builder = new StringBuilder();
            StringWriter writer = new StringWriter(builder);
            foreach (ObjectClass obj in ObjectClassRepository.objectRepository)
            {
                StringReader reader = new StringReader(obj.Contents);
                bool bracketFlag = false, beginFlag = false, writing = false, isOneLine = false;//, documentationPrompt = false;
                string line, tagLine = "", trimmer;

                if (Enum.TryParse(obj.Type, out result))
                {
                    while (null != (line = reader.ReadLine()))
                    {
                        if (line.StartsWith("    BEGIN"))
                        {
                            beginFlag = true;
                        }

                        if (line.StartsWith("      Automated Documentation"))
                        {
                            //documentationPrompt = true;
                            writing = true;
                            continue;
                        }

                        if (line.StartsWith("    {") && beginFlag)
                        {
                            bracketFlag = true;
                        }
                       
                        if (line.StartsWith("    }") && bracketFlag)
                        {
                            bracketFlag = false;
                            writing = false;
                        }

                        if (line.StartsWith("    END") && beginFlag)
                        {
                            beginFlag = false;
                        }

                        if(writing)
                        {
                            if (line.Length > 6)
                            {
                                line = line.Substring(6);
                                
                                trimmer = line.TrimStart(' ');
                                trimmer = trimmer.TrimEnd(' ');
                                if(blockChecker.IsMatch(trimmer)) {
                                    trimmer = trimmer.Trim('#');
                                    if (mappingDictionary.ContainsKey(trimmer))
                                    {
                                        trimmer = mappingDictionary[trimmer];
                                    }
                                    trimmer = "#" + trimmer + "#";
                                    tagLine = trimmer;
                                }
                                else if (lineChecker.IsMatch(trimmer))
                                {
                                    trimmer = trimmer.Substring(0, trimmer.LastIndexOf("#") + 1);
                                    tagLine = trimmer;
                                    isOneLine = true;
                                }
                                writer.WriteLine("{0}<next>{1}<next>{2}<next>{3}<next>{4}", lineAmount, (int)result, obj.Name, tagLine, line);
                                //Console.WriteLine("{0}<next>{1}<next>{2}<next>{3}<next>{4}", lineAmount, (int)result, obj.Name, tagLine, line);
                                lineAmount++;
                                if (isOneLine)
                                {
                                    tagLine = "";
                                    isOneLine = false;
                                }
                            }
                        }
                    }
                }
            }
            documentation = builder.ToString();

            return documentation;
        }
    }
}
