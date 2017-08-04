using NAV_Comment_tool.parserClass;
using NAV_Comment_tool.repositories;
using System.IO;

namespace NAV_Comment_tool.saveTool
{
    class SaveTool
    {
        public static bool saveToFiles(string path)
        {
            foreach(ObjectClass obj in ObjectClassRepository.objectRepository)
            {
                File.WriteAllText(path + obj.Type + " " + obj.Number + " " + obj.Name + " .txt", obj.Contents);
            }

            foreach(ChangeClass chg in ChangeClassRepository.changeRepository)
            {
                File.AppendAllText(path + chg.ChangelogCode + " .txt", chg.Contents);
                File.AppendAllText(path + chg.ChangelogCode + " .txt", "\n\n\n");
            }
            return true;
        }
    }
}
