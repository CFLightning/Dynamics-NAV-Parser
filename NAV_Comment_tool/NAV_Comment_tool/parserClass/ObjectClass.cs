using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NAV_Comment_tool.parserClass
{
    class ObjectClass
    {
        private int number;
        private int type;
        private string name;
        private List<ChangelogClass> changelog;

        public int Number { get => number; set => number = value; }
        public int Type { get => type; set => type = value; }
        public string Name { get => name; set => name = value; }
        internal List<ChangelogClass> Changelog { get => changelog; set => changelog = value; }
    }
}
