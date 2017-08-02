using System.IO;
using NAV_Comment_tool.parserClass;
using NAV_Comment_tool.repositories;
using System;
using System.Text;

namespace NAV_Comment_tool.fileSplitter
{
    class FileSplitter
    {
        public static void splitFile(string path)
        {
            StringBuilder builder = new StringBuilder();
            StringWriter sWriter = null; 
            ObjectClass newObject = new ObjectClass();
            string name = "";
            try
            {
                using(StreamReader inputfile = new StreamReader(path,Encoding.GetEncoding("ISO-8859-1")))
                {
                    string line;
                    while((line = inputfile.ReadLine()) != null)
                    {
                        if (line.Contains("OBJECT "))
                        {
                            name = "";
                            string[] parameters = line.Split(' ');
                            for (int i = 3; i <= parameters.Length - 1; i++)
                            {
                                name = string.Concat(name, parameters[i]);
                            }
                            name = name.Replace("/", "");
                            newObject = new ObjectClass(Int32.Parse(parameters[2]), parameters[1], name, "");
                        }
                        if (sWriter == null || line.Contains("OBJECT "))
                        {
                            if(sWriter != null)
                            {
                                sWriter.Close();
                                sWriter = null;
                            }
                            if(newObject.Name != "")
                            {
                                newObject.Contents = builder.ToString();
                                ObjectClassRepository.appendObject(newObject);
                            }

                            builder.Clear();
                            sWriter = new StringWriter(builder);
                        }
                        
                        sWriter.WriteLine(line);
                        //writer.WriteLine(line);

                    }
                }
            }
            finally
            {
                if (sWriter != null)
                    sWriter.Close();
            }
        }
    }
}
