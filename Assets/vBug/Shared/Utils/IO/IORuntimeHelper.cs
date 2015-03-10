using System;
using UnityEngine;
using System.Collections.Generic;
using Frankfort.VBug;


namespace Frankfort.VBug.Internal {
    public static class IORuntimeHelper {


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

#if !UNITY_WEBPLAYER && !UNITY_FLASH
            if (!System.IO.Directory.Exists(rebuildPath))
                System.IO.Directory.CreateDirectory(rebuildPath);
#endif
                if (cachePath)
                    createdFolderPaths.Add(rebuildPath);
            }
            return fullFilePath;
        }


        #endregion
        //--------------------------------------- FILE PREPERATION --------------------------------------
        //--------------------------------------- FILE PREPERATION --------------------------------------
			












        //--------------------------------------- WRITING / LOADING --------------------------------------
        //--------------------------------------- WRITING / LOADING --------------------------------------
        #region STRING WRITING / LOADING

        public static void WriteAllBytes(string path, byte[] data, bool cachePath = true) {

#if !UNITY_WEBPLAYER && !UNITY_FLASH
        try {
            path = IORuntimeHelper.PrepairDestinationPath(path, cachePath);
            System.IO.File.WriteAllBytes(path, data);
        } catch (Exception e) {
            if (vBug.settings.general.debugMode)
                Debug.Log(e.Message + e.StackTrace);
        }
#else
            Debug.LogWarning("IO-operations (read/write to disk) not allowed on this platform");
#endif
        }


        public static void WriteString(string saveString, string path, bool cachePath = false) {
#if !UNITY_WEBPLAYER && !UNITY_FLASH
            IORuntimeHelper.PrepairDestinationPath(path, cachePath);
            System.IO.StreamWriter fileWriter = System.IO.File.CreateText(path);
            fileWriter.Write(saveString);
            fileWriter.Close();
#else
            Debug.LogWarning("IO-operations (read/write to disk) not allowed on this platform");
#endif
        }


        public static string LoadString(string path) {
#if !UNITY_WEBPLAYER && !UNITY_FLASH
        if (System.IO.File.Exists(path))
            return System.IO.File.ReadAllText(path, System.Text.Encoding.UTF8);
#else
            Debug.LogWarning("IO-operations (read/write to disk) not allowed on this platform");
#endif
            return null;
        }

        #endregion
        //--------------------------------------- WRITING / LOADING --------------------------------------
        //--------------------------------------- WRITING / LOADING --------------------------------------

    }
}