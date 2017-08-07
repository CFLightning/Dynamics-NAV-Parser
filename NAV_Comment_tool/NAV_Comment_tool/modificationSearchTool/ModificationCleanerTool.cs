using System.Text;
using NAV_Comment_tool.parserClass;
using NAV_Comment_tool.repositories;
using System.IO;

namespace NAV_Comment_tool.modificationSearchTool
{
    class ModificationCleanerTool
    {
        public static bool cleanChangeCode()
        {
            foreach(ChangeClass chg in ChangeClassRepository.changeRepository)
            {
                StringReader reader = new StringReader(chg.Contents);
                StringBuilder builder = new StringBuilder();
                StringWriter writer = new StringWriter(builder);
                string line;
                bool isFirstLine = true;
                int firstLineIndent = 0;

                while (null != (line = reader.ReadLine()))
                {
                    if(isFirstLine)
                    {
                        isFirstLine = false;
                        //line = "FOUND FIRST LINE" + line;
                        firstLineIndent = line.Length - line.TrimStart(' ').Length;
                    }
                    if(line.Length > firstLineIndent)
                    {
                        line = line.Substring(firstLineIndent);
                    }
                    writer.WriteLine(line);
                }

                chg.Contents = builder.ToString();

                writer.Close();
                builder = new StringBuilder();
                writer = new StringWriter(builder);
            }
            return true;
        }
    }
}
