using System;
using System.Collections.Generic;
using UnityEngine;

namespace Frankfort.VBug.Internal
{
    public static class SceneHierarchyCache
    {
        private static int currentSceneIdx = -1;
        private static int sceneGoFrame;
        private static int allGoFrame;
        //private static int allObjectFrame;

        private static GameObject[] _sceneGameObjects;
        //private static UnityEngine.Object[] _allObjects;

        private static Dictionary<int, Mesh> meshFilterRelations = new Dictionary<int, Mesh>();
        private static List<int> combinedMeshIDlist = new List<int>();
        private static Dictionary<int, BoneWeight[]> meshBoneWeights = new Dictionary<int, BoneWeight[]>();
        





        //--------------- Memory management --------------------
        private static void ValidateSceneChange()
        {
            int idx = Application.loadedLevel;
            if (idx != currentSceneIdx)
            {
                PruneCache();
                currentSceneIdx = idx;
            }
        }

        public static void PruneCache()
        {
            _sceneGameObjects = null;
            
            if(meshFilterRelations != null)
                meshFilterRelations.Clear();

            if(combinedMeshIDlist != null)
                combinedMeshIDlist.Clear();

            if (meshBoneWeights != null)
                meshBoneWeights.Clear();
        }
        //--------------- Memory management --------------------










        //--------------------------------------- UNITY SCENE OBJECTS --------------------------------------
        //--------------------------------------- UNITY SCENE OBJECTS --------------------------------------
        #region UNITY SCENE OBJECTS


        public static GameObject[] RootGameObjects
        {
            get
            {
                ValidateSceneChange();
                if (sceneGoFrame != Time.frameCount || _sceneGameObjects == null)
                {
                    sceneGoFrame = Time.frameCount;

                    Transform[] all = GameObject.FindObjectsOfType<Transform>();//9.15 ms

                    List<GameObject> result = new List<GameObject>();
                    int i = all.Length;
                    while(--i > -1){
                        Transform trans = all[i];
                        if (trans.parent != null || trans.hideFlags != HideFlags.None)
                            continue;

                        result.Add(trans.gameObject);
                    }
                    _sceneGameObjects = result.ToArray();
                }

                return _sceneGameObjects;
            }
        }


        



        public static Mesh GetMesh(Component parent)
        {
            ValidateSceneChange();
            if (parent == null || !(parent is MeshFilter || parent is SkinnedMeshRenderer))
                return null;

            int id = parent.GetInstanceID();
            Mesh result = null;

            if (meshFilterRelations.ContainsKey(id)) {
                result = meshFilterRelations[id];
                if (result == null)
                    meshFilterRelations.Remove(id);
            } 

            if (result == null) { //if not present in the dictionary, or destroyed in the meantime...
                if (combinedMeshIDlist.Contains(id)) //Skip when flagged as 'static or combined' before
                    return null;

                //--------------- Check static batch --------------------
                bool isStaticBatched = false;
                MeshRenderer renderer = parent.gameObject.GetComponent<MeshRenderer>();
                if (renderer != null)
                    isStaticBatched = renderer.isPartOfStaticBatch;
                //--------------- Check static batch --------------------

                if (isStaticBatched || parent.gameObject.isStatic) { //if the mesh appears to be static batched, add to exception list
                    combinedMeshIDlist.Add(id);
                    return null;
                } else {
                    if (parent is SkinnedMeshRenderer) {
                        result = (parent as SkinnedMeshRenderer).sharedMesh;
                    } else if (parent is MeshFilter) {
                        MeshFilter filter = (MeshFilter)parent;
                        result = filter.sharedMesh; //only do the .mesh lookup ONCE, because it takes 26ms each time on an average sized gameobject!
                        if (result == null)
                            result = filter.mesh; //Is this safe?
                    }

                    if(result != null) {
                        if (result.name.Contains("Combined Mesh")) {
                            combinedMeshIDlist.Add(id);
                            return null;
                        } else {
                            meshFilterRelations.Add(id, result);
                        }
                    }
                }
            }

            return result;
        }



        public static BoneWeight[] GetBoneWeights(Mesh mesh)
        {
            if (mesh == null)
                return null;

            ValidateSceneChange();
            int ID = mesh.GetInstanceID();
            if (meshBoneWeights.ContainsKey(ID))
            {
                return meshBoneWeights[ID];
            }
            else
            {
                BoneWeight[] result = mesh.boneWeights;
                meshBoneWeights.Add(ID, result);
                return result;
            }
        }




        #endregion
        //--------------------------------------- UNITY SCENE OBJECTS --------------------------------------
        //--------------------------------------- UNITY SCENE OBJECTS --------------------------------------
    }
}
