using System;
using UnityEngine;
using System.Collections.Generic;
using Frankfort.VBug;


namespace Frankfort.VBug.Internal {
    public static class IOSharedHelper {


        public static string EscapePath(string path) {
            char seperator = GetDirectorySeperator();
            path = path.Replace('\\', seperator);
            path = path.Replace('/', seperator);
            return path;
        }


        public static string[] EscapePaths(string[] paths) {
            char seperator = GetDirectorySeperator();
            int i = paths.Length;
            string[] result = new string[paths.Length];

            while (--i > -1) {
                string tmp = paths[i].Replace('\\', seperator);
                result[i] = tmp.Replace('/', seperator);
            }

            return result;
        }



        public static char GetDirectorySeperator() {
            return '/';
            /*
#if !UNITY_WEBPLAYER && !UNITY_FLASH
            return System.IO.Path.DirectorySeparatorChar;
#else
            return '/';
#endif
             */
        }


        public static string CleanString(string text, bool removeDashes) {
            text = text.Replace(":", string.Empty);
            text = text.Replace("*", string.Empty);
            text = text.Replace("?", string.Empty);
            text = text.Replace("<", string.Empty);
            text = text.Replace(">", string.Empty);
            text = text.Replace("\"", string.Empty);
            text = text.Replace("|", string.Empty);

            if (removeDashes) {
                text = text.Replace("/", string.Empty);
                text = text.Replace("\\", string.Empty);
            }
            return text;
        }
    }
}