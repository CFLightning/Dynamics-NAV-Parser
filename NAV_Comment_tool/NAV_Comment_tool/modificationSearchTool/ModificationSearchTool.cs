using System;
using System.Collections.Generic;
using NAV_Comment_tool.repositories;
using NAV_Comment_tool.parserClass;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace NAV_Comment_tool.modificationSearchTool
{
    class ModificationSearchTool
    {
        private static List<string> tags;

        public static void initTags()
        {
            tags = new List<string>
            {
                "231/"
            };
        }

        public static bool findAndSaveChanges()
        {
            initTags();
            foreach (ObjectClass obj in ObjectClassRepository.objectRepository)
            {
                StringReader reader = new StringReader(obj.Contents);
                StringBuilder builder = new StringBuilder();
                StringWriter writer = new StringWriter(builder);
                string line, currentFlag=null;
                bool startFlag = false;

                while (null != (line = reader.ReadLine()))
                {
                    if (startFlag == true)
                    {
                        if (line.Contains(currentFlag))
                        {
                            startFlag = false;
                        }
                        else
                        {
                            writer.WriteLine(line);
                        }
                    }
                    else if (startFlag == false)
                    {
                        foreach (string flag in tags)
                        {
                            if (line.Contains(flag))
                            {
                                startFlag = true;
                                currentFlag = flag;
                            }
                        }
                    }
                }
                ChangeClassRepository.appendChange(new ChangeClass(currentFlag, builder.ToString()));

                writer.Close();
                builder = new StringBuilder();
                writer = new StringWriter(builder);
            }

            foreach(ChangeClass change in ChangeClassRepository.changeRepository)
            {
                Console.WriteLine(change.Contents);
            }
            return true;
        }
    }
}
