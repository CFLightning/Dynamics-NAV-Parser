using System;
using System.Collections.Generic;
using NAV_Comment_tool.fileSplitter;
using NAV_Comment_tool.repositories;
using System.IO;

namespace NAV_Comment_tool
{
    class Program
    {
        static void Main(string[] args)
        {
            //List<string> filesList = ImportFromTxt();

            //Console.ReadKey();
            FileSplitter.splitFile(@"C:\Users\Administrator\Documents\Exported example objects\Objects.txt");
            ObjectClassRepository.listAll();
        }
    }
}
