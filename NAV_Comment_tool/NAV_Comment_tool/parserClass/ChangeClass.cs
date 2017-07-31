using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NAV_Comment_tool.parserClass
{
    class ChangeClass
    {
        private int entryNumber;
        private int objectNumber;
        private string contents;

        public int Number { get => entryNumber; set => entryNumber = value; }
        public int ObjectNumber { get => objectNumber; set => objectNumber = value; }
        public string Contents { get => contents; set => contents = value; }
    }
}
