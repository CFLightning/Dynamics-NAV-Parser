using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NAV_Comment_tool.parserClass.ObjectClass
{
    class ObjectClass
    {
        private int number;
        private int type;
        private string name;
        // private List<ChangeClass>

        public int Number { get => number; set => number = value; }
        public int Type { get => type; set => type = value; }
        public string Name { get => name; set => name = value; }
    }
}
