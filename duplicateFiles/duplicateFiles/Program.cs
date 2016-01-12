using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

/*
 * 
 * Using System.IO library to parse file system for duplicate files
 * 
 */
namespace duplicateFiles
{
    class Program
    {
        public static Dictionary<string, string> fileDict = new Dictionary<string,string>();

        static void Main(string[] args)
        {
            DriveInfo[] allDrives = DriveInfo.GetDrives();

            foreach (DriveInfo drive in allDrives)
            {
                Console.WriteLine("Drive {0}, {1}", drive.Name, drive.IsReady);

                if (drive.IsReady == true)
                {
                    processDirectories( drive.Name);
                }
            }

            Console.ReadLine();
        }

        // METHOD TO PROCESS GIVEN DIRECTORY, PROCESS EACH FILE AND RECURSIVELY PROCESS SUB DIRECTORIES
        public static void processDirectories(string targetDirectory)
        {
            string[] files = System.IO.Directory.GetFiles(targetDirectory,"*.jpg");
            
            foreach (string fileName in files)
            {
                //Console.WriteLine("File{0}", fileName);
                processFile(fileName);
            }

            string[] subDirectories = System.IO.Directory.GetDirectories(targetDirectory);
            foreach (string subDirName in subDirectories)
            {
                // TRY TO PROCESS THE DIRECTORY, IF THERE IS AN EXCEPTION (USUALLY UNAUTH ACCESS), RESUME
                try {
                    processDirectories(subDirName);
                }
                catch
                {

                }
            }
        }

        public static void processFile(string targetFile)
        {
            string fileName = Path.GetFileNameWithoutExtension(targetFile);
           // Console.WriteLine("File: {0}", fileName);

            if(fileDict.ContainsKey(fileName) == true)
            {
                Console.WriteLine("{0},\n{1},\n{2}\n", fileName, fileDict[fileName], targetFile);
            }
            else
            {
                fileDict.Add(fileName, targetFile);
            }
        }
    }
}
