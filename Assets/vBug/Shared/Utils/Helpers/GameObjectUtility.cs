using System;
using UnityEngine;

namespace Frankfort.VBug.Internal
{
    public static class GameObjectUtility
    {

        public static T GetComponentInParent<T>(GameObject go) where T : MonoBehaviour {
            if (go == null)
                return null;

            Transform child = go.transform;
            while (true) {
                T result = child.GetComponent<T>();
                if (result != null)
                    return result;

                child = child.parent;
                if (child == null)
                    return null;
            }
        }


        public static void SetHideFlagsRecursively(UnityEngine.Object obj) {
            SetHideFlagsRecursively(obj, HideFlags.NotEditable | HideFlags.HideInInspector | HideFlags.HideInHierarchy | HideFlags.DontSave);
            //SetHideFlagsRecursively(obj, HideFlags.DontSave);
        }
        
        
        
        public static void SetHideFlagsRecursively(UnityEngine.Object obj, HideFlags flags)
        {
            obj.hideFlags = flags;
            if(obj is GameObject)
            {
                Transform trans = (obj as GameObject).transform;
                MonoBehaviour[] components = (obj as GameObject).GetComponents<MonoBehaviour>();

                foreach (MonoBehaviour comp in components)
                    comp.hideFlags = flags;

                foreach (Transform child in trans)
                    SetHideFlagsRecursively(child.gameObject, flags);
            }
        }


        public static void SetLayerRecursive(GameObject go, int layer)
        {
            go.layer = layer;
            Transform trans = go.transform;
            
            foreach (Transform child in trans)
                SetLayerRecursive(child.gameObject, layer);
        }

    }
}
