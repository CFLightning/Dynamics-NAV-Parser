using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NAV_Comment_tool.fileSplitter
{
    class ExtractObjectParts
    {
        private string text;
        private string[] textLines;
        string title_type;
        int title_no;
        string title_name;
        string part_object_properties;
        string part_properties;
        string part_fields;
        string part_keys;
        string part_fieldgroups;
        string part_code;


        public ExtractObjectParts(string path)
        {
            LoadFile(path);
        }

        public void LoadFile(string path)
        {
            text = System.IO.File.ReadAllText(@"C:\Users\Administrator\Documents\GIt\NAV_Comment_tool\NAV_Comment_tool\TEMP\tab5.txt");
            textLines = text.Replace("\r", "").Split('\n');
        }

        public void GetTitleInfo()
        {
            string line = textLines[0];
            string[] titleElements = line.Split();
            title_type = titleElements[1];
            title_no = Int32.Parse(titleElements[2]);
            title_name = titleElements[3];
            for (int i = 4; i < titleElements.Length; i++)
                title_name += " " + titleElements[i];

            //Console.WriteLine(line);
            //Console.WriteLine("type: " + type);
            //Console.WriteLine("no. : " + no);
            //Console.WriteLine("name: " + name);
        }

        public void ExtractParts()
        {
            string[] lines = text.Replace("\r", "").Split('\n');
            int i = 1;
            do
            {
                ;
            } while (lines[++i] != "  OBJECT-PROPERTIES");
            do
            {
                part_object_properties += lines[i] + Environment.NewLine;
            } while (lines[++i] != "  PROPERTIES");
            do
            {
                part_properties += lines[i] + Environment.NewLine;
            } while (lines[++i] != "  FIELDS");
            do
            {
                part_fields += lines[i] + Environment.NewLine;
            } while (lines[++i] != "  KEYS");
            do
            {
                part_keys += lines[i] + Environment.NewLine;
            } while (lines[++i] != "  FIELDGROUPS");
            do
            {
                part_fieldgroups += lines[i] + Environment.NewLine;
            } while (lines[++i] != "  CODE");
            do
            {
                part_code += lines[i] + Environment.NewLine;
            } while (++i != lines.Length);

            //Console.Write(object_properties);
            //Console.Write(properties);
            //Console.Write(fields);
            //Console.Write(keys);
            //Console.Write(object_properties);
            //Console.Write(code);
        }
    }
}
