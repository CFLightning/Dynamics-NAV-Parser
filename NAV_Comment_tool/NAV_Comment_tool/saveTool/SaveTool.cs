﻿using NAV_Comment_tool.parserClass;
using NAV_Comment_tool.repositories;
using System;
using System.IO;

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
                if (File.Exists(modPath + chg.ChangelogCode + " .txt")) File.Delete(modPath + chg.ChangelogCode + " .txt");
            }

            foreach (ChangeClass chg in ChangeClassRepository.changeRepository)
            {
                if(chg.ChangeType != "Field")
                {
                    File.AppendAllText(modPath + @"\Modification " + chg.ChangelogCode + " list.txt", "Source object: " + chg.SourceObject + Environment.NewLine);
                    File.AppendAllText(modPath + @"\Modification " + chg.ChangelogCode + " list.txt", "Change location: " + chg.Location + Environment.NewLine + Environment.NewLine);
                    File.AppendAllText(modPath + @"\Modification " + chg.ChangelogCode + " list.txt", chg.Contents);
                    File.AppendAllText(modPath + @"\Modification " + chg.ChangelogCode + " list.txt", Environment.NewLine + Environment.NewLine);
                }
            }
            return true;
        }
    }
}
