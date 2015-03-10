using System;
using System.Collections.Generic;
using UnityEngine;


namespace Frankfort.VBug.Internal
{
    public class SkinnedMeshProvider : IMeshDataProvider
    {
        private Mesh snapShotMesh;
        public SkinnedMeshProvider()
        {
            snapShotMesh = new Mesh();   
        } 

        public void Dispose()
        {
            if (snapShotMesh != null)
                UnityEngine.Object.DestroyImmediate(snapShotMesh);

            snapShotMesh = null;
        }

        public void GetResultEOF(ref int streamPriority, GameObject go, MeshCaptureMethod captureMethod, bool forceKeyframe, List<SMesh> storeTo)
        {
            SkinnedMeshRenderer[] renderers = go.GetComponentsInChildren<SkinnedMeshRenderer>(true);
            foreach (SkinnedMeshRenderer renderer in renderers) {
                SMesh surrogate = null;

                switch (captureMethod) {
                    case MeshCaptureMethod.smartMesh:
                        if (forceKeyframe) { // || MeshDataChangeDetector.DetectChange(renderer.GetInstanceID(), SceneHierarchyCache.GetMesh(renderer), renderer.transform)) {//not needed, its forced every one and then anyways
                            surrogate = MeshGrabberHelper.BakeBonesKeyFrame(renderer);
                            streamPriority++;
                        } else {
                            surrogate = MeshGrabberHelper.BakeBonesOnly(renderer);
                        }
                        break;

                    case MeshCaptureMethod.fullMesh:
                        surrogate = MeshGrabberHelper.BakeSkinnedMesh(renderer, vBug.settings.recording.meshDataRecording.maxBlendWeights); //19 - 54ms (4 bones), 13 - 43 (1 bone) //14 ms steady by caching boneWeights, and 6 ms(!!!) by splitting the AddBoneInfluences and removing the static method all together!
                        break;
                }

                if (surrogate != null) {
                    if (surrogate.subMeshes != null)
                        surrogate.subMeshes.materialIDs = MaterialDataGrabber.RegisterMaterials(renderer);
                    
                    storeTo.Add(surrogate);
                }
            }
        }

    }
}
