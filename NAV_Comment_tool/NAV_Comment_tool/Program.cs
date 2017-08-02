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
            FileSplitter.splitFile(@"C:\Users\Administrator\Documents\Exported example objects\Objects.txt"); // TODO: Change hardcoded path to dynamically chosen one
            IndentationChecker.checkIndentations(ObjectClassRepository.fetchRepo());
        }
    }
}
