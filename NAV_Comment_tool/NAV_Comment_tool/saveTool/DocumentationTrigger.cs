using NAV_Comment_tool.parserClass;
using NAV_Comment_tool.repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NAV_Comment_tool.saveTool
{
    class DocumentationTrigger
    {
        /*  Nr zmiany
         *      -nowe pole (nazwy pól?)
         *      -nowy kod  (trigger?)
         *      
         *  Nr zmiany
         *      -nowe pole
         *      
         *      itd....
         */
        public static bool updateDocumentationTrigger()
        {
            foreach (ObjectClass obj in ObjectClassRepository.objectRepository)
            {
                StringReader reader = new StringReader(obj.Contents);
                StringBuilder builder = new StringBuilder();
                StringWriter writer = new StringWriter(builder);
                string line;
                bool bracketFlag = false, beginFlag = false, writecheck = false;

                while (null != (line = reader.ReadLine()))
                {
                    if (line.StartsWith("    BEGIN"))
                    {
                        beginFlag = true;
                    }

                    if (line.StartsWith("    {") && beginFlag)
                    {
                        bracketFlag = true;
                        writecheck = true;
                        continue;
                    }

                    if (line.StartsWith("    }") && bracketFlag)
                    {
                        bracketFlag = false;
                        writecheck = false;
                        //continue;
                    }

                    if (line.StartsWith("    END") && beginFlag)
                    {
                        beginFlag = false;
                    }

                    //if (writecheck)
                    //{
                    //    Console.WriteLine("found documentation" + line);
                    //}

                    if (writecheck)
                    {
                        line = "new code" + Environment.NewLine + line;
                        Console.WriteLine(line);
                    }
                }
            }
            return true;
        }
    }
}
