using NAV_Comment_tool.fileSplitter;
using NAV_Comment_tool.saveTool;
using NAV_Comment_tool.modificationSearchTool;
using NAV_Comment_tool.indentationChecker;

namespace NAV_Comment_tool
{
    class Program
    {
        static void Main()
        {

            string path = @"C:\Users\Administrator\Documents\GIt\NAV_Comment_tool\NAV_Comment_tool\TEMP\";
            FileSplitter.SplitFile(path + "ITWS_Test_01.08.2017.txt");
            //string path = @"C:\Users\Administrator\Documents\Exported example objects\";
            //FileSplitter.SplitFile(path + "Objects.txt"); // TODO: Change hardcoded path to dynamically chosen one
            IndentationChecker.CheckIndentations();
            ModificationSearchTool.FindAndSaveChanges();
            ModificationCleanerTool.CleanChangeCode();
            DocumentationTrigger.UpdateDocumentationTrigger();
            DocumentationExport.GenerateDocumentationFile();
            SaveTool.SaveObjectsToFiles(path);
            SaveTool.SaveChangesToFiles(path);
        }
    }
}
