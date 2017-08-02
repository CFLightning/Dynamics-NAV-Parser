using NAV_Comment_tool.parserClass;
using System.Collections.Generic;
using System.IO;

namespace NAV_Comment_tool.saveTool
{
    class SaveTool
    {
        public static bool saveToFiles(ObjectClass obj)
        {
            File.WriteAllText(@"C:\Users\Administrator\Documents\Exported example objects\" + obj.Name + " " + obj.Number + ".txt", obj.Contents);
           
            return true;
        }
    }
}
