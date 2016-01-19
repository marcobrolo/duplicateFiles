using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace duplicateFilesApp
{
    class DupFilesWrapper
    {
        public static Dictionary<string, FileInfo> fileList = new Dictionary<string, FileInfo>();
        // STORE DUPLICATES IN STRINGBUILDER FOR QUICKER STREAMING OUT CONTENTS TO FILE
        public static StringBuilder dupOutputList = new StringBuilder();
        private static Dictionary<string, List<FileInfo>> dupFileDict = new Dictionary<string,List<FileInfo>>();

        public void duplicateFilesApp()
        {

        }

        // METHOD TO PROCESS GIVEN DIRECTORY, PROCESS EACH FILE AND RECURSIVELY PROCESS SUB DIRECTORIES
        private static void processDirectories(string targetDirectory)
        {
            DirectoryInfo dirInfo = new DirectoryInfo(targetDirectory);
            FileInfo[] fileInfoArr = dirInfo.GetFiles("*.jpg");

            //string[] files = System.IO.Directory.GetFiles(targetDirectory, "*.jpg");

            foreach (FileInfo file in fileInfoArr)
            {
                Console.WriteLine("File {0}", file.Name + file.Length);
                processFile(file);
            }

            string[] subDirectories = System.IO.Directory.GetDirectories(targetDirectory);
            foreach (string subDirName in subDirectories)
            {
                // TRY TO PROCESS THE DIRECTORY, IF THERE IS AN EXCEPTION (USUALLY UNAUTH ACCESS), RESUME
                try
                {
                    processDirectories(subDirName);
                }
                catch
                {

                }
            }
        }

        private static void processFile(FileInfo targetFile)
        {
            string fileNameHash = targetFile.Name + "_." + targetFile.Length;
            // Console.WriteLine("File: {0}", fileName);

            if (fileList.ContainsKey(fileNameHash) == true)
            {
                // Console.WriteLine("{0},\n{1},\n{2}\n", fileName, fileDict[fileName], targetFile);
                // dupFileDict.Add(fileDict[targetFile], targetFile);
                if (dupFileDict.ContainsKey(fileNameHash))
                {
                    dupFileDict[fileNameHash].Add(targetFile);
                }
                else
                {
                    dupFileDict.Add(fileNameHash, new List<FileInfo>() { targetFile });
                    dupFileDict[fileNameHash].Add(fileList[fileNameHash]);
                }
            }
            else
            {
                fileList.Add(fileNameHash, targetFile);
            }
        }

        // OUTPUT DUPLICATES INTO A TEXT FILE FOR LATER VIEWING
        public Dictionary<string,List<FileInfo>> getDuplicateFiles ()
        {
            processDirectories("D:\\");
            return dupFileDict; 
        }

    }
}
