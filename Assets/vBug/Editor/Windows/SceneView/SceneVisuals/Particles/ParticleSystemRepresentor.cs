using System;
using UnityEngine;
using Frankfort.VBug.Internal;
using UnityEditor;

namespace Frankfort.VBug.Editor
{
    [AddComponentMenu(""), ExecuteInEditMode()]
    public class ParticleSystemSurrogate : MonoBehaviour {//Rename because of 'Its not a monobehaviour' error @Unity-Free
        private int randomKey;
        private Mesh mesh;
        private Material mat;
        private MeshFilter meshFilter;
        private MeshRenderer meshRenderer;

        private SGenericParticleArray particleData;
        private SParticleRenderer particleRenderer;
        public int currentFrameNumber = -1;

        public bool isVisible {
            set {
                if (meshRenderer != null)
                    meshRenderer.enabled = value;
            }
        }
        
        public static ParticleSystemSurrogate Create(string name)
        {
            GameObject go = new GameObject("vBugParticleSystemRepresentor_" + name);
            ParticleSystemSurrogate representor = go.AddComponent<ParticleSystemSurrogate>();
            return representor;
        }

        private void Awake() {
            meshFilter = gameObject.AddComponent<MeshFilter>();
            meshRenderer = gameObject.AddComponent<MeshRenderer>();
            mesh = new Mesh();

            meshFilter.mesh = mesh = new Mesh();
            Frankfort.VBug.Internal.GameObjectUtility.SetHideFlagsRecursively(this.gameObject);
        }


        private void OnDestroy()
        {
            if (this.mesh != null)
                UnityEngine.Object.DestroyImmediate(this.mesh);

            if (mat != null)
                UnityEngine.Object.DestroyImmediate(mat);

            mat = null;
            mesh = null;
            meshFilter = null;
            meshRenderer = null;
            particleData = null;
        }


        public void SetParticles(int frameNumber, SParticleRenderer particleRenderer, SGenericParticleArray particleData)
        {
            this.particleRenderer = particleRenderer;
            this.particleData = particleData;

            if (frameNumber != currentFrameNumber){
                SetMaterial(particleRenderer, frameNumber);
                Render();
                currentFrameNumber = frameNumber;
            }
        }

        private void SetMaterial(SParticleRenderer particleRenderer, int frameNumber) {
            Material newMat = EditorMaterialHelper.GetClosestAvailableMaterialByID(particleRenderer.materialInstanceID, frameNumber);
            if(newMat != null){
                if (mat != null)
                    UnityEngine.Object.DestroyImmediate(mat);

                mat = newMat;
                meshRenderer.material = newMat;
            }
            EditorMaterialHelper.SetFallbackMaterial(meshRenderer);
        }


        private void Render()
        {
            if (this.particleData == null || this.particleRenderer == null)
                return;

            particleRenderer.camUp.Normalize();
            
            int iMax = particleData.sizes.Length;
            int vMax = iMax * 4;
            int t = 0;
            int v = 0;

            int[] triangles = new int[iMax * 6];
            Vector3[] vertices = new Vector3[vMax];
            Vector3[] normals = new Vector3[vMax];
            Vector4[] tangents = new Vector4[vMax];
            Vector2[] uvs = new Vector2[vMax];
            Color32[] colors = new Color32[vMax];

            for (int i = 0; i < iMax; i++)
            {
                int iThree = i * 3;
                int iFour = i * 4;
                float halfSize = particleData.sizes[i] / 2f;

                Color32 color = new Color32(particleData.colors[iFour], particleData.colors[iFour + 1], particleData.colors[iFour + 2], particleData.colors[iFour + 3]);
                Vector3 center = new Vector3(particleData.positions[iThree], particleData.positions[iThree + 1], particleData.positions[iThree + 2]);
                
                Vector3 normal = (center - particleRenderer.camPos).normalized;
                Vector3 up = particleRenderer.camUp;
                Vector3 right = Vector3.right;
                Vector3 tangent = right;

                switch (particleRenderer.renderMode)
                {
                    case SParticleRenderMode.Billboard:
                        right = Vector3.Cross(normal, up);
                        break;

                    case SParticleRenderMode.VerticalBillboard:
                        normal.y = 0f;
                        normal.Normalize();
                        right = Vector3.Cross(normal, up);
                        break;
                        
                    case SParticleRenderMode.HorizontalBillboard:
                        up.y = 0f;
                        up.Normalize();
                        normal = Vector3.up;
                        right = Vector3.Cross(up, normal);
                        break;

                    case SParticleRenderMode.Stretch:
                        Vector3 velocity = new Vector3(
                            particleData.velocities[iThree],
                            particleData.velocities[iThree + 1],
                            particleData.velocities[iThree + 2]);

                        right = velocity.normalized;
                        up = Vector3.Cross(right, normal);

                        right *= particleRenderer.lengthScale;
                        right += velocity * particleRenderer.velocityScale;
                        break;
                }
                up *= halfSize;
                right *= halfSize;


                triangles[t++] = v;
                triangles[t++] = v + 1;
                triangles[t++] = v + 2;
                triangles[t++] = v + 2;
                triangles[t++] = v + 3;
                triangles[t++] = v;

                normals[v] = normal; tangents[v] = tangent; colors[v] = color; vertices[v] = center - right - up; uvs[v] = new Vector2(0, 0);
                v++;
                normals[v] = normal; tangents[v] = tangent; colors[v] = color; vertices[v] = center + right - up; uvs[v] = new Vector2(1, 0);
                v++;
                normals[v] = normal; tangents[v] = tangent; colors[v] = color; vertices[v] = center + right + up; uvs[v] = new Vector2(1, 1);
                v++;
                normals[v] = normal; tangents[v] = tangent; colors[v] = color; vertices[v] = center - right + up; uvs[v] = new Vector2(0, 1);
                v++;
            }
            
            mesh.Clear();
            mesh.vertices = vertices;
            mesh.normals = normals;
            mesh.tangents = tangents;
            mesh.uv = uvs;
            mesh.colors32 = colors;
            mesh.triangles = triangles;
        }


        public void DrawWireframe(Color colorQuad, Color colorForward){
            if(mesh != null && mesh.vertexCount > 0) {

                for(int i = 0; i < mesh.vertexCount; i += 4){
                    Vector3 p0 = mesh.vertices[i + 0];
                    Vector3 p1 = mesh.vertices[i + 1];
                    Vector3 p2 = mesh.vertices[i + 2];
                    Vector3 p3 = mesh.vertices[i + 3];
                    Vector3 pCenter = (p0 + p1 + p2 + p3) / 4f;
                    Vector3 pForward = pCenter - ((Vector3.Cross(p3 - p0, p1 - p0).normalized * (p2 - p0).magnitude / 4f));
                    
                    /*
                    EditorHelper.SceneviewDrawLine(p0, p1, colorQuad);
                    EditorHelper.SceneviewDrawLine(p1, p2, colorQuad);
                    EditorHelper.SceneviewDrawLine(p2, p3, colorQuad);
                    EditorHelper.SceneviewDrawLine(p3, p0, colorQuad);
                    EditorHelper.SceneviewDrawLine(pCenter, pForward, colorForward);
                     */

                    /*
                    Debug.DrawLine(p0, p1, colorQuad);
                    Debug.DrawLine(p1, p2, colorQuad);
                    Debug.DrawLine(p2, p3, colorQuad);
                    Debug.DrawLine(p3, p0, colorQuad);
                    Debug.DrawLine(pCenter, pForward, colorForward);
                     */

                    Handles.color = colorQuad;
                    Handles.DrawAAPolyLine(HandleUtility.WorldToGUIPoint(p0), HandleUtility.WorldToGUIPoint(p1));
                    Handles.DrawAAPolyLine(HandleUtility.WorldToGUIPoint(p1), HandleUtility.WorldToGUIPoint(p2));
                    Handles.DrawAAPolyLine(HandleUtility.WorldToGUIPoint(p2), HandleUtility.WorldToGUIPoint(p3));
                    Handles.DrawAAPolyLine(HandleUtility.WorldToGUIPoint(p3), HandleUtility.WorldToGUIPoint(p0));
                    Handles.color = colorForward;
                    Handles.DrawAAPolyLine(HandleUtility.WorldToGUIPoint(pCenter), HandleUtility.WorldToGUIPoint(pForward));
                }
            }
        }
    }
}
