using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace NAV_Comment_tool
{
    class Program
    {
        static void Main(string[] args)
        {
            List<string> filesList = ImportFromTxt();

            Console.ReadKey();
        }

        private static List<string> ImportFromTxt()
        {
            DirectoryInfo d = new DirectoryInfo(@"C:\Users\Administrator\Documents\GIt\NAV_Comment_tool\NAV_Comment_tool\TEMP");//Assuming Test is your Folder
            FileInfo[] Files = d.GetFiles("*.txt"); //Getting Text files
            List<string> filesList = new List<string>();
            foreach (FileInfo file in Files)
            {
                filesList.Add(file.Name);
                Console.WriteLine("{0}.\t{1}", filesList.Count, file.Name);
            }
            return filesList;
        }
    }
}
