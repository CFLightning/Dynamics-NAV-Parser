using NAV_Comment_tool.fileSplitter;
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
            Console.WriteLine("Checking path, splitting files");

            //string path = @"C:\Users\Administrator\Documents\Exported example objects\";
            //FileSplitter.SplitFile(path + "ExportedObjectsNAV.txt"); // TODO: Change hardcoded path to dynamically chosen one
            //string path = @"C:\Users\Administrator\Documents\GIt\NAV_Comment_tool\NAV_Comment_tool\TEMP\";
            //FileSplitter.SplitFile(path + @"All.txt");

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
