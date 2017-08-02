using NAV_Comment_tool.parserClass;
using NAV_Comment_tool.repositories;
using System.IO;

namespace NAV_Comment_tool.saveTool
{
    class SaveTool
    {
        public static bool saveToFiles()
        {
            foreach(ObjectClass obj in ObjectClassRepository.objectRepository)
            {
                File.WriteAllText(@"C:\Users\Administrator\Documents\Exported example objects\" + obj.Type + " " + obj.Number + " " + obj.Name + " .txt", obj.Contents);
            }
            
            return true;
        }
    }
}
