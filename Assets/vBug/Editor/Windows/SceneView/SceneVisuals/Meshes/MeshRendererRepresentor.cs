using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Frankfort.VBug.Internal;

namespace Frankfort.VBug.Editor
{
    [AddComponentMenu(""), ExecuteInEditMode()]
    public class MeshRendererSurrogate : MonoBehaviour//Rename because of 'Its not a monobehaviour' error @Unity-Free
    {
        private int randomKey;
        private Mesh mesh;
        private int goInstanceID;
        private Material[] materials;
        private MeshFilter meshFilter;
        private MeshRenderer meshRenderer;

        private SMesh keyframeMesh;
        public int currentFrameNumber = -1;

        public bool isEnabled { get; set; }
        public bool isVisible { get; private set; }

        public Dictionary<int, Bounds> boundsPerFrame = new Dictionary<int, Bounds>();

        public static MeshRendererSurrogate Create(int woInstanceID, int goInstanceID, string name){
            GameObject go = new GameObject("vBugMeshRendererRepresentor_" + name); 
            MeshRendererSurrogate representor = go.AddComponent<MeshRendererSurrogate>();
            representor.goInstanceID = goInstanceID;
            return representor;
        }


        private void Awake() {
            meshFilter = gameObject.AddComponent<MeshFilter>();
            meshRenderer = gameObject.AddComponent<MeshRenderer>();
            mesh = new Mesh();

            meshFilter.mesh = mesh = new Mesh();
            Frankfort.VBug.Internal.GameObjectUtility.SetHideFlagsRecursively(this.gameObject);
        }



        private void OnDestroy(){
            if (this.mesh != null)
                UnityEngine.Object.DestroyImmediate(this.mesh);

            DisposeMaterials();
            materials = null;
            mesh = null;
            meshFilter = null;
            keyframeMesh = null;
        }

        private void DisposeMaterials() {
            if (materials == null)
                return;

            foreach (Material mat in materials)
                UnityEngine.Object.DestroyImmediate(mat);
        }

        public void SetCurrentFrame(int frameNumber){
            if (frameNumber == currentFrameNumber)
                return;

            SMesh currentMesh = GetClosestAvailableMesh(frameNumber, false, vBugEditorSettings.PlaybackMeshSearchRange);
            if (SceneVisualsHelper.CheckMeshKeyFrame(currentMesh)) //if we can use this as a keyframe-mesh
                keyframeMesh = currentMesh;

            if(keyframeMesh == null)//Check if not previously set
                keyframeMesh = GetClosestAvailableMesh(frameNumber, true, vBugEditorSettings.PlaybackMeshSearchRange); //if unusable and still null

            if (currentMesh == null || keyframeMesh == null)
                return;

            ApplyMeshData(frameNumber, currentMesh);
            currentFrameNumber = frameNumber;
        }








        //--------------------------------------- Get mesh data --------------------------------------
        //--------------------------------------- Get mesh data --------------------------------------
        #region Get mesh data

        public SMesh GetClosestAvailableMesh(int frameNumber, bool isKeyFrame, int maxRange)
        {
            //--------------- Search current and down --------------------
            bool toggle = false;
            for (int i = 0; i < vBugEditorSettings.PlaybackMeshSearchRange; i++) {
                int offset = (int)Mathf.Ceil(i / 2);
                if (toggle)
                    offset = -offset;
                toggle = !toggle;
                int f = frameNumber + offset;

                VerticalActivitySlice slice = VerticalSliceDatabase.GetSlice(f);
                if (slice == null)
                    continue;

                SMesh mesh = SceneVisualsHelper.GetMeshFromSlice(slice, this.goInstanceID);
                if (mesh == null)
                    continue;

                if (isKeyFrame && !SceneVisualsHelper.CheckMeshKeyFrame(mesh))
                    continue;
                return mesh;
            } 
            //--------------- Search current and down --------------------
			
            return null;
        }
        



        

        private Vector3[] GetMeshVertices(SMesh mesh)
        {
            int vMax = mesh.vertices.count;
            Vector3[] result = new Vector3[vMax];
            int v = vMax;
            while (--v > -1)
                result[v] = keyframeMesh.vertices.GetVertex(v);

            return result;
        }


        public Vector3[] GetVertices() {
            if (mesh == null)
                return null;

            return mesh.vertices;
        }
        #endregion
        //--------------------------------------- Get mesh data --------------------------------------
        //--------------------------------------- Get mesh data --------------------------------------
			









        //--------------------------------------- APPLY BONES TO VERTICES --------------------------------------
        //--------------------------------------- APPLY BONES TO VERTICES --------------------------------------
        #region APPLY BONES TO VERTICES


        private void ApplyMeshData(int frameNumber, SMesh currentMesh) {
            if (!currentMesh.header.activeInHierarchy) {
                isEnabled = false;
            } else {
                isEnabled = true;
                int vMax = keyframeMesh.vertices.count;
                
                //--------------- Vertices --------------------
                Vector3[] vertices = new Vector3[vMax];
                if (currentMesh.header.type == SMesh.Type.full || currentMesh.bones == null || currentMesh.bones.count == 0) {
                    vertices = ApplyMeshMatrix(keyframeMesh, currentMesh);
                } else {
                    vertices = ApplyBoneWeights(keyframeMesh, currentMesh);
                }
                //--------------- Vertices --------------------


                if (vertices != null && vertices.Length > 0) {
                    //--------------- Set mesh-data --------------------
                    if (this.mesh != null) {
                        this.mesh.Clear();
                        this.mesh.vertices = vertices;
                        this.mesh.triangles = keyframeMesh.triangles;

                        if (keyframeMesh.uv != null && keyframeMesh.uv.count > 0)
                            this.mesh.uv = keyframeMesh.uv.GetVertexArray();
                        if (keyframeMesh.uv1 != null && keyframeMesh.uv1.count > 0)
                            this.mesh.uv1 = keyframeMesh.uv1.GetVertexArray();
                        if (keyframeMesh.uv2 != null && keyframeMesh.uv2.count > 0)
                            this.mesh.uv2 = keyframeMesh.uv2.GetVertexArray();

                        this.mesh.RecalculateNormals();
                        this.mesh.tangents = CalculateCheapTangents(keyframeMesh.triangles, vertices, mesh.normals);
                        //this.mesh.Optimize();
                    }
                    //--------------- Set mesh-data --------------------


                    //--------------- Set submesh-data --------------------
                    if (keyframeMesh.subMeshes != null) {
                        if (keyframeMesh.subMeshes.triangles != null) {
                            int i = keyframeMesh.subMeshes.triangles.Length;
                            this.mesh.subMeshCount = i;

                            while (--i > -1)
                                this.mesh.SetTriangles(keyframeMesh.subMeshes.triangles[i], i); 
                        }

                        if (keyframeMesh.subMeshes.materialIDs != null) {
                            DisposeMaterials();
                            Material[] materials = EditorMaterialHelper.GetClosestAvailableMaterialsByID(keyframeMesh.subMeshes.materialIDs, frameNumber);
                            if (materials != null && materials.Length > 0)
                                this.meshRenderer.sharedMaterials = materials;

                            EditorMaterialHelper.SetFallbackMaterial(this.meshRenderer);
                        }
                    } 
                    //--------------- Set submesh-data --------------------


                    //--------------- Set currentframe boundingbox for clipping --------------------
                    if (!boundsPerFrame.ContainsKey(frameNumber))
                        AddMeshBounds(frameNumber, vertices);
                    //--------------- Set currentframe boundingbox for clipping --------------------
			
                }
            }
            this.meshRenderer.enabled = isVisible && isEnabled;
        }

        private Vector4[] CalculateCheapTangents(int[] triangles, Vector3[] vertices, Vector3[] normals) {
            int tIdx = triangles.Length;
            bool[] mask = new bool[tIdx];
            Vector4[] tangents = new Vector4[vertices.Length];

            while(--tIdx > -1){
                int t0 = triangles[tIdx]; 
                tIdx --;
                int t1 = triangles[tIdx]; 
                tIdx --;
                int t2 = triangles[tIdx];

                if (mask[t0] || mask[t1] || mask[t2])
                    continue;

                tangents[t0] = Vector3.Cross(Vector3.up, normals[t0]).normalized;
                tangents[t1] = Vector3.Cross(Vector3.up, normals[t1]).normalized;
                tangents[t2] = Vector3.Cross(Vector3.up, normals[t2]).normalized;

                mask[t0] = true;
                mask[t1] = true;
                mask[t2] = true;
            }

            return tangents;
        }





        private Vector3[] ApplyMeshMatrix(SMesh keyframeMesh, SMesh currentMesh)
        {
            int vMax = keyframeMesh.vertices.count;
            Vector3[] result = new Vector3[vMax];
            Matrix4x4 matrix = currentMesh.header.matrix;
            int v = vMax;
            while (--v > -1)
                result[v] = matrix.MultiplyPoint(keyframeMesh.vertices.GetVertex(v));

            return result;
        }


        public Vector3[] ApplyBoneWeights(SMesh keyframeMesh, SMesh currentMesh)
        {
            if (keyframeMesh == null || currentMesh == null) {
                if (vBugEditorSettings.DebugMode)
                    Debug.LogError("Error: keyframeMesh || currentMesh null: " + currentMesh + ", " + keyframeMesh);
                return null;
            }

            //--------------- Get Bones Matrix --------------------
            SBoneWeightArray boneWeights = (keyframeMesh.boneWeights != null && keyframeMesh.boneWeights.count > 0) ? keyframeMesh.boneWeights : currentMesh.boneWeights;
            if (boneWeights == null) {
                if (vBugEditorSettings.DebugMode)
                    Debug.LogError("Error @ " + currentMesh.header.parentName + ": boneWeight null");
                return null;
            }

            int bMax = boneWeights.count;
            BoneWeight[] weights = new BoneWeight[bMax];
            int b = bMax;
            while (--b > -1)
                weights[b] = boneWeights.GetBoneWeight(b);
            //--------------- Get Bones Matrix --------------------


            //--------------- Init --------------------
            if (currentMesh.bones == null || currentMesh.bones.count == 0 || weights.Length == 0) {
                if (vBugEditorSettings.DebugMode)
                    Debug.LogError("Error @ " + currentMesh.header.parentName + ": currentMesh.bones null || currentMesh.bones.count == 0 || weights.Length == 0"); 
                return null;
            }

            int vMax = keyframeMesh.vertices.count;
            Vector3[] result = new Vector3[vMax];
            Vector3[] vertices = GetMeshVertices(keyframeMesh);

            if (weights.Length != vertices.Length || keyframeMesh.bindPoses.count != currentMesh.bones.count) {// should not happen, but anyways
                if (vBugEditorSettings.DebugMode)
                    Debug.LogError("Error: " + weights.Length + ", " + currentMesh.bindPoses.count + ", " + currentMesh.bones.count);
                return ApplyMeshMatrix(keyframeMesh, currentMesh);
            }

            Matrix4x4[] boneMatrices = new Matrix4x4[currentMesh.bones.count];
            int i = currentMesh.bones.count;
            while (--i > -1)
                boneMatrices[i] = currentMesh.bones.matrices.GetMatrix(i) * keyframeMesh.bindPoses.GetMatrix(i);

            i = 0;
            int iMax = vertices.Length;
            //--------------- Init --------------------


            //--------------- Apply influences --------------------
            while (i < iMax)
            {
                BoneWeight boneWeight = weights[i];
                Vector3 oV = vertices[i];
                Vector3 v = oV;
                float wTotal = 0f;
                float weight0 = boneWeight.weight0;
                float weight1 = boneWeight.weight1;
                float weight2 = boneWeight.weight2;
                float weight3 = boneWeight.weight3;

                if (weight0 > 0f)
                {
                    v = boneMatrices[boneWeight.boneIndex0].MultiplyPoint3x4(oV) * weight0;
                    wTotal = weight0;
                }
                if (weight1 > 0f)
                {
                    v += boneMatrices[boneWeight.boneIndex1].MultiplyPoint3x4(oV) * weight1;
                    wTotal += weight1;
                }
                if (weight2 > 0f)
                {
                    v += boneMatrices[boneWeight.boneIndex2].MultiplyPoint3x4(oV) * weight2;
                    wTotal += weight2;
                }
                if (weight3 > 0f)
                {
                    v += boneMatrices[boneWeight.boneIndex3].MultiplyPoint3x4(oV) * weight3;
                    wTotal += weight3;
                }
                if (wTotal < 1f)
                    v /= wTotal;

                result[i++] = v;
            }
            //--------------- Apply influences --------------------


            return result;
        }

        #endregion
        //--------------------------------------- APPLY BONES TO VERTICES --------------------------------------
        //--------------------------------------- APPLY BONES TO VERTICES --------------------------------------
	









        //--------------------------------------- DRAW BONES --------------------------------------
        //--------------------------------------- DRAW BONES --------------------------------------
        #region DRAW BONES

        public void DrawBones(int frameNumber, Color color) {
            SMesh mesh = SceneVisualsHelper.GetMeshByFrameNumber(frameNumber, this.goInstanceID);
            if (mesh != null && mesh.bones != null) {
                
                int i = mesh.bones.count;
                while (--i > -1){
                    int parentIdx = SceneVisualsHelper.GetBoneByID(mesh.bones, mesh.bones.parentIDs[i]);
                    if (parentIdx != -1){
                        SMatrix4x4 bMatrix = mesh.bones.matrices.GetMatrix(i);
                        SMatrix4x4 pMatrix = mesh.bones.matrices.GetMatrix(parentIdx);
                        EditorHelper.SceneviewDrawLine(bMatrix.position, pMatrix.position, color, 0.666f);
                    }
                }
            }
        }

        #endregion
        //--------------------------------------- DRAW BONES --------------------------------------
        //--------------------------------------- DRAW BONES --------------------------------------












        //--------------------------------------- CLIPPING --------------------------------------
        //--------------------------------------- CLIPPING --------------------------------------
        #region CLIPPING


        private void AddMeshBounds(int frameNumber, Vector3[] vertices) {
            if (vertices == null || vertices.Length == 0)
                return;

            Vector3 min = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
            Vector3 max = new Vector3(float.MinValue, float.MinValue, float.MinValue);

            int i = vertices.Length;
            while (--i > -1) {
                Vector3 v = vertices[i];
                min.x = Mathf.Min(min.x, v.x);
                min.y = Mathf.Min(min.y, v.y);
                min.z = Mathf.Min(min.z, v.z);
                max.x = Mathf.Max(max.x, v.x);
                max.y = Mathf.Max(max.y, v.y);
                max.z = Mathf.Max(max.z, v.z);
            }
            boundsPerFrame.Add(frameNumber, new Bounds((min + max) / 2f, max - min));
        }


        internal void UpdateClipping(int frameNumber, Plane[] planes) {
            isVisible = CheckFrustrum(frameNumber, planes);

            if (this.meshRenderer != null)
                this.meshRenderer.enabled = isVisible && isEnabled;
        }

        private bool CheckFrustrum(int frameNumber, Plane[] planes) {
            if (!boundsPerFrame.ContainsKey(frameNumber))
                return true;
            
            return GeometryUtility.TestPlanesAABB(planes, boundsPerFrame[frameNumber]);
        }

        public bool GetBounds(int frameNumber, out Bounds bounds) {
            if (!boundsPerFrame.ContainsKey(frameNumber)) {
                bounds = default(Bounds);
                return false;
            }
            bounds = boundsPerFrame[frameNumber];
            return true;
        }

        public void DrawBoundingBox(int frameNumber, Color color, Plane[] clippingPlanes) {
            Bounds bounds;
            if (GetBounds(frameNumber, out bounds)) {
                if (clippingPlanes == null || GeometryUtility.TestPlanesAABB(clippingPlanes, bounds))
                    EditorHelper.DrawBounds(bounds, color, 0.666f);
            }
        }
        #endregion
        //--------------------------------------- CLIPPING --------------------------------------
        //--------------------------------------- CLIPPING --------------------------------------


    }
}