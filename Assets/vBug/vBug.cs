using UnityEngine;
using System.Collections;
using Frankfort.VBug.Internal;

namespace Frankfort.VBug
{
    public static class vBug
    {
        public static bool isRunning { get; private set; }
        public static vBugGlobalSettings settings = new vBugGlobalSettings();

        public static VBugGenericNotification OnRecordingStarted;
        public static VBugGenericNotification OnRecordingStopped;
        public static VBugGenericNotification OnRecordingEnded; //TODO: build it.




        public static void StartRecording()
        {
            if (isRunning)
                return;

            isRunning = true;
            UnityActivityRecorder.StartRecording();

            if (OnRecordingStarted != null)
                OnRecordingStarted();
        }


        public static void StopRecording()
        {
            if (!isRunning)
                return;

            isRunning = false;
            UnityActivityRecorder.StopRecording();
            
            if (OnRecordingStopped != null)
                OnRecordingStopped();
        }


        public static void SaveSettingsAsXML(){
            SerializeRuntimeHelper.SerializeAndSaveXML<vBugGlobalSettings>(settings, Application.persistentDataPath + "/vBugGlobalSettings.xml");//, vBugGlobalSettings.getExtraTypes());
        }


        public static bool LoadSettingsFromXML()
        {
#if UNITY_WEBPLAYER || UNITY_FLASH
            Debug.Log("IO Operations are not supported on this platform!");
#else
            vBugGlobalSettings result = null;
            TextAsset asset = Resources.Load<TextAsset>("vBug/vBugGlobalSettings.xml");
            if (asset != null && !string.IsNullOrEmpty(asset.text))
            {
                result = SerializeRuntimeHelper.DeserializeXML<vBugGlobalSettings>(asset.text);
            }
            else
            {
                result = SerializeRuntimeHelper.LoadAndDeserializeXML<vBugGlobalSettings>(Application.persistentDataPath + "/vBugGlobalSettings.xml");//, vBugGlobalSettings.getExtraTypes());
            }

            if (result != null)
            {
                settings = result;
                return true;
            }
#endif
            return false;
        }


        public static int StartNewSession()
        {
            return vBugEnvironment.StartNewSession();
        }



        public static void Log(string logString)
        {
            vBugLogger.Log(logString);
        }
        public static void LogWarning(string logString)
        {
            vBugLogger.LogWarning(logString);
        }
        public static void LogError(string logString)
        {
            vBugLogger.LogError(logString);
        }
        public static void LogException(System.Exception exception)
        {
            vBugLogger.LogException(exception);
        }
    }
}