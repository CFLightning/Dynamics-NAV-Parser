using NAV_Comment_tool.parserClass;
using NAV_Comment_tool.repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NAV_Comment_tool.saveTool
{
    class DocumentationExport
    {
        enum Types { TableData, Table, Form, Report, Dataport, Codeunit, XMLport, MenuSuite, Page }; // Add actual numbers for each of the parameters

        public static bool GenerateDocumentationFile()
        {
            Types result;
            int lineAmount = 1;
            
            Regex lineChecker = new Regex(".*#.*#.*");
            Regex blockChecker = new Regex(".*#.*#$");
            foreach (ObjectClass obj in ObjectClassRepository.objectRepository)
            {
                StringReader reader = new StringReader(obj.Contents);
                StringBuilder builder = new StringBuilder();
                StringWriter writer = new StringWriter(builder);
                bool bracketFlag = false, beginFlag = false, writing = false, isOneLine = false, documentationPrompt = false;
                string line, tagLine = "", trimmer;

                if (Enum.TryParse(obj.Type, out result))
                {
                    //Console.WriteLine("1<next>{0}<next>{1}<next>#tagtagtag#<next> tresc", (int)result, obj.Name);
                    while (null != (line = reader.ReadLine()))
                    {
                        if (line.StartsWith("    BEGIN"))
                        {
                            beginFlag = true;
                        }

                        if (line.StartsWith("      Automated Documentation"))
                        {
                            documentationPrompt = true;
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
                                    tagLine = trimmer;
                                }
                                else if (lineChecker.IsMatch(trimmer))
                                {
                                    trimmer = trimmer.Substring(0, trimmer.LastIndexOf("#") + 1);
                                    tagLine = trimmer;
                                    isOneLine = true;
                                }
                                Console.WriteLine("{3}<next>{0}<next>{1}<next>{4}<next>{2}", (int)result, obj.Name, line, lineAmount, tagLine);
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
            return true;
        }
    }
}
