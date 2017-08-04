using System;
using System.Collections.Generic;
using NAV_Comment_tool.repositories;
using NAV_Comment_tool.parserClass;
using System.Linq;
using NAV_Comment_tool.fileSplitter;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;

namespace NAV_Comment_tool.modificationSearchTool
{
    class ModificationSearchTool
    {
        private static List<string> tags;

        public static void initTags(ObjectClass obj)
        {
            //tags = new List<string>
            //{
            //    "231/"
            //};

            tags = ChangeCheck.GetModyficationList(obj.Contents);
        }

        public static bool findAndSaveChanges()
        {
            //initTags();
            foreach (ObjectClass obj in ObjectClassRepository.objectRepository)
            {
                initTags(obj);
                foreach (string modtag in tags) //Insert another foreach to go through the object by each tag alone, finding all changes needed
                {
                    StringReader reader = new StringReader(obj.Contents);
                    StringBuilder builder = new StringBuilder();
                    StringWriter writer = new StringWriter(builder);
                    string line, currentFlag = null; //MAYBE SUBJECT TO CHANGES
                    Regex endFlag = new Regex("");
                    bool startFlag = false;

                    while (null != (line = reader.ReadLine()))
                    {
                        if (startFlag == true)
                        {
                            if (line.Contains(modtag) && endFlag.IsMatch(line)) //MAYBE SUBJECT TO CHANGES
                            {
                                startFlag = false;
                                if (builder.ToString() != "")
                                {
                                    ChangeClassRepository.appendChange(new ChangeClass(currentFlag, builder.ToString(), "Code"));
                                }

                                writer.Close();
                                builder = new StringBuilder();
                                writer = new StringWriter(builder);
                            }
                            else
                            {
                                writer.WriteLine(line);
                            }
                        }
                        else if (startFlag == false)
                        {
                            if (line.Contains(modtag) && !(line.StartsWith("Description=")) && !(line.Contains("Version List=")) && line.Contains(@"//"))
                            {
                                currentFlag = modtag;
                                if (new Regex("<-+ *(IT/)?" + modtag).IsMatch(line))
                                {
                                    startFlag = true;
                                    endFlag = new Regex(@">-+ *(IT/)?" + modtag);
                                }
                                else if (new Regex("-+< *(IT/)?" + modtag).IsMatch(line))
                                {
                                    startFlag = true;
                                    endFlag = new Regex(@"-+> *(IT/)?" + modtag);
                                }
                                else if (new Regex("(IT/)?" + modtag + " *begin").IsMatch(line))
                                {
                                    startFlag = true;
                                    endFlag = new Regex(@"(IT/)?" + modtag + " *end");
                                }
                                else if (new Regex(@"(IT/)?" + modtag + " */S").IsMatch(line))
                                {
                                    startFlag = true;
                                    endFlag = new Regex(@"(IT/)?" + modtag + " */E");
                                }


                                //if (line.Contains(modtag + @"/S")) endFlag = (modtag + @"/E"); //MAYBE SUBJECT TO CHANGES
                                //if (line.Contains(@"<--")) endFlag = "-->";
                                }
                                else if (line.Contains(modtag) && line.Contains("Description=") && !(line.Contains("Version List=")))
                                {
                                    ChangeClassRepository.appendChange(new ChangeClass(currentFlag, "FieldFound Test MESSAGE", "Field"));
                                }
                        }
                    }
                }
               
            }

            foreach(ChangeClass change in ChangeClassRepository.changeRepository)
            {
                Console.WriteLine("THIS IS A NEW CHANGE" + change.ChangelogCode);
                Console.WriteLine(change.Contents);
            }
            return true;
        }
    }
}
