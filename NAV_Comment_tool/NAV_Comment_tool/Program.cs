using System;
using System.Collections.Generic;
using NAV_Comment_tool.fileSplitter;
using NAV_Comment_tool.repositories;
using NAV_Comment_tool.indentationChecker;
using System.IO;
using NAV_Comment_tool.saveTool;

namespace NAV_Comment_tool
{
    class Program
    {
        static void Main(string[] args)
        {
            //List<string> filesList = ImportFromTxt();

            //Console.ReadKey();
            FileSplitter.splitFile(@"C:\Users\Administrator\Documents\Exported example objects\Objects.txt");
            IndentationChecker.checkIndentations(ObjectClassRepository.fetchRepo());
            //Console.Write(ObjectClassRepository.fetchRepo()[3]);
           // SaveTool.saveToFiles(ObjectClassRepository.fetchRepo());
            //split
            //checkindents
            //save
        }
    }
}
