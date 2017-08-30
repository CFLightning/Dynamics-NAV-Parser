using NAV_Comment_tool.fileSplitter;
using NAV_Comment_tool.saveTool;
using NAV_Comment_tool.modificationSearchTool;
using NAV_Comment_tool.indentationChecker;
using System;

namespace NAV_Comment_tool
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Checking path, splitting files");
            Console.WriteLine(Environment.NewLine + "----------------------------------------------------------------");
            Console.WriteLine("Parameters: " + args.Length);
            for (int i = 0; i < args.Length; i++)
                Console.WriteLine(i + "\t" + args[i]);

            if (args.Length != 4)
            {
                Console.WriteLine("\nParameters error! You should pass 3 values!\n");
                Console.ReadLine();
                return;
            }

            string expectedModification = args[0];
            string inputFilePath = args[1];
            string mappingFilePath = args[2];
            string outputPath = args[3] + "\\";

            Console.WriteLine();
            Console.WriteLine("Modification name:\t" + expectedModification);
            Console.WriteLine("Path to input file:\t" + inputFilePath);
            Console.WriteLine("Path to mapping file:\t" + mappingFilePath);
            Console.WriteLine("Path to output folder:\t" + outputPath);
            Console.WriteLine("----------------------------------------------------------------" + Environment.NewLine);


            FileSplitter.SplitFile(inputFilePath);
            Console.WriteLine("Fixing indentations");
            IndentationChecker.CheckIndentations();
            Console.WriteLine("Looking for modifications");
            if (!ModificationSearchTool.FindAndSaveChanges(expectedModification))
            {
                Console.WriteLine("ERROR: Modofication {0} not found", expectedModification);
                Console.ReadLine();
                return;
            }
            Console.WriteLine("Preparing the modifications for saving");
            ModificationCleanerTool.CleanChangeCode();
            Console.WriteLine("Generating Documentation() trigger");
            DocumentationTrigger.UpdateDocumentationTrigger();

            Console.WriteLine("Saving objects");
            SaveTool.SaveObjectsToFiles(outputPath);
            Console.WriteLine("Saving changelogs");
            SaveTool.SaveChangesToFiles(outputPath);
            Console.WriteLine("Generating <next> documentation file");
            SaveTool.SaveDocumentationToFile(outputPath, DocumentationExport.GenerateDocumentationFile(outputPath,mappingFilePath));
            Console.WriteLine("Saving objects grouped");
            SaveTool.SaveObjectModificationFiles(outputPath);
            Console.WriteLine("Success");

            Console.ReadLine();
        }
    }
}
