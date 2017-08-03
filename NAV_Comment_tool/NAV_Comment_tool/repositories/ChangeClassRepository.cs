using NAV_Comment_tool.parserClass;
using System.Collections.Generic;

namespace NAV_Comment_tool.repositories
{
    class ChangeClassRepository
    {
            public static List<ChangeClass> changeRepository = new List<ChangeClass>();

            public static void appendChange(ChangeClass newChange)
            {
                changeRepository.Add(newChange);
            }
        
    }
}

