using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NAV_Comment_tool.parserClass
{
    class ChangelogClass
    {
        private string changeTag;
        private List<ChangeClass> changes;

        public string ChangeTag { get => changeTag; set => changeTag = value; }
        internal List<ChangeClass> Changes { get => changes; set => changes = value; }
    }
}
