using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _MKTRT_C
{
    class Program
    {
        // Mario Kart Tour Renaming Tool: Version converter
        // This tool converts the new filesystem to the old one, given that this tool can only unpack files in the old format
        static void Main(string[] args)
        {
            string[] Paths = Directory.GetDirectories(Directory.GetCurrentDirectory());
            if (!Directory.Exists(Directory.GetCurrentDirectory() + "/_out/"))
            {
                Directory.CreateDirectory(Directory.GetCurrentDirectory() + "/_out/");
            }
            foreach (string folder in Paths)
            {
                Console.WriteLine(Path.GetFileName(folder));
                if (Path.GetFileName(folder).StartsWith("a") || Path.GetFileName(folder) == "b")
                {
                    string[] files = Directory.GetFiles(folder);
                    foreach (string file in files)
                    {
                        string newName = Path.GetFileName(file)[0] + "_" + Path.GetFileName(file).Substring(1).TrimStart('0').PadLeft(1, '0');
                        Console.WriteLine(Path.GetFileName(file) + " -> " + newName);
                        MoveWithReplace(file, Directory.GetCurrentDirectory() + "/_out/" + newName);
                    }
                    if (!Directory.EnumerateFileSystemEntries(folder).Any())
                    {
                        Directory.Delete(folder);
                    }
                }
            }
        }
        public static void MoveWithReplace(string sourceFileName, string destFileName)
        {
            if (File.Exists(destFileName))
            {
                File.Delete(destFileName);
            }
            File.Move(sourceFileName, destFileName);
        }
    }
}
