using System.IO;
using NAV_Comment_tool.parserClass;
using NAV_Comment_tool.repositories;
using System;
using System.Text;

namespace NAV_Comment_tool.fileSplitter
{
    class FileSplitter
    {
        enum types {}
        public static void splitFile(string path)
        {
            StreamWriter writer = null;
            StringBuilder builder = new StringBuilder();
            StringWriter sWriter = new StringWriter(builder);
            ObjectClass newObject = new ObjectClass();
            string name = "";
            try
            {
                using(StreamReader inputfile = new StreamReader(path))
                {
                    string line;
                    while((line = inputfile.ReadLine()) != null)
                    {
                        if(writer == null || line.Contains("OBJECT "))
                        {
                            if(line.Contains("OBJECT "))
                            {
                                name = "";
                                string[] parameters = line.Split(' ');
                                for(int i=3; i <= parameters.Length - 1; i++)
                                {
                                    name = string.Concat(name, parameters[i]);
                                }
                                name = name.Replace("/", "");
                                newObject = new ObjectClass(Int32.Parse(parameters[2]),parameters[1],name,"");
                            }

                            if(writer != null)
                            {
                                writer.Close();
                                sWriter.Close();
                                writer = null;
                                sWriter = null;
                            }

                            
                            newObject.Contents = builder.ToString();
                            ObjectClassRepository.appendObject(newObject);

                            builder = new StringBuilder();
                            sWriter = new StringWriter(builder);
                            
                            string fileName = string.Concat("Object ", newObject.Number.ToString(), " ", newObject.Type, " ", name, " .txt");
                            string savePath = Path.Combine(@"C:\Users\Administrator\Documents\Exported example objects", fileName);
                            writer = new StreamWriter(savePath, true);

                        }
                        writer.WriteLine(line);
                        sWriter.WriteLine(line);
                    }
                }
            }
            finally
            {
                if (writer != null)
                    writer.Close();
            }
        }
    }
}
