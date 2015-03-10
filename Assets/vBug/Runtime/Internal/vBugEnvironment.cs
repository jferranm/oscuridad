using System;
using System.Collections.Specialized;
using UnityEngine;
using Frankfort.VBug;
using System.Collections.Generic;
using System.Runtime.Serialization;


namespace Frankfort.VBug.Internal
{
    public static class vBugEnvironment
    {
        public static DateTime identityTimeStamp = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public static string rootSaveFolder {get; private set;}

        private static string userUID = null;
        private static string niceUserID = null;
        private static string datePath = null;
        private static Dictionary<Type, bool> vBugClassTable = new Dictionary<Type, bool>();
        

        public static void InitEnvironment()
        {
            rootSaveFolder = GetSaveRootFolder();
            
            try {
                userUID = GetCustomUID();//SystemInfo.deviceUniqueIdentifier;
            } finally {
                niceUserID = GenNiceUserID();   
            }
        }


        private static string GetCustomUID() {
            if(PlayerPrefs.HasKey("customUID"))
                return PlayerPrefs.GetString("customUID");

            string result = UnityEngine.Random.Range(0, int.MaxValue).ToString();
            PlayerPrefs.SetString("customUID", result);
            return result;
        }

        private static string GenNiceUserID()
        {
            int uidIdx = Math.Max(0, userUID.Length - 10);
            string result = 
                Application.platform + "_" +
                SystemInfo.deviceName +
                "[" + userUID.Substring(uidIdx, userUID.Length - uidIdx) + "]";

            return IOSharedHelper.CleanString(result, true); ;
        }
	





        //--------------------------------------- TODAYS SESSION NUMBERS --------------------------------------
        //--------------------------------------- TODAYS SESSION NUMBERS --------------------------------------
        #region TODAYS SESSION NUMBERS
        public static int StartNewSession(){
            InitEnvironment();

            int currentSession = GetCurrentSessionNumber();
            currentSession++;
            PlayerPrefs.SetInt("vBug_TodaysSessionNumber", currentSession);
            return currentSession;
        }

        public static int GetCurrentSessionNumber()
        {
            string currentSessionDate = DateTime.Now.ToString("ddd_MMM_yyyy");
            string lastSessionDate = PlayerPrefs.GetString("vBug_LastRecordSessionDate");
            if (currentSessionDate != lastSessionDate) {
                PlayerPrefs.SetString("vBug_LastRecordSessionDate", currentSessionDate);
                PlayerPrefs.SetInt("vBug_TodaysSessionNumber", 0);
            }
            return PlayerPrefs.GetInt("vBug_TodaysSessionNumber");
        }


        #endregion
        //--------------------------------------- TODAYS SESSION NUMBERS --------------------------------------
        //--------------------------------------- TODAYS SESSION NUMBERS --------------------------------------







        //--------------------------------------- SAVE ROOT FOLDER --------------------------------------
        //--------------------------------------- SAVE ROOT FOLDER --------------------------------------
        #region SAVE ROOT FOLDER

        public static string GetSaveRootFolder() {
            string result = Application.persistentDataPath + "/vBug";
            
#if UNITY_EDITOR
            if (UnityEditor.EditorPrefs.HasKey("vBugRootFolder"))
                result = UnityEditor.EditorPrefs.GetString("vBugRootFolder");
#elif UNITY_STANDALONE
            if(vBug.settings.general.dataProcessing.standaloneStoreToApplicationFolder)
                result = Application.dataPath + "/vBug";
#endif
            return IOSharedHelper.EscapePath(result);
        }

        public static void SetSaveRootFolder(string path) {
            rootSaveFolder = IOSharedHelper.EscapePath(path);

#if UNITY_EDITOR
            UnityEditor.EditorPrefs.SetString("vBugRootFolder", rootSaveFolder);
#endif
        }



        #endregion
        //--------------------------------------- SAVE ROOT FOLDER --------------------------------------
        //--------------------------------------- SAVE ROOT FOLDER --------------------------------------
			




        
        //--------------------------------------- SESSION FOLDER LOCATIONS --------------------------------------
        //--------------------------------------- SESSION FOLDER LOCATIONS --------------------------------------
        
        #region SESSION FOLDER LOCATIONS


        public static string GetFullSessionPath(){
            return rootSaveFolder + "/" + userUID + "/" + GetFolder_Date() + "/" + GetFolder_CurrentSession() + "/" + GetFolder_HoursMinutes();
        }

        public static string GetFullSessionPath(VerticalSliceHeaderData header){
            return rootSaveFolder + "/" + header.userNiceID + "/" + GetFolder_Date(header.dateTimeStamp) + "/" + GetFolder_CurrentSession(header.sessionNumber) + "/" + GetFolder_HoursMinutes(header.dateTimeStamp);
        }

        public static string GetFolder_CurrentSession()
        {
            return "Session_" + GetPaddedNumber(GetCurrentSessionNumber(), 3);
        }
        public static string GetFolder_CurrentSession(int sessionNumber)
        {
            return "Session_" + GetPaddedNumber(sessionNumber, 3);
        }


        public static string GetUserUID()
        {
            return userUID;
        }

        public static string GetNiceUserID()
        {
            return niceUserID;
        }

        public static string GetFolder_CurrentLevel()
        {
            return Application.loadedLevelName;
        }

        #endregion

        //--------------------------------------- SESSION FOLDER LOCATIONS --------------------------------------
        //--------------------------------------- SESSION FOLDER LOCATIONS --------------------------------------








        //--------------------------------------- DATE TIME --------------------------------------
        //--------------------------------------- DATE TIME --------------------------------------
        #region DATE TIME

        public static string GetFolder_Date()
        {
            if (datePath == null)
                datePath = DateTime.UtcNow.ToString("ddd_d_MMM_yyyy");

            return datePath;
        }
        public static string GetFolder_Date(double fromTimeStamp)
        {
            return ToUnixDateTime(fromTimeStamp).ToString("ddd_d_MMM_yyyy");
        }


        public static string GetFolder_HoursMinutes()
        {
            return DateTime.UtcNow.ToString("h_mm_tt");
        }
        public static string GetFolder_HoursMinutes(double fromTimeStamp)
        {
            return ToUnixDateTime(fromTimeStamp).ToString("h_mm_tt");
        }


        public static double GetUnixTimestamp()
        {
            return DateTime.UtcNow.Subtract(identityTimeStamp).TotalSeconds;
        }
        public static DateTime ToUnixDateTime(double seconds)
        {
            return identityTimeStamp.Add(TimeSpan.FromSeconds(seconds));
        }

        #endregion
        //--------------------------------------- DATE TIME --------------------------------------
        //--------------------------------------- DATE TIME --------------------------------------








        public static string GetPaddedNumber(int frameNumber, int framePadding)
        {
            string result = frameNumber.ToString();
            if (result.Length < framePadding)
            {
                for (int i = result.Length; i < framePadding; i++)
                    result = "0" + result;
            }
            return result;
        }




        public static bool CheckIsVBugClass(Type type)
        {
            if (vBugClassTable.ContainsKey(type)) {
                return vBugClassTable[type];
            } else {
                if (type.FullName.ToLower().Contains("vbug") || type.IsAssignableFrom(typeof(vBugHiddenUniqueComponent<>)) || type.IsAssignableFrom(typeof(BaseActivityGrabber<>))){
                    vBugClassTable.Add(type, true);
                    return true;
                } else {
                    vBugClassTable.Add(type, false);
                    return false;
                }
            }
        }



        public static Camera GetActiveOrMainCamera()
        {
            int i = Camera.allCameras.Length;
            while(--i > -1) {
                Camera cam = Camera.allCameras[i];
                if (cam.enabled && cam.gameObject.activeInHierarchy && cam.hideFlags == HideFlags.None && !cam.name.Contains("vBug"))
                    return Camera.allCameras[i];
            }
            return Camera.main;
        }


        public static string GetNiceFrameNumber(int frameNumber)
        {
            if(frameNumber < 1000)
                return frameNumber.ToString();

            string result = "";
            string normal = frameNumber.ToString();
            int iMax = normal.Length;

            for (int i = 0; i < iMax; i++ ) {
                result += normal[i];
                if (i % 3 == 0 && i != iMax - 1)
                    result += ".";
            }
            return result;
        }

    }
}