using NAV_Comment_tool.parserClass;
using NAV_Comment_tool.repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NAV_Comment_tool.saveTool
{
    class DocumentationExport
    {
        enum Types { Table, Codeunit }; // Add actual numbers for each of the parameters

        public static bool GenerateDocumentationFile()
        {
            Types result;
            foreach (ObjectClass obj in ObjectClassRepository.objectRepository)
            { 
                if(Enum.TryParse(obj.Type, out result))
                {
                    Console.WriteLine("Converted {0} to {1}", obj.Type, (int)result);
                }
            }
            return true;
        }
    }
}
