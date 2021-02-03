using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _MKTRT_A
{
    // Mario Kart Tour Renaming Tool: Asset name collector
    // This tool works by placing the program where the unpacked folders are
    // Use AssetStudio to unpack all files by going to File -> Export folder, and then select the Nabe folder
    // Use MKTRT_C before this in order to convert the new filesystem to the old one
    class Program
    {
        static void Main(string[] args)
        {
            String[] UnpackedAssetPaths = Directory.GetDirectories(Directory.GetCurrentDirectory());
            EndianWriter Writer = new EndianWriter(File.Open("!MKTRT_A_out.txt", FileMode.Create), Endianness.BigEndian);
            List<String> HashList = new List<String>();
            for (int i = 0; i < UnpackedAssetPaths.Length; i++)
            {
                if (Path.GetFileName(UnpackedAssetPaths[i]).EndsWith("_unpacked") && !HashList.Contains(Path.GetFileName(UnpackedAssetPaths[i]).Split('_')[1]))
                {
                    Writer.WriteString(Path.GetFileName(UnpackedAssetPaths[i]).Split('_')[1] + "=");
                    String[] AssetBundleFiles = Directory.GetFiles(UnpackedAssetPaths[i]);
                    String FileName = "";
                    for (int j = 0; j < AssetBundleFiles.Length; j++)
                    {
                        if (Path.GetFileName(AssetBundleFiles[j]).StartsWith("CAB-") && !Path.GetFileName(AssetBundleFiles[j]).EndsWith(".resS"))
                        {
                            FileName = FindString(AssetBundleFiles[j], new byte[] { 0x61, 0x73, 0x73, 0x65, 0x74, 0x73, 0x2F });
                        }
                        else if (Path.GetFileName(AssetBundleFiles[j]).EndsWith(".sharedAssets"))
                        {
                            FileName = FindString(AssetBundleFiles[j], new byte[] { 0x41, 0x73, 0x73, 0x65, 0x74, 0x73, 0x2F });
                        }
                    }
                    Console.WriteLine(FileName);
                    Writer.WriteString(FileName + "\r\n");
                    HashList.Add(Path.GetFileName(UnpackedAssetPaths[i]).Split('_')[1]);
                }
            }
            Writer.Close();
        }

        public static String FindString(string AssetBundleFiles, byte[] StrData)
        {
            String FileName = "";
            Byte[] FileData = File.ReadAllBytes(AssetBundleFiles);
            EndianReader Reader = new EndianReader(File.Open(AssetBundleFiles, FileMode.Open), Endianness.LittleEndian);
            Int32 NameOffset = IndexOf(FileData, StrData);
            if (NameOffset == -1)
            {
                FileName = "[Not found]";
            }
            else
            {
                Reader.Position = NameOffset - 4;
                Int32 NameLength = Reader.ReadInt32();
                FileName = Reader.ReadString(NameLength);
            }
            Reader.Close();
            return FileName;
        }

        public static int IndexOf(byte[] arrayToSearchThrough, byte[] patternToFind)
        {
            if (patternToFind.Length > arrayToSearchThrough.Length)
                return -1;
            for (int i = 0; i < arrayToSearchThrough.Length - patternToFind.Length; i++)
            {
                bool found = true;
                for (int j = 0; j < patternToFind.Length; j++)
                {
                    if (arrayToSearchThrough[i + j] != patternToFind[j])
                    {
                        found = false;
                        break;
                    }
                }
                if (found)
                {
                    return i;
                }
            }
            return -1;
        }
    }
}
