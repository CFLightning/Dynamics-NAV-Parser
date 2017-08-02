using System.Collections.Generic;

namespace NAV_Comment_tool.parserClass
{
    class ObjectClass
    {
        private int number;
        private string type;
        private string name;
        private string contents;
        private List<ChangelogClass> changelog;

        public ObjectClass()
        {
            this.number = 0;
            this.type = "";
            this.name = "";
            this.contents = "";
        }
        public ObjectClass(int number, string type, string name, string contents)
        {
            this.number = number;
            this.type = type;
            this.name = name;
            this.contents = contents;
        }
        public int Number { get => number; set => number = value; }
        public string Type { get => type; set => type = value; }
        public string Name { get => name; set => name = value; }
        public string Contents { get => contents; set => contents = value; }
        internal List<ChangelogClass> Changelog { get => changelog; set => changelog = value; }
    }
}
