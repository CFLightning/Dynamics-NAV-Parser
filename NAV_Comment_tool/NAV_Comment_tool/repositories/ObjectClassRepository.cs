using System;
using System.Collections.Generic;
using System.Linq;
using NAV_Comment_tool.parserClass;
using System.Text;
using System.Threading.Tasks;

namespace NAV_Comment_tool.repositories
{
    class ObjectClassRepository
    {
        private static List<ObjectClass> objectRepository = new List<ObjectClass>();

        public static void appendObject(ObjectClass newObject)
        {
            objectRepository.Add(newObject);
        }

        public static void listAll()
        {
            foreach(ObjectClass item in objectRepository.ToArray()) {
                Console.WriteLine(item.Contents);
            }
            
        }
    }
}
