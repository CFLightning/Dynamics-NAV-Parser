using System.Collections.Generic;

namespace NAV_Comment_tool.parserClass
{
    class ObjectClass
    {
        private int number;
        private string type;
        private string name;
        private string contents;
        public List<ChangeClass> changelog;

        public ObjectClass()
        {
            this.number = 0;
            this.type = "";
            this.name = "";
            this.contents = "";
            this.changelog = new List<ChangeClass>();
        }

        public ObjectClass(int number, string type, string name, string contents)
        {
            this.number = number;
            this.type = type;
            this.name = name;
            this.contents = contents;
            this.changelog = new List<ChangeClass>();
        }

        public int Number { get => number; set => number = value; }
        public string Type { get => type; set => type = value; }
        public string Name { get => name; set => name = value; }
        public string Contents { get => contents; set => contents = value; }
        internal List<ChangeClass> Changelog { get => changelog; set => changelog = value; }
    }
}
