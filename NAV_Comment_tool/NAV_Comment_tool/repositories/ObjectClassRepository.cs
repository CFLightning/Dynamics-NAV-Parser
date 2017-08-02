using System.Collections.Generic;
using NAV_Comment_tool.parserClass;

namespace NAV_Comment_tool.repositories
{
    class ObjectClassRepository
    {
        public static List<ObjectClass> objectRepository = new List<ObjectClass>();

        public static void appendObject(ObjectClass newObject)
        {
            objectRepository.Add(newObject);
        }

        public static List<ObjectClass> fetchRepo()
        {
            return objectRepository;
        }
    }
}
