using NAV_Comment_tool.parserClass;
using NAV_Comment_tool.repositories;
using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;

namespace NAV_Comment_tool.saveTool
{
    class SaveTool
    {
        public static bool SaveObjectsToFiles(string path)
        {
            string objPath = path + "Objects";
            DirectoryInfo directory = Directory.CreateDirectory(objPath);
            foreach (ObjectClass obj in ObjectClassRepository.objectRepository)
            {
                File.WriteAllText(path + @"\Objects\" + obj.Type + " " + obj.Number + " " + obj.Name + " .txt", obj.Contents);
            }
            return true;
        }

        public static bool SaveChangesToFiles(string path)
        {
            string modPath = path + "Modifications";
            DirectoryInfo directory = Directory.CreateDirectory(modPath);
            foreach (ChangeClass chg in ChangeClassRepository.changeRepository)
            {
                if (File.Exists(modPath + @"\Modification " + chg.ChangelogCode + " list.txt")) File.Delete(modPath + @"\Modification " + chg.ChangelogCode + " list.txt");
            }

            foreach (ChangeClass chg in ChangeClassRepository.changeRepository)
            {
                if(chg.ChangeType == "Code")
                {
                    File.AppendAllText(modPath + @"\Modification " + chg.ChangelogCode + " list.txt", "Source object: " + chg.SourceObject + Environment.NewLine);
                    File.AppendAllText(modPath + @"\Modification " + chg.ChangelogCode + " list.txt", "Change location: " + chg.Location + Environment.NewLine + Environment.NewLine);
                    File.AppendAllText(modPath + @"\Modification " + chg.ChangelogCode + " list.txt", chg.Contents);
                    File.AppendAllText(modPath + @"\Modification " + chg.ChangelogCode + " list.txt", Environment.NewLine + "----------------------------------------------------------------------------------------------------" + Environment.NewLine);
                }
            }
            return true;
        }

        private static string CleanFileName(string fileName)
        {
            return Path.GetInvalidFileNameChars().Aggregate(fileName, (current, c) => current.Replace(c.ToString(), string.Empty));
        }

        public static bool SaveObjectModificationFiles(string path)
        {
            string objModPath = path + "Modification Objects";
            DirectoryInfo directory = Directory.CreateDirectory(objModPath);
            foreach (ObjectClass obj in ObjectClassRepository.objectRepository)
            {
                List < string > changeList = new List<string>();
                changeList = obj.Changelog.Select(o => o.ChangelogCode).Distinct().ToList();

                foreach(string change in changeList)
                {
                    File.AppendAllText(CleanFileName(objModPath + @"\Objects modificated in " + change + " .txt"), obj.Contents);
                    File.AppendAllText(CleanFileName(objModPath + @"\Objects modificated in " + change + " .txt"), Environment.NewLine + "----------------------------------------------------------------------------------------------------" + Environment.NewLine);
                }
            }
            return true;
        }

        public static bool SaveDocumentationToFile(string path, string documentation)
        {
            File.WriteAllText(path + @"Documentation .txt", documentation);
            return true;
        }
    }
}
