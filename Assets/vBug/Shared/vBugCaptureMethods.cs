using System;
using UnityEngine;

namespace Frankfort.VBug
{
    public enum MeshCaptureMethod
    {
        /// <summary>
        /// Best practice when using normal (non-runtime generated) SkinnedMeshRenderers and MeshFilters
        /// - If we encounter a SkinnedMeshRender, all bone information is stored and a Key-frame mesh now and then to make sure we can reproduce the actual result in editor-time
        /// - If we encounter a MeshFilter, a baked mesh will be captured if it changed compared to last time it got recorded.
        /// </summary>
        smartMesh = 0,

        /// <summary>
        /// Heavy on performance and bandwith:
        /// Best practiv
        /// - If we encounter a SkinnedMeshRenderer, the entire mesh is baked to their bone-influences.
        /// - If we encounter a MeshFilter, the entire baked mesh is captured as is.
        /// </summary>
        fullMesh = 1
    }

}