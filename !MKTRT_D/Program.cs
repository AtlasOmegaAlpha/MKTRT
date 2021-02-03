using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _MKTRT_D
{
    class Program
    {
        // Mario Kart Tour Renaming Tool: File Deleter
        // This tool cleans the folder where the program is executing by deleting all files that couldn't be renamed
        static void Main(string[] args)
        {
            using (StringReader Reader = new StringReader(File.ReadAllText(args[0])))
            {
                string line;
                while ((line = Reader.ReadLine()) != null)
                {
                    string rootFolderPath = Directory.GetCurrentDirectory();
                    string filesToDelete = @"*_" + line.Split('=')[0] + @"_*";
                    string[] fileList = Directory.GetFiles(rootFolderPath, filesToDelete);
                    foreach (string file in fileList)
                    {
                        Console.WriteLine(file);
                        File.Delete(file);
                    }
                }
            }
            Console.ReadKey();
        }
    }
}
