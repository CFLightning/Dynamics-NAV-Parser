﻿using NAV_Comment_tool.fileSplitter;
using NAV_Comment_tool.saveTool;
using NAV_Comment_tool.modificationSearchTool;
using NAV_Comment_tool.indentationChecker;
using System;

namespace NAV_Comment_tool
{
    class Program
    {
        static void Main()
        {
            //string path = @"C:\Users\Administrator\Documents\GIt\NAV_Comment_tool\NAV_Comment_tool\TEMP\";
            //FileSplitter.SplitFile(path + "Object.txt");
            string path = @"C:\Users\Administrator\Documents\Exported example objects\";
            Console.WriteLine("Checking path, splitting files");
            FileSplitter.SplitFile(path + "ObjectsFull.txt"); // TODO: Change hardcoded path to dynamically chosen one
            Console.WriteLine("Fixing indentations");
            IndentationChecker.CheckIndentations();
            Console.WriteLine("Looking for modifications");
            ModificationSearchTool.FindAndSaveChanges();
            Console.WriteLine("Preparing the modifications for saving");
            ModificationCleanerTool.CleanChangeCode();
            Console.WriteLine("Generating Documentation() trigger");
            DocumentationTrigger.UpdateDocumentationTrigger();
            Console.WriteLine("Saving objects");
            SaveTool.SaveObjectsToFiles(path);
            Console.WriteLine("Saving changelogs");
            SaveTool.SaveChangesToFiles(path);
            Console.WriteLine("Generating <next> documentation file");
            SaveTool.SaveDocumentationToFile(path, DocumentationExport.GenerateDocumentationFile());
            Console.WriteLine("Success");
        }
    }
}
