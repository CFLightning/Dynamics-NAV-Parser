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
            string path = @"C:\Users\Administrator\Documents\GIt\NAV_Comment_tool\NAV_Comment_tool\TEMP\";
            //string path = @"C:\Users\Administrator\Documents\Exported example objects\";
            FileSplitter.splitFile(path + "Table 18 Customer .txt"); // TODO: Change hardcoded path to dynamically chosen one
            IndentationChecker.checkIndentations();
            ModificationSearchTool.findAndSaveChanges();
            ModificationCleanerTool.cleanChangeCode();
            SaveTool.saveObjectsToFiles(path);
            SaveTool.saveChangesToFiles(path);
            //string path = @"C:\Users\Administrator\Documents\export\18cust.txt";
            //string objectTextLines = System.IO.File.ReadAllText(path);
            //string[] txt = ChangeCheck.GetTagList(objectTextLines).ToArray();
        }
    }
}
