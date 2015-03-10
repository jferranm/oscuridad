using System;
using System.Collections.Generic;
using UnityEngine;


namespace Frankfort.VBug.Internal
{
    public static class vBugLogger
    {
        private static List<string> excludeStack = new List<string>();

        public static bool Contains(string logString)
        {
            bool result = false;
            lock (excludeStack)     
            {
                result = excludeStack.Contains(logString);
            }
            return result;
        }

        public static void Clear()
        {
            lock (excludeStack)
            {
                excludeStack.Clear();
            }
        }

        public static void Log(string logString)
        {
            if (!vBug.settings.general.debugMode)
                return;

            lock (excludeStack)
            {
                excludeStack.Add(logString);
            }
            Debug.Log(logString);
        }
        public static void LogWarning(string logString)
        {
            if (!vBug.settings.general.debugMode)
                return;

            lock (excludeStack)
            {
                excludeStack.Add(logString);
            }
            Debug.LogWarning(logString);
        }
        public static void LogError(string logString)
        {
            if (!vBug.settings.general.debugMode)
                return;

            lock (excludeStack)
            {
                excludeStack.Add(logString);
            }
            Debug.LogError(logString);
        }
        public static void LogException(Exception exception)
        {
            if (!vBug.settings.general.debugMode)
                return;

            lock (excludeStack)
            {
                string completeMessage = exception.GetType().Name + ": " + exception.Message;
                excludeStack.Add(completeMessage);
            }
            Debug.LogException(exception);
        }
    }
}
