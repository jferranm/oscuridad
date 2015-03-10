using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using Frankfort.VBug.Internal;


namespace Frankfort.VBug.Editor
{
    public static class IOEditorHelper
    {

        private static string projectRoot;
        private static List<string> createdFolderPaths = new List<string>();




        //--------------------------------------- FILE PREPERATION --------------------------------------
        //--------------------------------------- FILE PREPERATION --------------------------------------
        #region FILE PREPERATION


        public static string PrepairDestinationPath(string fullFilePath, bool cachePath) {
            if (string.IsNullOrEmpty(fullFilePath))
                return fullFilePath;

            fullFilePath = IOSharedHelper.EscapePath(fullFilePath);
            char seperator = IOSharedHelper.GetDirectorySeperator();
            string[] folders = fullFilePath.Split(seperator);

            if (folders == null || folders.Length < 1)
                return fullFilePath;

            string rebuildPath = folders[0];
            for (int i = 1; i < folders.Length - 1; i++) {
                rebuildPath += seperator + folders[i];
                if (cachePath && createdFolderPaths.Contains(rebuildPath))
                    continue;
                
            if (!System.IO.Directory.Exists(rebuildPath))
                System.IO.Directory.CreateDirectory(rebuildPath);

                if (cachePath)
                    createdFolderPaths.Add(rebuildPath);
            }
            return fullFilePath;
        }


        #endregion
        //--------------------------------------- FILE PREPERATION --------------------------------------
        //--------------------------------------- FILE PREPERATION --------------------------------------
			












        //--------------------------------------- DIRECTORIES --------------------------------------
        //--------------------------------------- DIRECTORIES --------------------------------------
        #region DIRECTORIES

        public static string GetProjectRoot()
        {
            if (!string.IsNullOrEmpty(projectRoot))
                return projectRoot;

            string dataPath = Application.dataPath.Replace('\\', '/');
            projectRoot = dataPath.Substring(0, dataPath.LastIndexOf('/'));
            return projectRoot;
        }


        public static string[] GetUserDirs(string vbugPersistDir)
        {
            return GetFilteredDirs(vbugPersistDir);
        }
        public static string[] GetUserDateDirs(string userDir)
        {
            return GetFilteredDirs(userDir, "Mon", "Tue", "Wed", "Thu", "Fri", "Sat", "Sun");
        }
        public static string[] GetDateSessionDirs(string dateDir)
        {
            return GetFilteredDirs(dateDir, "Session_");
        }
        public static string[] GetSessionsMinutesDirs(string sessionDir)
        {
            return GetFilteredDirs(sessionDir, "AM", "PM");
        }



        public static string[] GetFilteredDirs(string path, params string[] include)
        {
            if (string.IsNullOrEmpty(path) || !Directory.Exists(path))
                return null;

            try
            {
                string[] dirs = IOSharedHelper.EscapePaths(Directory.GetDirectories(path));
                if (include == null || include.Length == 0)
                    return dirs;

                string[] names = GetDirectoryNames(dirs);
                List<string> filtered = new List<string>();
                int i = dirs.Length;

                while (--i > -1)
                {
                    string name = names[i];
                    string dir = dirs[i];
                    int j = include.Length;

                    while (--j > -1)
                    {
                        if (name.Contains(include[j]))
                        {
                            filtered.Add(dir);
                            break;
                        }
                    }
                }

                string[] sorted = filtered.ToArray();
                SortPathByDate(sorted, true);
                return sorted;
            } catch (Exception e) {
                if (vBug.settings.general.debugMode)
                    Debug.LogError(e.Message + e.StackTrace);
            }
            return null;
        }



        public static void SortPathByDate(string[] input, bool isDirectory)
        {
            int iMax = input.Length;
            double[] dates = new double[iMax];
            for(int i = 0; i < iMax; i++)
            {
                if(isDirectory)
                {
                    if (Directory.Exists(input[i]))
                        dates[i] = Directory.GetCreationTime(input[i]).Subtract(vBugEnvironment.identityTimeStamp).TotalSeconds;
                }
                else
                {
                    if (File.Exists(input[i]))
                        dates[i] = File.GetCreationTime(input[i]).Subtract(vBugEnvironment.identityTimeStamp).TotalSeconds;
                }
            }

            int iterations = iMax;
            int iMaxMinus = iMax - 1;
            while(--iterations > -1)
            {
                for (int j = 0; j < iMaxMinus; j++)
                {
                    int jplus = j + 1;
                    if(dates[j] < dates[jplus])
                    {
                        string tmpS = input[j];
                        input[j] = input[jplus];
                        input[jplus] = tmpS;

                        double tmpD = dates[j];
                        dates[j] = dates[jplus];
                        dates[jplus] = tmpD;
                    }
                }
            }
        }

        #endregion
        //--------------------------------------- DIRECTORIES --------------------------------------
        //--------------------------------------- DIRECTORIES --------------------------------------











        //--------------------------------------- FILES --------------------------------------
        //--------------------------------------- FILES --------------------------------------
        #region FILES

        public static string[] GetVBugSliceLocations(string[] minutesDirs)
        {
            List<string> result = new List<string>();

            for (int i = 0; i < minutesDirs.Length; i ++ )
            {
                string dir = minutesDirs[i];
                if(!Directory.Exists(dir))
                    continue;

                string[] files = IOSharedHelper.EscapePaths(Directory.GetFiles(dir));

                for(int j = 0; j < files.Length; j++)
                {
                    if (CheckExtention(files[j], "vBugSlice"))
                        result.Add(files[j]);
                }
            }
            return result.ToArray();
        }

        private static bool CheckExtention(string path, string extention)
        {
            if (string.IsNullOrEmpty(path) || path.Length == 0)
                return false;

            string[] buff = path.Split('.');
            if (buff == null || buff.Length < 2)
                return false;

            return buff[buff.Length - 1] == extention;
        }

        #endregion
        //--------------------------------------- FILES --------------------------------------
        //--------------------------------------- FILES --------------------------------------












        


        //--------------------------------------- LOADING --------------------------------------
        //--------------------------------------- LOADING --------------------------------------
        #region LOADING


        public static VerticalActivitySlice LoadVBugSliceFromDisk(string sliceLocation)
        {
            if (string.IsNullOrEmpty(sliceLocation) || !File.Exists(sliceLocation))
                return null;

            try {
                VerticalActivitySlice result = VerticalActivitySlice.Deserialize(File.ReadAllBytes(sliceLocation));
                ComposedByteStream.BlitBuffers();
                return result;
            } catch(Exception e) {
                if (vBug.settings.general.debugMode)
                    Debug.LogError(e.Message + e.StackTrace);
            }
            return null;
        }


        //TODO: Spread over time using a Thread;
        public static VerticalActivitySlice[] LoadVBugSlicesFromDisk(string sessionPath )
        {
            if (string.IsNullOrEmpty(sessionPath) || !Directory.Exists(sessionPath))
                return null;

            string[] allSlicePaths = GetVBugSliceLocations( GetSessionsMinutesDirs(sessionPath));
            if (allSlicePaths == null || allSlicePaths.Length == 0)
                return null;


            int count = 0;
            int iMax = allSlicePaths.Length;
            //iMax = 1;
            VerticalActivitySlice[] result = new VerticalActivitySlice[iMax];

            for(int i = 0; i < iMax; i ++)
            {
                string filePath = allSlicePaths[i];
                if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
                    continue;

                try {
                    result[count] = VerticalActivitySlice.Deserialize(File.ReadAllBytes(filePath));
                    ComposedByteStream.BlitBuffers();
                    count ++;
                } catch(Exception e) {
                    if (vBug.settings.general.debugMode)
                        Debug.LogError(e.Message + e.StackTrace);
                    break;
                }
            }

            if(count != result.Length)
                Array.Resize(ref result, count);

            Array.Sort<VerticalActivitySlice>(result, SortByFrameNumber);
            return result;
        }

        private static int SortByFrameNumber(VerticalActivitySlice a, VerticalActivitySlice b)
        {
            return a.header.frameNumber.CompareTo(b.header.frameNumber);
        }

        #endregion
        //--------------------------------------- LOADING --------------------------------------
        //--------------------------------------- LOADING --------------------------------------








        //--------------------------------------- WRITING / LOADING --------------------------------------
        //--------------------------------------- WRITING / LOADING --------------------------------------
        #region STRING WRITING / LOADING

        public static void WriteAllBytes(string path, byte[] data, bool cachePath = true) { 
            try {
                path = PrepairDestinationPath(path, cachePath);
                System.IO.File.WriteAllBytes(path, data);
            } catch (Exception e) {
                if (vBug.settings.general.debugMode)
                    Debug.Log(e.Message + e.StackTrace);
            }
        }


        public static void WriteString(string saveString, string path, bool cachePath = false) {
            PrepairDestinationPath(path, cachePath);
            System.IO.StreamWriter fileWriter = System.IO.File.CreateText(path);
            fileWriter.Write(saveString);
            fileWriter.Close();
        }


        public static string LoadString(string path) {
            if (System.IO.File.Exists(path))
                return System.IO.File.ReadAllText(path, System.Text.Encoding.UTF8);
            return null;
        }

        #endregion
        //--------------------------------------- WRITING / LOADING --------------------------------------
        //--------------------------------------- WRITING / LOADING --------------------------------------
			






        //--------------------------------------- MISC --------------------------------------
        //--------------------------------------- MISC --------------------------------------
        #region MISC


        public static string AbreviatePaths(string folderName, int maxCharacters)
        {
            if (folderName == null || folderName.Length <= Math.Min(maxCharacters, 15))
                return folderName;

            int endSize = maxCharacters - 10;
            return folderName.Substring(0, 7) + "..." + folderName.Substring(folderName.Length - endSize, endSize);
        }


        public static string[] GetDirectoryNames(string[] paths){
            if (paths == null)
                return null;

            int i = paths.Length;
            string[] result = new string[i];
            while (--i > -1)
                result[i] = GetDirectoryName(paths[i]);

            return result;
        }

        public static string GetDirectoryName(string path)
        {
            if (string.IsNullOrEmpty(path) || path.Length == 0)
                return null;

            path = path.Replace('\\', '/');
            int idx = path.LastIndexOf('/') + 1;
            return path.Substring(idx, path.Length - idx);
        }


        public static string FindFileByName(string dir, string searchName, bool matchCase = true, bool scanRecursive = true, bool removeAppDataPath = true)
        {
            if (string.IsNullOrEmpty(dir))
                return null;
            
            dir = IOSharedHelper.EscapePath(dir);
            if (!Directory.Exists(dir))
                return null;

            if(!matchCase)
                searchName = searchName.ToLower();


            string[] files = IOSharedHelper.EscapePaths(Directory.GetFiles(dir));
            if(files != null && files.Length > 0)
            {
                int i = files.Length;
                while(--i > -1)
                {
                    string path = files[i];
                    int start = path.LastIndexOf('/') + 1;
                    string fullFile = path.Substring(start, path.Length - start);

                    if (!matchCase)
                        fullFile = fullFile.ToLower();


                    string fileName = fullFile;

                    int maxIdx = fullFile.LastIndexOf('.');
                    if(maxIdx != -1)
                        fileName = fullFile.Substring(0, maxIdx);

                    if (fileName == searchName)
                    {
                        if(removeAppDataPath && path.Contains(GetProjectRoot()))
                            return path.Replace(GetProjectRoot() + "/", string.Empty);

                        return path;
                    }
                }
            }

            if(scanRecursive)
            {
                string[] subDirs = IOSharedHelper.EscapePaths(Directory.GetDirectories(dir));
                if(subDirs != null && subDirs.Length > 0)
                {
                    int i = subDirs.Length;
                    while(--i > -1)
                    {
                        string pathFound = FindFileByName(subDirs[i], searchName, matchCase, scanRecursive, removeAppDataPath);
                        if (!string.IsNullOrEmpty(pathFound))
                            return pathFound;
                    }
                }
            }

            return null;
        }

        #endregion
        //--------------------------------------- MISC --------------------------------------
        //--------------------------------------- MISC --------------------------------------





        internal static void OpenInExplorer(string folderPath) {
            try {
                folderPath = folderPath.Replace('/', '\\');
                System.Diagnostics.Process.Start("explorer.exe", folderPath);
            } catch (Exception e) {
                if (vBugEditorSettings.DebugMode)
                    Debug.LogError(e.Message + e.StackTrace);
            }
        }
    }
}