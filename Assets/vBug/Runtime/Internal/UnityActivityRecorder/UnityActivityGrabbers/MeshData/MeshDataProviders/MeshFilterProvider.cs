using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Frankfort.VBug.Internal
{
    public class MeshFilterProvider : IMeshDataProvider
    {
        public void Dispose()
        { }

        public void GetResultEOF(ref int streamPriority, GameObject go, MeshCaptureMethod captureMethod, bool forceKeyframe, List<SMesh> storeTo)
        {
            MeshFilter[] filters = go.GetComponentsInChildren<MeshFilter>(true);
            foreach (MeshFilter filter in filters) {
                MeshRenderer renderer = filter.gameObject.GetComponent<MeshRenderer>();
                if (renderer == null)
                    continue;

                Mesh mesh = null;
                SMesh surrogate = null;
                switch (captureMethod) {
                    case MeshCaptureMethod.smartMesh:
                        mesh = SceneHierarchyCache.GetMesh(filter);
                        if (mesh != null) {
                            if (forceKeyframe) {
                                surrogate = MeshGrabberHelper.BakeMesh(filter.transform, mesh, renderer.enabled);
                                streamPriority ++;
                            } else {
                                surrogate = new SMesh(filter.transform, mesh.name, mesh.GetInstanceID(), filter.gameObject.activeInHierarchy && renderer.enabled);
                            }
                        } else {
                            if (vBug.settings.general.debugMode)
                                Debug.LogWarning("Mesh == Null");
                        }
                        break;

                    case MeshCaptureMethod.fullMesh:
                        mesh = filter.sharedMesh != null ? filter.sharedMesh : filter.mesh;
                        if (mesh != null) {
                            surrogate = MeshGrabberHelper.BakeMesh(filter.transform, mesh, renderer.enabled);
                            streamPriority++;
                        } else {
                            if (vBug.settings.general.debugMode)
                                Debug.LogWarning("Mesh == Null");
                        }
                        break;
                }

                if (surrogate != null) {
                    if (surrogate.subMeshes != null)
                        surrogate.subMeshes.materialIDs = MaterialDataGrabber.RegisterMaterials(renderer);

                    storeTo.Add(surrogate);
                } else {
                    if (vBug.settings.general.debugMode)
                        Debug.Log(go.name + "_" + filter.name + " has no SMesh!!! " + captureMethod + ", changed? " + MeshDataChangeDetector.DetectChange(filter.GetInstanceID(), mesh, filter.transform));
                }
            }
        }


    }
}
