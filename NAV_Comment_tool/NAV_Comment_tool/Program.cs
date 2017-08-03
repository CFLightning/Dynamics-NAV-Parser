﻿using NAV_Comment_tool.fileSplitter;
using NAV_Comment_tool.saveTool;
using NAV_Comment_tool.indentationChecker;
using NAV_Comment_tool.modificationSearchTool;

namespace NAV_Comment_tool
{
    class Program
    {
        static void Main()
        {
            string path = @"C:\Users\Administrator\Documents\export\18cust.txt";
            string[] objectTextLines = System.IO.File.ReadAllText(path).Replace("\r", "").Split('\n');
            string[] txt = ChangeCheck.GetModyficationList(objectTextLines).ToArray();
        }
    }
}
