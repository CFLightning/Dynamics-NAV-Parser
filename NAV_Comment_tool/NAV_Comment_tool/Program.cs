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
            string path = @"C:\Users\Administrator\Documents\GIt\NAV_Comment_tool\NAV_Comment_tool\TEMP\";
            FileSplitter.splitFile(path + "ITWS_Test_01.08.2017.txt");
            //string path = @"C:\Users\Administrator\Documents\Exported example objects\";
            //FileSplitter.splitFile(path + "Objects.txt"); // TODO: Change hardcoded path to dynamically chosen one
            IndentationChecker.checkIndentations();
            ModificationSearchTool.findAndSaveChanges();
            ModificationCleanerTool.cleanChangeCode();
            DocumentationTrigger.updateDocumentationTrigger();
            SaveTool.saveObjectsToFiles(path);
        }
    }
}
