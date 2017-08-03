using NAV_Comment_tool.fileSplitter;
using NAV_Comment_tool.saveTool;
using NAV_Comment_tool.indentationChecker;
using NAV_Comment_tool.modificationSearchTool;

namespace NAV_Comment_tool
{
    class Program
    {
        static void Main()
        {
            FileSplitter.splitFile(@"C:\Users\Administrator\Documents\Exported example objects\Objects.txt"); // TODO: Change hardcoded path to dynamically chosen one
            IndentationChecker.checkIndentations();
            ModificationSearchTool.findAndSaveChanges();
            SaveTool.saveToFiles();


        }
    }
}
