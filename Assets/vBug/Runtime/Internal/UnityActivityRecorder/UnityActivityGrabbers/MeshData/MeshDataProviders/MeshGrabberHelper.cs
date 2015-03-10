using UnityEngine;
using System;
using System.Collections;


namespace Frankfort.VBug.Internal
{
    public static class MeshGrabberHelper
    {


        //--------------------------------------- BAKE MESH FILTER --------------------------------------
        //--------------------------------------- BAKE MESH FILTER --------------------------------------
        #region BAKE MESH FILTER

        public static SMesh BakeMesh(Transform transform, Mesh mesh, bool rendererEnabled, SVector3Array customVertices = null)
        {
            SMesh surrogate = new SMesh(transform, mesh.name, mesh.GetInstanceID(), transform.gameObject.activeInHierarchy && rendererEnabled);
            surrogate.header.type = SMesh.Type.full;
            CopyMeshData(mesh, surrogate, customVertices);
            return surrogate;
        }

        #endregion
        //--------------------------------------- BAKE MESH FILTER --------------------------------------
        //--------------------------------------- BAKE MESH FILTER --------------------------------------
			







        //--------------------------------------- BAKE SKINNED MESH RENDERER --------------------------------------
        //--------------------------------------- BAKE SKINNED MESH RENDERER --------------------------------------
        #region BAKE SKINNED MESH RENDERER

        public static SMesh BakeBonesOnly(SkinnedMeshRenderer source)
        {
            if (source == null)
                return null;

            Mesh mesh = SceneHierarchyCache.GetMesh(source);
            if (mesh == null)
                return null;

            Renderer renderer = source.gameObject.GetComponent<Renderer>();
            SMesh surrogate = new SMesh(source.transform, mesh.name, mesh.GetInstanceID(), source.gameObject.activeInHierarchy && (renderer != null && renderer.enabled));
            surrogate.header.type = SMesh.Type.bonesOnly;
            CopyBoneTransform(source, surrogate);

            return surrogate;
        }


        public static SMesh BakeBonesKeyFrame(SkinnedMeshRenderer renderer)
        {
            if (renderer == null)
                return null;

            Mesh mesh = SceneHierarchyCache.GetMesh(renderer);
            if (mesh == null)
                return null;

            SMesh surrogate = BakeMesh(renderer.transform, mesh, renderer.enabled);
            surrogate.header.type = SMesh.Type.bonesKeyFrame;

            CopyBoneTransform(renderer, surrogate);
            CopyBoneBindPoses(mesh, surrogate);
            CopyBoneWeights(mesh, surrogate);

            return surrogate;
        }



        /// <summary>
        /// SkinnedMeshRenderer.BakeMesh but approximately 8 to 10 times faster, due too caching and raw copy-pasting code
        /// I know it looks like copy-pasted code, and it is, but I started with one method handling one, two and four bones at once... the static lookup, passing parameters, etc took twice the calculation power it does now!
        /// </summary>
        /// <param name="renderer"></param>
        /// <param name="maxBlendWeights"></param>
        /// <returns></returns>
        public static SMesh BakeSkinnedMesh(SkinnedMeshRenderer renderer, int maxBlendWeights)
        {
            if (renderer == null)
                return null;

            Mesh mesh = SceneHierarchyCache.GetMesh(renderer);
            if (mesh == null)
                return null;

            Transform[] bones = renderer.bones;
            //BoneWeight[] weights = mesh.boneWeights; //takes 25ms!
            BoneWeight[] weights = SceneHierarchyCache.GetBoneWeights(mesh); //reduced to 0.04ms!

            if (bones == null || bones.Length == 0 || weights == null || weights.Length == 0)
                return BakeMesh(renderer.transform, mesh, renderer.enabled);

            Vector3[] vertices = mesh.vertices;
            SVector3Array vectorArray = new SVector3Array(mesh.vertices.Length);
            float[] result = vectorArray.vectorData;

            if (weights.Length != vertices.Length)
            {
                if (vBug.settings.general.debugMode)
                    Debug.Log("weights.Length != vertices.Length");
                return BakeMesh(renderer.transform, mesh, renderer.enabled);
            }

            Matrix4x4 worldToLocal = renderer.transform.worldToLocalMatrix;
            Matrix4x4[] bindposes = mesh.bindposes;
            Matrix4x4[] matrices = new Matrix4x4[bones.Length];
            int i = bones.Length;
            while (--i > -1)
                matrices[i] = worldToLocal * bones[i].localToWorldMatrix * bindposes[i];
             
            i = 0;
            int j = 0;
            int iMax = vertices.Length;
            int maxInfluences = Mathf.Min(maxBlendWeights, (int)QualitySettings.blendWeights);
            
            if(maxInfluences == 1)
            {
                while (i < iMax)
                {
                    Vector3 vertex = matrices[weights[i].boneIndex0].MultiplyPoint3x4(vertices[i]);
                    result[j++] = vertex.x;
                    result[j++] = vertex.y;
                    result[j++] = vertex.z;
                    i++;
                }
                    //result[i] = AddBoneInfluenceOne(ref vertices[i], ref matrices, ref weights[i]);
            }
            else if (maxInfluences == 2)
            {
                while (i < iMax)
                {
                    //result[i] = AddBoneInfluenceTwo(ref vertices[i], ref matrices, ref weights[i]);
                    BoneWeight boneWeight = weights[i];
                    Vector3 oV = vertices[i];
                    Vector3 v = oV;
                    float wTotal = 0f;
                    float weight0 = boneWeight.weight0;
                    float weight1 = boneWeight.weight1;

                    if (weight0 > 0f)
                    {
                        v = matrices[boneWeight.boneIndex0].MultiplyPoint3x4(oV) * weight0;
                        wTotal = weight0;
                    }
                    if (weight1 > 0f)
                    {
                        v += matrices[boneWeight.boneIndex1].MultiplyPoint3x4(oV) * weight1;
                        wTotal += weight1;
                    }
                    if (wTotal < 1f)
                        v /= wTotal;

                    result[j++] = v.x;
                    result[j++] = v.y;
                    result[j++] = v.z;
                    i++;
                }
            }
            else
            {
                while (i < iMax)
                {
                    //result[i] = AddBoneInfluenceFour(ref vertices[i], ref matrices, ref weights[i]); //BAM! doubles performance using this instead of using one AddBoneInfluence method checking thousands of times the maxInfluences
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
                        v = matrices[boneWeight.boneIndex0].MultiplyPoint3x4(oV) * weight0;
                        wTotal = weight0;
                    }
                    if (weight1 > 0f)
                    {
                        v += matrices[boneWeight.boneIndex1].MultiplyPoint3x4(oV) * weight1;
                        wTotal += weight1;
                    }
                    if (weight2 > 0f)
                    {
                        v += matrices[boneWeight.boneIndex2].MultiplyPoint3x4(oV) * weight2;
                        wTotal += weight2;
                    }
                    if (weight3 > 0f)
                    {
                        v += matrices[boneWeight.boneIndex3].MultiplyPoint3x4(oV) * weight3;
                        wTotal += weight3;
                    }
                    if (wTotal < 1f)
                        v /= wTotal;

                    result[j++] = v.x;
                    result[j++] = v.y;
                    result[j++] = v.z;
                    i++;
                }
            }


            SMesh surrogate = BakeMesh(renderer.transform, mesh, renderer.enabled, vectorArray);
            return surrogate;
        }


        #endregion
        //--------------------------------------- BAKE SKINNED MESH RENDERER --------------------------------------
        //--------------------------------------- BAKE SKINNED MESH RENDERER --------------------------------------
			










        //--------------------------------------- COPY DATA --------------------------------------
        //--------------------------------------- COPY DATA --------------------------------------
        #region COPY DATA

        private static void CopyMeshData(Mesh mesh, SMesh destination, SVector3Array customVertices = null)
        {
            destination.triangles = mesh.triangles;
            destination.subMeshes = new SSubMeshArray(mesh);

            if (customVertices != null) {
                destination.vertices = customVertices;
            } else {
                destination.vertices = new SVector3Array(mesh.vertices);
            }

            if (mesh.uv != null)
                destination.uv = new SVector2Array(mesh.uv);

            if (mesh.uv2 != null)
                destination.uv1 = new SVector2Array(mesh.uv2);

            if (mesh.uv2 != null)
                destination.uv2 = new SVector2Array(mesh.uv2);
        }

        private static void CopyBoneBindPoses(Mesh source, SMesh destination)
        {
            Matrix4x4[] bindPoses = source.bindposes;
            if (bindPoses == null)
                return;

            destination.bindPoses = new SMatrix4x4Array(bindPoses);
        }


        private static void CopyBoneWeights(Mesh source, SMesh destination)
        {
            BoneWeight[] weights = SceneHierarchyCache.GetBoneWeights(source);
            if (weights == null)
                return;

            destination.boneWeights = new SBoneWeightArray(weights);
        }

        private static void CopyBoneTransform(SkinnedMeshRenderer source, SMesh destination)
        {
            Transform[] bones = source.bones;
            if (bones == null)
                return;

            int rootID = -1;
            if (source.rootBone != null)
                source.rootBone.GetInstanceID();

            destination.bones = new SBonesArray(bones, rootID);
        }

        #endregion
        //--------------------------------------- COPY DATA --------------------------------------
        //--------------------------------------- COPY DATA --------------------------------------

    }
}