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
            //FileSplitter.splitFile(@"C:\Users\Administrator\Documents\export\18cust.txt"); // TODO: Change hardcoded path to dynamically chosen one
            //IndentationChecker.checkIndentations();
            //ModificationSearchTool.findAndSaveChanges();
            //SaveTool.saveToFiles();

            string path = @"C:\Users\Administrator\Documents\export\18cust.txt";
            string objectTextLines = System.IO.File.ReadAllText(path);
            string[] txt = ChangeCheck.GetModyficationList(objectTextLines).ToArray();
        }
    }
}
