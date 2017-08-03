using System;
using System.Collections.Generic;
using NAV_Comment_tool.repositories;
using NAV_Comment_tool.parserClass;
using System.Linq;
using NAV_Comment_tool.fileSplitter;
using System.Text;
using System.Threading.Tasks;
using System.IO;

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
                //foreach
                // Insert another foreach to go through the object by each tag alone, finding all changes needed
                StringReader reader = new StringReader(obj.Contents);
                StringBuilder builder = new StringBuilder();
                StringWriter writer = new StringWriter(builder);
                string line, currentFlag=null;
                bool startFlag = false;

                while (null != (line = reader.ReadLine()))
                {
                    if (startFlag == true)
                    {
                        
                        if (line.Contains(currentFlag) && line.Contains(@"//"))
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
                    else if (startFlag == false && line.Contains(@"//"))
                    {
                        foreach (string flag in tags)
                        {
                            if (line.Contains(flag) && !(line.Contains("Description=")) && !(line.Contains("Version List=")))
                            {
                                startFlag = true;
                                currentFlag = flag;
                            }
                            else if(line.Contains(flag) && line.Contains("Description=") && !(line.Contains("Version List=")))
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
