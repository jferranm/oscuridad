using System;
using UnityEngine;
using System.Collections.Generic;


namespace Frankfort.VBug.Internal {
    [AddComponentMenu("")]
    public class vBugHiddenUniqueComponent <T>: MonoBehaviour where T: MonoBehaviour
    {

        //--------------------------------------- STATIC --------------------------------------
        //--------------------------------------- STATIC --------------------------------------
        #region STATIC
        private static Dictionary<Type, MonoBehaviour> vBugHelpers = new Dictionary<Type, MonoBehaviour>();

        protected static T helper {
            get {
                Type thisType = typeof(T);
                if (!vBugHelpers.ContainsKey(thisType))
                    SetHelper();

                return (T)vBugHelpers[thisType];
            }
        }

        public static void SetHelper(GameObject parent = null) {
            try {

                Type thisType = typeof(T);
                if (!vBugHelpers.ContainsKey(thisType)) {
                    T helper = (T)UnityEngine.Object.FindObjectOfType(typeof(T));
                    if (helper == null) {
                        if (parent == null) {
                            helper = GetVBugHelperGameObject().AddComponent<T>();
                        } else {
                            helper = parent.AddComponent<T>();
                        }
                    }
                    GameObjectUtility.SetHideFlagsRecursively(helper);
                    vBugHelpers.Add(thisType, helper);
                }
            } catch (Exception e) {
                if (vBug.settings.general.debugMode)
                    Debug.LogError(e.Message + e.StackTrace);
            }
        }


        private static GameObject GetVBugHelperGameObject() {
            GameObject result = GameObject.Find("vBugHelper");
            if (result == null) {
                result = new GameObject("vBugHelper");
                GameObjectUtility.SetHideFlagsRecursively(result);
                GameObject.DontDestroyOnLoad(result);
            }
            return result;
        }

        #endregion
        //--------------------------------------- STATIC --------------------------------------
        //--------------------------------------- STATIC --------------------------------------
			





         






        //--------------------------------------- BASE COMPONENT --------------------------------------
        //--------------------------------------- BASE COMPONENT --------------------------------------
        #region BASE COMPONENT

        protected virtual void OnApplicationQuit() {
            UnityEngine.GameObject.Destroy(this.gameObject);
        }

        #endregion
        //--------------------------------------- BASE COMPONENT --------------------------------------
        //--------------------------------------- BASE COMPONENT --------------------------------------
			
    }
}