using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


namespace Frankfort.VBug.Internal
{
    public static class MeshDataChangeDetector
    {
        private struct MeshDataMirror
        {
            public Vector3 position;
            public Vector3 scale;
            public Quaternion rotation;
            
            public int meshInstanceID;
            public int vertexCount;
            public int subMeshCount;
            public int blendShapeCount;
            //public int triangleCount;
            //public int tangentCount;
            //public int uvCount;
            public Vector3 extents;
        }


        private static Dictionary<int, MeshDataMirror> detectChangeTable = new Dictionary<int, MeshDataMirror>();

        public static void Reset()
        {
            detectChangeTable.Clear();
        }

        public static bool DetectChange(int id, Mesh mesh, Transform transform = null)
        {
            if (mesh == null)
                return false;

            if (detectChangeTable.ContainsKey(id))
            {
                MeshDataMirror mirror = detectChangeTable[id];

                bool change = false;
                if (transform != null) {
                    change =
                        mirror.position != transform.position ||
                        mirror.rotation != transform.rotation ||
                        mirror.scale != transform.GetScale();
                }

                if (!change) {
                    change =
                    mirror.meshInstanceID != mesh.GetInstanceID() ||
                    mirror.vertexCount != mesh.vertexCount ||
                    mirror.blendShapeCount != mesh.blendShapeCount ||
                    mirror.subMeshCount != mesh.subMeshCount ||
                        //mirror.triangleCount != mesh.triangles.Length ||
                        //mirror.tangentCount != mesh.tangents.Length ||
                        //mirror.uvCount != mesh.uv.Length ||
                    mirror.extents != mesh.bounds.extents;
                }

                return change;
            } else {
                MeshDataMirror mirror = new MeshDataMirror();
                mirror.meshInstanceID = mesh.GetInstanceID();
                mirror.vertexCount = mesh.vertexCount;
                mirror.blendShapeCount = mesh.blendShapeCount;
                mirror.subMeshCount = mesh.subMeshCount;
                //mirror.triangleCount = mesh.triangles.Length;
                //mirror.tangentCount = mesh.tangents.Length;
                //mirror.uvCount = mesh.uv.Length;
                mirror.extents = mesh.bounds.extents;

                if (transform != null) {
                    mirror.position = transform.position;
                    mirror.rotation = transform.rotation;
                    mirror.scale = transform.GetScale();
                }

                detectChangeTable.Add(id, mirror);
                return true;
            }
        }
    }
}