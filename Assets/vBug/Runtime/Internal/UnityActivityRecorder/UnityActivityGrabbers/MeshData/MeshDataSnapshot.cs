using System;
using System.Collections.Generic;
using UnityEngine;

namespace Frankfort.VBug.Internal
{
    [Serializable]
    public class MeshDataSnapshot
    {
        public List<SWorldObject> worldObjects = new List<SWorldObject>();

        public static byte[] Serialize(MeshDataSnapshot input)
        {
            if (input.worldObjects == null)
                return null;

            int iMax = input.worldObjects.Count;
            if (iMax == 0)
                return new byte[0];

            ComposedByteStream stream = ComposedByteStream.FetchStream(iMax);
            for(int i = 0; i < iMax; i++)
                stream.AddStream(SWorldObject.Serialize(input.worldObjects[i]));

            return stream.Compose();
        }

        public static MeshDataSnapshot Deserialize(byte[] input)
        {
            if (input == null || input.Length == 0)
                return null;

            MeshDataSnapshot result = new MeshDataSnapshot();
            ComposedByteStream stream = ComposedByteStream.FromByteArray(input);
            if (stream == null)
                return null;

            int iMax = stream.streamCount;
            result.worldObjects = new List<SWorldObject>(iMax);
            for (int i = 0; i < iMax; i++)
                result.worldObjects.Add(SWorldObject.Deserialize(stream.ReadNextStream<byte>()));

            stream.Dispose();
            return result;
        }

        public void Dispose()
        {
            if (worldObjects != null) {
                foreach (SWorldObject wo in worldObjects) {
                    if (wo != null)
                        wo.Dispose();
                }
            }
            worldObjects = null;
        }
    }




    [Serializable]
    public class SWorldObject {
        public int instanceID;
        public bool activeInHierarchy;
        public string name;
        public SColor color;
        public List<SMesh> meshes;

        public SWorldObject() //Default constructor
        { }

        public SWorldObject(GameObject go) {
            instanceID = go.GetInstanceID();
            name = go.name;
            activeInHierarchy = go.activeInHierarchy;
        }

        public static byte[] Serialize(SWorldObject input) {
            ComposedPrimitives prims = new ComposedPrimitives();

            prims.AddValue(input.instanceID);
            prims.AddValue(input.activeInHierarchy);
            prims.AddValue(input.color.r);
            prims.AddValue(input.color.g);
            prims.AddValue(input.color.b);
            prims.AddValue(input.color.a);

            ComposedByteStream stream = new ComposedByteStream(input.meshes.Count + 2);
            stream.AddStream(prims.Compose());
            stream.AddStream(input.name);
            
            foreach (SMesh mesh in input.meshes)
                stream.AddStream(SMesh.Serialize(mesh));

            return stream.Compose();
        }

        public static SWorldObject Deserialize(byte[] input) {
            SWorldObject result = new SWorldObject();
            ComposedByteStream stream = ComposedByteStream.FromByteArray(input);
            ComposedPrimitives prims = ComposedPrimitives.FromByteArray(stream.ReadNextStream<byte>());
            
            result.instanceID = prims.ReadNextValue<int>();
            result.activeInHierarchy = prims.ReadNextValue<bool>(); 
            result.color = new Color(
                    prims.ReadNextValue<float>(),
                    prims.ReadNextValue<float>(),
                    prims.ReadNextValue<float>(),
                    prims.ReadNextValue<float>());


            result.name = stream.ReadNextStream();
            if (stream.streamCount > 2) {
                int iMax = stream.streamCount - 2;
                result.meshes = new List<SMesh>(iMax);
                for (int i = 0; i < iMax; i++)
                    result.meshes.Add(SMesh.Deserialize(stream.ReadNextStream<byte>()));
            }

            stream.Dispose();
            return result;
        }

        internal void Dispose() {
            if (meshes != null) {
                foreach (SMesh mesh in meshes) {
                    if (mesh != null)
                        mesh.Dispose();
                }
            }
            meshes = null;
            name = null;
        }
    }



    [Serializable]
    public class SSubMeshArray {
        public int[][] triangles;
        public int[] materialIDs;

        public SSubMeshArray() //Default Constructor
        { }

        public SSubMeshArray(Mesh mesh) {
            if (mesh == null)
                return;

            int i = mesh.subMeshCount;
            triangles = new int[i][];
            while (--i > -1)
                triangles[i] = mesh.GetTriangles(i);
        }


        public static byte[] Serialize(SSubMeshArray input) {
            if (input == null || input.triangles == null || input.materialIDs == null)
                return null;

            //--------------- Triangles --------------------
            int i = input.triangles.Length;
            ComposedByteStream triangleStream = new ComposedByteStream(i);

            while (--i > -1)
                triangleStream.AddStream(input.triangles[i]);
            //--------------- Triangles --------------------

            ComposedByteStream merged = new ComposedByteStream(2);
            merged.AddStream(triangleStream.Compose());
            merged.AddStream(input.materialIDs);
            return merged.Compose();
        }


        public static SSubMeshArray Deserialize(byte[] input) {
            if (input == null || input.Length == 0)
                return null;

            SSubMeshArray result = new SSubMeshArray();
            ComposedByteStream merged = ComposedByteStream.FromByteArray(input);

            ComposedByteStream triangleStream = ComposedByteStream.FromByteArray(merged.ReadNextStream<byte>());
            result.materialIDs = merged.ReadNextStream<int>();


            //--------------- Triangles --------------------
            if (triangleStream != null && triangleStream.streamCount > 0) {
                int i = triangleStream.streamCount;
                result.triangles = new int[i][];

                while (--i > -1)
                    result.triangles[i] = triangleStream.ReadNextStream<int>();
            }
            //--------------- Triangles --------------------

            merged.Dispose();
            triangleStream.Dispose();
            return result;
        }

        internal void Dispose() {
            triangles = null;
            materialIDs = null;
        }
    }






    [Serializable]
    public class SMesh {

        [Serializable]
        public enum Type {
            bonesOnly,
            bonesKeyFrame,
            full
        }

        [Serializable]
        public class HeaderData {
            public bool activeInHierarchy;
            public string parentName;
            public string meshName;
            public int meshInstanceID = -1;
            public int goInstanceID = -1;
            public Type type;
            public SMatrix4x4 matrix;


            internal static byte[] Serialize(HeaderData input) {
                if (input == null)
                    return null;

                //Debug.Log(input.parentName + " [" + input.meshName + "] " + " -> eol: " + input.eol + ", active: " + input.activeInHierarchy);

                ComposedPrimitives prims = new ComposedPrimitives();
                prims.AddValue(input.activeInHierarchy);
                prims.AddValue(input.meshInstanceID);
                prims.AddValue(input.goInstanceID);
                prims.AddValue(input.type);
                
                ComposedByteStream stream = ComposedByteStream.FetchStream(4);
                stream.AddStream(input.parentName);
                stream.AddStream(input.meshName);
                stream.AddStream(prims.Compose());
                stream.AddStream(input.matrix.GetRawData());
                return stream.Compose();
            }

            public static HeaderData Deserialize(byte[] input) {
                if (input == null || input.Length < 32)
                    return null;

                HeaderData result = new HeaderData();
                ComposedByteStream stream = ComposedByteStream.FromByteArray(input);

                result.parentName = stream.ReadNextStream();
                result.meshName = stream.ReadNextStream();
                ComposedPrimitives prims = ComposedPrimitives.FromByteArray(stream.ReadNextStream<byte>());
                result.matrix = new SMatrix4x4(stream.ReadNextStream<float>());

                result.activeInHierarchy = prims.ReadNextValue<bool>();
                result.meshInstanceID = prims.ReadNextValue<int>();
                result.goInstanceID = prims.ReadNextValue<int>();
                result.type = prims.ReadNextValue<Type>();
                
                prims.Dispose();
                return result;
            }

            internal void Dispose() {
                parentName = null;
                meshName = null;
            }
        }


        public HeaderData header;
        public SSubMeshArray subMeshes;

        public int[] triangles;
        public SVector3Array vertices;
        public SVector2Array uv;
        public SVector2Array uv1;
        public SVector2Array uv2;

        public SMatrix4x4Array bindPoses;
        public SBonesArray bones;
        public SBoneWeightArray boneWeights;

        public SMesh() //Default Constructor
        { }

        public SMesh(Transform parent, string meshName, int meshInstanceID, bool isEnabled) {
            header = new HeaderData();
            header.activeInHierarchy = isEnabled;
            header.meshName = meshName;
            header.meshInstanceID = meshInstanceID;
            header.parentName = parent.name;
            header.goInstanceID = parent.gameObject.GetInstanceID();
            header.matrix = new SMatrix4x4(parent.localToWorldMatrix);
        }


        public static byte[] Serialize(SMesh input) {
            ComposedByteStream stream = ComposedByteStream.FetchStream(10);
            stream.AddStream(SMesh.HeaderData.Serialize(input.header));
            stream.AddStream(SSubMeshArray.Serialize(input.subMeshes));

            stream.AddStream(input.triangles);
            stream.AddStream(input.vertices != null ? input.vertices.vectorData : null);
            stream.AddStream(input.uv != null ? input.uv.vectorData : null);
            stream.AddStream(input.uv1 != null ? input.uv1.vectorData : null);
            stream.AddStream(input.uv2 != null ? input.uv2.vectorData : null);

            stream.AddStream(input.bindPoses != null ? input.bindPoses.matrixData : null);
            stream.AddStream(SBonesArray.Serialize(input.bones));
            stream.AddStream(SBoneWeightArray.Serialize(input.boneWeights));
            return stream.Compose();
        }

        public static SMesh Deserialize(byte[] input) {
            SMesh result = new SMesh();
            ComposedByteStream stream = ComposedByteStream.FromByteArray(input);

            result.header = SMesh.HeaderData.Deserialize(stream.ReadNextStream<byte>());
            result.subMeshes = SSubMeshArray.Deserialize(stream.ReadNextStream<byte>());

            result.triangles = stream.ReadNextStream<int>();
            result.vertices = new SVector3Array(stream.ReadNextStream<float>());
            result.uv = new SVector2Array(stream.ReadNextStream<float>());
            result.uv1 = new SVector2Array(stream.ReadNextStream<float>());
            result.uv2 = new SVector2Array(stream.ReadNextStream<float>());

            result.bindPoses = new SMatrix4x4Array(stream.ReadNextStream<float>());
            result.bones = SBonesArray.Deserialize(stream.ReadNextStream<byte>());
            result.boneWeights = SBoneWeightArray.Deserialize(stream.ReadNextStream<byte>());

            stream.Dispose();
            return result;
        }

        internal void Dispose() {
            if (header != null)     header.Dispose();
            if (subMeshes != null)  subMeshes.Dispose();
            if (vertices != null)   vertices.Dispose();
            if (uv != null)         uv.Dispose();
            if (uv1 != null)        uv1.Dispose();
            if (uv2 != null)        uv2.Dispose();
            if (bindPoses != null)  bindPoses.Dispose();
            if (bones != null)      bones.Dispose();
            if (boneWeights != null) boneWeights.Dispose();

            header = null;
            subMeshes = null;
            triangles = null;
            vertices = null;
            uv = null;
            uv1 = null;
            uv2 = null;
            bindPoses = null;
            bones = null;
            boneWeights = null;
        }
    }














    //--------------------------------------- SKINNED MESH RENDERER --------------------------------------
    //--------------------------------------- SKINNED MESH RENDERER --------------------------------------
    #region SKINNED MESH RENDERER


    [Serializable]
    public class SBone {
        public int instanceID;
        public int parentID;
        public SMatrix4x4 matrix;

        public SBone() //Default constructor.
        { }
    }



    [Serializable]
    public struct SBoneWeight {
        public int boneIndex0;
        public int boneIndex1;
        public int boneIndex2;
        public int boneIndex3;
        public float weight0;
        public float weight1;
        public float weight2;
        public float weight3;

        public SBoneWeight(BoneWeight source) {
            this.boneIndex0 = source.boneIndex0;
            this.boneIndex1 = source.boneIndex1;
            this.boneIndex2 = source.boneIndex2;
            this.boneIndex3 = source.boneIndex3;
            this.weight0 = source.weight0;
            this.weight1 = source.weight1;
            this.weight2 = source.weight2;
            this.weight3 = source.weight3;
        }
        public static implicit operator BoneWeight(SBoneWeight value) {
            BoneWeight result = new BoneWeight();

            result.boneIndex0 = value.boneIndex0;
            result.boneIndex1 = value.boneIndex1;
            result.boneIndex2 = value.boneIndex2;
            result.boneIndex3 = value.boneIndex3;
            result.weight0 = value.weight0;
            result.weight1 = value.weight1;
            result.weight2 = value.weight2;
            result.weight3 = value.weight3;
            return result;
        }
        public static implicit operator SBoneWeight(BoneWeight value) {
            return new SBoneWeight(value);
        }
    }



    [Serializable]
    public class SBonesArray {
        public int count;
        public int[] instanceIDs;
        public int[] parentIDs;
        public SMatrix4x4Array matrices;

        public SBonesArray() //Default constructor.
        { }

        public SBonesArray(Transform[] input, int rootID) {
            count = input.Length;
            instanceIDs = new int[count];
            parentIDs = new int[count];

            Matrix4x4[] worldMatrices = new Matrix4x4[count];
            int iMax = input.Length;

            for (int i = 0; i < iMax; i++) {
                Transform bone = input[i];
                int childID = bone.GetInstanceID();
                instanceIDs[i] = childID;

                if (bone.parent != null) {
                    int parentID = bone.parent.GetInstanceID();
                    if (parentID != rootID) {
                        parentIDs[i] = parentID;
                    } else {
                        parentIDs[i] = -1; //Known root element
                    }
                } else {
                    parentIDs[i] = -1; //probible root element
                }
                worldMatrices[i] = bone.localToWorldMatrix;
            }

            //Debug.Log("test: " + test);
            matrices = new SMatrix4x4Array(worldMatrices);
        }

        public static byte[] Serialize(SBonesArray input) {
            ComposedByteStream stream = ComposedByteStream.FetchStream();
            if (input != null) {
                stream.AddStream(input.instanceIDs);
                stream.AddStream(input.parentIDs);
                stream.AddStream(input.matrices.matrixData);
            } else {
                stream.AddEmptyStreams(3);
            }
            return stream.Compose();
        }
        public static SBonesArray Deserialize(byte[] input) {
            SBonesArray result = new SBonesArray();
            ComposedByteStream stream = ComposedByteStream.FromByteArray(input);
            if (stream == null)
                return null;

            result.instanceIDs = stream.ReadNextStream<int>();
            result.parentIDs = stream.ReadNextStream<int>();
            result.matrices = new SMatrix4x4Array(stream.ReadNextStream<float>());
            result.count = result.matrices.count;
            stream.Dispose();
            return result;
        }

        internal void Dispose() {
            if (matrices != null)
                matrices.Dispose();

            instanceIDs = null;
            parentIDs = null;
            matrices = null;
        }
    }



    [Serializable]
    public class SBoneWeightArray {
        public int count;
        public int[] indices;
        public float[] weights;

        public SBoneWeightArray() //Default constructor.
        { }
        public SBoneWeightArray(BoneWeight[] input) //Default constructor.
        {
            count = input.Length;
            indices = new int[count * 4];
            weights = new float[count * 4];

            int i = 0;
            int j = 0;

            while (i < count) {
                BoneWeight boneWeight = input[i];
                indices[j] = boneWeight.boneIndex0; weights[j] = boneWeight.weight0; j++;
                indices[j] = boneWeight.boneIndex1; weights[j] = boneWeight.weight1; j++;
                indices[j] = boneWeight.boneIndex2; weights[j] = boneWeight.weight2; j++;
                indices[j] = boneWeight.boneIndex3; weights[j] = boneWeight.weight3; j++;
                i++;
            }
        }

        public BoneWeight GetBoneWeight(int origionalIndex) {
            int idx = origionalIndex * 4;
            BoneWeight result = new BoneWeight();

            result.boneIndex0 = indices[idx]; result.weight0 = weights[idx]; idx++;
            result.boneIndex1 = indices[idx]; result.weight1 = weights[idx]; idx++;
            result.boneIndex2 = indices[idx]; result.weight2 = weights[idx]; idx++;
            result.boneIndex3 = indices[idx]; result.weight3 = weights[idx];

            return result;
        }

        public static byte[] Serialize(SBoneWeightArray input) {
            ComposedByteStream stream = ComposedByteStream.FetchStream();
            if (input != null) {
                stream.AddStream(input.indices);
                stream.AddStream(input.weights);
            } else {
                stream.AddEmptyStreams(2);
            }
            return stream.Compose();
        }
        public static SBoneWeightArray Deserialize(byte[] input) {
            SBoneWeightArray result = new SBoneWeightArray();
            ComposedByteStream stream = ComposedByteStream.FromByteArray(input);
            result.indices = stream.ReadNextStream<int>();
            result.weights = stream.ReadNextStream<float>();
            result.count = result.weights.Length / 4;
            stream.Dispose();
            return result;
        }

        internal void Dispose() {
            indices = null;
            weights = null;
        }
    }

    #endregion
    //--------------------------------------- SKINNED MESH RENDERER --------------------------------------
    //--------------------------------------- SKINNED MESH RENDERER --------------------------------------
			
}
