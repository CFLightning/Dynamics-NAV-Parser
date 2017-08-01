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
            try
            {
                using(StreamReader inputfile = new StreamReader(path))
                {
                    string line;
                    while((line = inputfile.ReadLine()) != null)
                    {
                        if(writer == null || inputfile.ReadLine())
                    }
                }
            }
            //string fileText;
            //fileText = System.IO.File.ReadAllText(path);
           
        }
    }
}
