﻿using System.IO;
using NAV_Comment_tool.parserClass;
using NAV_Comment_tool.repositories;
using System;
using System.Text;

namespace NAV_Comment_tool.fileSplitter
{
    class FileSplitter
    {
        public static bool SplitFile(string path)
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
                        if (sWriter == null || line.StartsWith("OBJECT ") || inputfile.EndOfStream)
                        {
                            if(sWriter != null)
                            {
                                sWriter.Close();
                                sWriter = null;
                            }
                            if(newObject.Name != "")
                            {
                                if(inputfile.EndOfStream)
                                {
                                    builder.AppendLine("}");
                                }
                                newObject.Contents = builder.ToString();
                                ObjectClassRepository.AppendObject(newObject);
                            }

                            builder.Clear();
                            sWriter = new StringWriter(builder);
                        }

                        if (line.StartsWith("OBJECT "))
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
                        sWriter.WriteLine(line);
                    }
                }
            }
            finally
            {
                if (sWriter != null)
                    sWriter.Close();
            }
            return true;
        }
    }
}
