using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace _MKTRT
{
    class Program
    {
        // Mario Kart Tour Renaming Tool
        static void Main(string[] args)
        {
            WebClient client = new WebClient();
            String text = client.DownloadString("http://avsys.xyz/MKT/filenames.txt");
            using (StringReader Reader = new StringReader(text))
            {
                string line;
                while ((line = Reader.ReadLine()) != null)
                {
                    String Hash = line.Split('=')[0];
                    String FileName = line.Split('=')[1];
                    string rootFolderPath = Directory.GetCurrentDirectory();
                    string filesToRename = @"*" + Hash + @"_*";
                    string[] fileList = Directory.GetFiles(rootFolderPath, filesToRename);
                    foreach (string file in fileList)
                    {
                        if (FileName.Contains('/'))
                        {
                            String test = Directory.GetParent(FileName).FullName;
                            Console.WriteLine(file + " -> " + FileName);
                            if (!Directory.Exists(test))
                            {
                                Directory.CreateDirectory(test);
                            }
                            MoveWithReplace(file, test + "/" + FileName.Split('/')[FileName.Split('/').Length - 1]);
                        }
                        else
                        {
                            MoveWithReplace(file, FileName);
                        }
                    }
                }
                Reader.Close();
            }
            Console.ReadKey();
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
