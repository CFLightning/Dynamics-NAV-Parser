using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NAV_Comment_tool.fileSplitter
{
    class FileSplitter
    {
        public static void splitFile(string path)
        {
            StreamWriter writer = null;
            int count = 0;
            try
            {
                using(StreamReader inputfile = new StreamReader(path))
                {
                    string line;
                    while((line = inputfile.ReadLine()) != null)
                    {
                        if(writer == null || line.Contains("OBJECT "))
                        {
                            if(writer != null)
                            {
                                writer.Close();
                                writer = null;
                            }

                            writer = new StreamWriter(@"C:\Users\Administrator\Documents\Exported example objects\file" + count.ToString() + ".txt", true);

                            count = 0;
                        }
                        writer.WriteLine(line);
                        ++count;
                    }
                }
            }
            finally
            {
                if (writer != null)
                    writer.Close();
            }
            //string fileText;
            //fileText = System.IO.File.ReadAllText(path);

        }
    }
}
