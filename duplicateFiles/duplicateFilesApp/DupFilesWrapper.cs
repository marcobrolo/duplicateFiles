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
        
        // STORE DUPLICATES IN STRINGBUILDER FOR QUICKER STREAMING OUT CONTENTS TO FILE
        public static StringBuilder dupOutputList = new StringBuilder();
        private static Dictionary<string, List<FileInfo>> dupFileDict = new Dictionary<string,List<FileInfo>>();
        private static Dictionary<string, Dictionary<string, FileInfo>> fileListByExtension = new Dictionary<string, Dictionary<string, FileInfo>>();

        public void duplicateFilesApp()
        {

        }

        // METHOD TO PROCESS GIVEN DIRECTORY, PROCESS EACH FILE AND RECURSIVELY PROCESS SUB DIRECTORIES
        private static void processDirectories(string targetDirectory)
        {
            DirectoryInfo dirInfo = new DirectoryInfo(targetDirectory);
            FileInfo[] fileInfoArr = dirInfo.GetFiles("*");

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

            // WE WANT TO GROUP FILES BY THEIR EXTENSIONS, THAT WAY WHEN SEARCHING FOR DUPLICATES
            // WE ONLY CONSIDER FILES WITH THE SAME EXTENSIONS, MAKING IT MORE EFFICIENT IN THE SEARCH
            if( fileListByExtension.ContainsKey(targetFile.Extension) == true)
            {
                // CHECK THE DUPLiCATE FILE LIST FIRST FOR DUPLICATES, SINCE EXISTING DUPLICATES
                // HAVE A HIGHER CHANCE OF REOCCURING DUPLICATES (AND THIS IS A SHORTER LIST TO SEARCH
                // THUS IMPROVING SEARCH TIME
                if(dupFileDict.ContainsKey(fileNameHash) == true)
                {
                    dupFileDict[fileNameHash].Add(targetFile);
                }
                else
                {
                    if(fileListByExtension[targetFile.Extension].ContainsKey(fileNameHash) == true)
                    {
                        dupFileDict.Add(fileNameHash, new List<FileInfo>() { targetFile });
                        dupFileDict[fileNameHash].Add(fileListByExtension[targetFile.Extension][fileNameHash]);
                        // REMOVE FILE FROM THE FILE LIST SINCE NOW AN ENTRY LIVEs IN DUPFILEDICT AND BE
                        // CHECKED THERE
                        fileListByExtension[targetFile.Extension].Remove(fileNameHash);
                    }
                    else
                    {
                        fileListByExtension[targetFile.Extension].Add(fileNameHash, targetFile);
                    }
                }
            }
            else
            {
                Dictionary<string, FileInfo> fileList = new Dictionary<string, FileInfo>();
                fileList.Add(fileNameHash, targetFile);
                fileListByExtension.Add(targetFile.Extension, fileList);
            }
            /*
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
            }*/
        }

        // OUTPUT DUPLICATES INTO A TEXT FILE FOR LATER VIEWING
        public Dictionary<string,List<FileInfo>> getDuplicateFiles ()
        {
            processDirectories("D:\\");
            return dupFileDict; 
        }

    }
}
