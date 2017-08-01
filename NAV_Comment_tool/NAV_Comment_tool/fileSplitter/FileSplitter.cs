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
            // int count = 1;
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
                                string[] parameters = line.Split(' ');
                                string name = "";
                                for(int i=3; i > parameters.Length; i++)
                                {
                                    name = string.Concat(name, parameters[i]);
                                }
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

                            writer = new StreamWriter(@"C:\Users\Administrator\Documents\Exported example objects\file" + newObject.Number.ToString() + newObject.Type + " .txt", true);

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
