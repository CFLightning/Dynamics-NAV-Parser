using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NAV_Comment_tool.fileSplitter
{
    class ExtractObjectParts
    {
        private string[] objectTextLines;

        string[] part_objectProperties;
        string[] part_properties;
        string[] part_fields;
        string[] part_keys;
        string[] part_fieldgroups;
        string[] part_code;

        string title_type;
        int title_no;
        string title_name;
        List<string> versionList;


        //fileSplitter.ExtractObjectParts Obj = new ExtractObjectParts(@"C:\Users\Administrator\Documents\GIt\NAV_Comment_tool\NAV_Comment_tool\TEMP\18cust.txt");
        public ExtractObjectParts(string path)
        {
            LoadFile(path);
            ExtractParts();
            FillTitleInfo();
            FillVersionList();
            //FindPropertiesTriggers();
        }

        public List<string> GetVersionList()
        {
            return versionList;
        }

        public void LoadFile(string path)
        {
            string objectText = System.IO.File.ReadAllText(path);
            objectTextLines = objectText.Replace("\r", "").Split('\n');
        }


        private void FillTitleInfo()
        {
            string line = objectTextLines[0];
            string[] titleElements = line.Split();
            title_type = titleElements[1];
            title_no = Int32.Parse(titleElements[2]);
            title_name = titleElements[3];
            for (int i = 4; i < titleElements.Length; i++)
                title_name += " " + titleElements[i];
        }

        private void ExtractParts()
        {
            List<string> tempPartLines = new List<string>();
            int i = 1;
            do
            {
                ;
            } while (objectTextLines[++i] != "  OBJECT-PROPERTIES");
            do
            {
                tempPartLines.Add(objectTextLines[i]);
            } while (objectTextLines[++i] != "  PROPERTIES");
            part_objectProperties = tempPartLines.ToArray();
            tempPartLines.Clear();
            do
            {
                tempPartLines.Add(objectTextLines[i]);
            } while (objectTextLines[++i] != "  FIELDS");
            part_properties = tempPartLines.ToArray();
            tempPartLines.Clear();
            do
            {
                tempPartLines.Add(objectTextLines[i]);
            } while (objectTextLines[++i] != "  KEYS");
            part_fields = tempPartLines.ToArray();
            tempPartLines.Clear();
            do
            {
                tempPartLines.Add(objectTextLines[i]);
            } while (objectTextLines[++i] != "  FIELDGROUPS");
            part_keys = tempPartLines.ToArray();
            do
            {
                tempPartLines.Add(objectTextLines[i]);
            } while (objectTextLines[++i] != "  CODE");
            part_fieldgroups = tempPartLines.ToArray();
            tempPartLines.Clear();
            do
            {
                tempPartLines.Add(objectTextLines[i]);
            } while (++i != objectTextLines.Length);
            part_code = tempPartLines.ToArray();
            tempPartLines.Clear();

            //foreach (var item in part_code)
            //    Console.WriteLine(item);
        }

        private void FillVersionList()
        {
            foreach (var line in part_objectProperties)
            {
                if (line.Contains("Version List="))
                {
                    versionList = line.Substring(line.IndexOf('=')+1).Split(',').ToList();
                }
            }
            versionList[versionList.Count - 1] = versionList.Last().Substring(0, versionList.Last().Length - 1);
        }

        private void FindPropertiesTriggers()
        {
            string[] triggerList = new string[4] {
                "OnInsert",
                "OnModify",
                "OnDelete",
                "OnRename" };
            int[] triggerPosition = new int[4];
            int triggerIdx = 0;

            for (int i = 0; i < part_properties.Length; i++)
            {
                if (triggerIdx < 4 && part_properties[i].StartsWith("    " + triggerList[triggerIdx] + "="))
                {
                    while (!part_properties[i].Contains("BEGIN")) i++;

                    triggerPosition[triggerIdx] = i;
                    triggerIdx++;
                }
            }
            
            for (triggerIdx = 0; triggerIdx < 4; triggerIdx++)
            {
                for (int line = triggerPosition[triggerIdx]; part_properties[line-1] != "             END;"; line++)
                {
                    Console.WriteLine(triggerList[triggerIdx] + ":" + part_properties[line]);
                }
            }
        }
    }
}
