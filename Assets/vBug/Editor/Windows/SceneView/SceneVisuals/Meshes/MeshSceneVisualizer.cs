using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Frankfort.VBug.Internal;


namespace Frankfort.VBug.Editor {

    public class MeshSceneVisualizer : IBaseSceneVisualizer {
        private int currentFrameNumber;
        private List<SWorldObject> currentWorldObjects;
        private string currentLevelName;
        private int isMouseOverID = -1;
        private int isMouseTapID = -1;

        private Dictionary<int, MeshRendererSurrogate> representors = new Dictionary<int, MeshRendererSurrogate>();
        private Dictionary<long, Vector3[]> ghostPointsPerWorldObject = new Dictionary<long, Vector3[]>();
        private Dictionary<long, Color[]> ghostColorsPerWorldObject = new Dictionary<long, Color[]>();
        private Dictionary<long, Bounds> boundsPerFrame = new Dictionary<long, Bounds>();
        private Dictionary<long, string> infoPerFrame = new Dictionary<long, string>();
        private List<MeshRendererSurrogate> mouseOverRepresentors = new List<MeshRendererSurrogate>();



        //--------------- Enable / Disable --------------------
        private SceneVisualState _state = SceneVisualState.full;
        public SceneVisualState state {
            get {
                return _state;
            }
             
            set {
                if (_state == value)
                    return;

                _state = value;
                if (_state == SceneVisualState.off) {
                    PruneAllVisualizers();
                } else {
                    currentFrameNumber = -1;
                    DrawSlice(vBugWindowMediator.currentFrameNumber);
                }
            }
        }
        //--------------- Enable / Disable --------------------



        //--------------- External commands --------------------
        public void Dispose() {
            PruneAllVisualizers();
            currentWorldObjects = null;
            currentLevelName = null;
            currentFrameNumber = -1;
        }


        public void OnDatabaseUpdated() {
            Reset();
            DrawSlice(vBugWindowMediator.currentFrameNumber);
        }


        public void PruneAllVisualizers() {
            MeshRendererSurrogate[] leftovers = Resources.FindObjectsOfTypeAll<MeshRendererSurrogate>();
            int i = leftovers.Length;
            while (--i > -1)
                GameObject.DestroyImmediate(leftovers[i].gameObject);

            representors.Clear();
            currentFrameNumber = 0;
            currentWorldObjects = null;
            Reset();
        }


        public void Reset() {
            ghostPointsPerWorldObject.Clear();
            ghostColorsPerWorldObject.Clear();
            boundsPerFrame.Clear();
            infoPerFrame.Clear();
        }
        //--------------- External commands --------------------






        public void DrawSlice(int frameNumber) {
            if (_state == SceneVisualState.off || currentFrameNumber == frameNumber)
                return;

            VerticalActivitySlice slice = GetClosestAvailableSlice(frameNumber);
            if (slice == null) {
                PruneAllVisualizers();
                return;
            }

            if (currentLevelName != slice.header.levelName) {
                PruneAllVisualizers();
                currentLevelName = slice.header.levelName;
            }

            if (slice.meshData == null || slice.meshData.worldObjects == null) {
                return;

            } else {
                currentFrameNumber = slice.header.frameNumber;
                currentWorldObjects = slice.meshData.worldObjects;
                List<int> updatedIDs = new List<int>();
                List<int> missingIDs = new List<int>();

                //--------------- Create and set frame-data --------------------
                foreach (SWorldObject wo in slice.meshData.worldObjects) {
                    if (!wo.activeInHierarchy || wo.meshes == null || wo.meshes.Count == 0)
                        continue;

                    foreach (SMesh mesh in wo.meshes) {
                        if (mesh.header == null)
                            return;

                        if (!representors.ContainsKey(mesh.header.goInstanceID))
                            representors.Add(mesh.header.goInstanceID, MeshRendererSurrogate.Create(wo.instanceID, mesh.header.goInstanceID, wo.name + "_" + mesh.header.parentName));

                        representors[mesh.header.goInstanceID].SetCurrentFrame(slice.header.frameNumber);
                        updatedIDs.Add(mesh.header.goInstanceID);
                    }
                }
                //--------------- Create and set frame-data --------------------

                //--------------- Remove missing --------------------
                foreach (KeyValuePair<int, MeshRendererSurrogate> pair in representors) {
                    if (!updatedIDs.Contains(pair.Key))
                        missingIDs.Add(pair.Key);
                }

                if (missingIDs.Count > 0) {
                    foreach (int id in missingIDs) {
                        GameObject.DestroyImmediate(representors[id].gameObject);
                        representors.Remove(id);
                    }
                }
                //--------------- Remove missing --------------------
            }
        }








        //--------------------------------------- RENDER UPDATE --------------------------------------
        //--------------------------------------- RENDER UPDATE --------------------------------------
        #region RENDER UPDATE


        public void RenderMouseOverSceneView(SceneView view) {
            if (_state == SceneVisualState.off)
                return;

            Plane[] planes = GeometryUtility.CalculateFrustumPlanes(view.camera);
            Update2DOverlay(view, planes);
            UpdateClipping(view, planes);
            DrawSceneBB(planes);
        }

        public void RenderAnySceneView(SceneView view) {
            if (_state == SceneVisualState.off)
                return;

            DrawCurrentWOsBones();
        }


        public void Update2DOverlay(SceneView view, Plane[] planes) {
            if (currentWorldObjects == null)
                return;

            isMouseOverID = -1;
            Rect isMouseOverRect = default(Rect);
            bool mouseOverOccurred = false;

            foreach (SWorldObject obj in currentWorldObjects) {

                Bounds bounds;
                if (GetBoundsFromVisualizers(obj, currentFrameNumber, out bounds) && GeometryUtility.TestPlanesAABB(planes, bounds)) {
                    Rect bbRect = SceneVisualsHelper.GetRectFromBounds(bounds);

                    if (isMouseOverID != -1 && isMouseOverRect.Contains(bbRect.center))
                        continue;

                    Rect iconRect = VisualResources.DrawIcon(EditorIcon.playbackToggles, 3, bbRect.center, 1f, true);
                    if (iconRect.Contains(Event.current.mousePosition)) {
                        isMouseOverID = obj.instanceID;
                        UpdateMouseOverRepresentors(isMouseOverID);
                        mouseOverOccurred = true;

                        //--------------- Arrow & BB Rect --------------------
                        SceneVisualsHelper.DrawBBArrow(bbRect);
                        //SceneVisualsHelper.DrawBBRect(bbRect);
                        //--------------- Arrow & BB Rect --------------------

                        //--------------- Layout init --------------------
                        float width = 200f;
                        float subHeight = 58f;
                        float infoHeight = 20 + ((obj.meshes != null && obj.meshes.Count > 0) ? obj.meshes.Count * subHeight : 15f);
                        float left = bbRect.xMax + 16;
                        float top = bbRect.y + (bbRect.height / 2f) - (infoHeight / 2f);

                        GUI.contentColor = Color.white;
                        EditorHelper.styleLabelSceneVisuals.fontSize = 12;
                        //--------------- Layout init --------------------

                        //--------------- Label --------------------
                        Rect labelRect = new Rect(left, top, width, 20);
                        VisualResources.DrawTexture(labelRect, VisualResources.proConsoleLineDark);
                        GUI.Label(labelRect, !string.IsNullOrEmpty(obj.name) ? obj.name : "N/A", EditorHelper.styleLabelSceneVisuals);
                        //--------------- Label --------------------

                        //--------------- Mesh Info --------------------
                        GUI.contentColor = Color.black;
                        EditorHelper.styleLabelSceneVisuals.fontSize = 9;
                        Rect contentRect = new Rect(left, top + 20, width, infoHeight - 20);
                        isMouseOverRect = contentRect;

                        VisualResources.DrawTexture(contentRect, VisualResources.gray);
                        if (obj.meshes != null && obj.meshes.Count > 0) {
                            for (int i = 0; i < obj.meshes.Count; i++) {
                                SMesh mesh = obj.meshes[i];
                                if (mesh == null)
                                    continue;

                                Rect subContentRect = new Rect(left, top + 20 + (i * subHeight), width, subHeight);
                                GUI.Label(subContentRect, GetMeshInfoByID(currentFrameNumber, mesh.header.goInstanceID), EditorHelper.styleLabelSceneVisuals);

                                if (i > 0) {
                                    Rect horBar = new Rect(subContentRect.x + 3, subContentRect.y, width - 6, 2);
                                    VisualResources.DrawTexture(horBar, VisualResources.proConsoleLineDark);
                                }
                            }

                            GUI.Label(contentRect, "", EditorHelper.styleLabelSceneVisuals);
                        } else {
                            GUI.Label(contentRect, "No mesh-data found", EditorHelper.styleLabelSceneVisuals);
                        }
                        //--------------- Mesh Info --------------------


                        //--------------- Navigation --------------------
                        //if (Event.current.type == EventType.MouseDown) {
                        if (Handles.Button(bounds.center, Quaternion.identity, 10, 10, Handles.CubeCap)) {
                            if (isMouseTapID == isMouseOverID) { //already selected
                                Transform camTrans = view.camera.transform;
                                view.LookAt(bounds.center, Quaternion.LookRotation(bounds.center - camTrans.position, Vector3.up), Mathf.Max(1, bounds.extents.magnitude * 2f));

                                if (vBugBaseWindow.CheckIfOpen(typeof(vBugHierarchyWindow))) {
                                    vBugHierarchyWindow hierarchyWin = EditorWindow.GetWindow<vBugHierarchyWindow>();
                                    if (hierarchyWin != null)
                                        hierarchyWin.NavigateToID(isMouseOverID);
                                }
                            }
                            isMouseTapID = isMouseOverID;
                        }
                        //--------------- Navigation --------------------

                    }
                }

                GUI.contentColor = Color.white;
            }

            if (!mouseOverOccurred) {
                if (mouseOverRepresentors.Count > 0)
                    SceneView.RepaintAll();

                mouseOverRepresentors.Clear();
            }
        }


        private string GetMeshInfoByID(int frameNumber, int meshGOInstanceID) {
            long key = MathHelper.CombineInts(frameNumber, meshGOInstanceID);

            if (infoPerFrame.ContainsKey(key)) {
                return infoPerFrame[key];
            } else {

                for (int i = frameNumber; i >= 0; i--) {
                    SMesh mesh = SceneVisualsHelper.GetMeshByFrameNumber(i, meshGOInstanceID);
                    if (mesh == null || !SceneVisualsHelper.CheckMeshKeyFrame(mesh))
                        continue;

                    string info =
                        "Mesh name: " + mesh.header.meshName + "\n" +
                        "Parent name: " + mesh.header.parentName + "\n" +
                        "Material count: " + (mesh.subMeshes != null ? mesh.subMeshes.materialIDs.Length.ToString() : "n/a") + "\n" +
                        "Triangle count: " + (mesh.triangles != null ? (mesh.triangles.Length / 3).ToString() : "n/a") + "\n" +
                        "Vertex count: " + (mesh.vertices != null ? mesh.vertices.count.ToString() : "n/a");

                    infoPerFrame.Add(key, info);
                    return info;
                }
            }

            return "No mesh keyframe-data found";
        }




        private bool CheckVisibility(SWorldObject obj) {
            if (obj == null || obj.meshes == null || obj.meshes.Count == 0)
                return false;

            foreach (SMesh mesh in obj.meshes) {
                if (mesh.header.activeInHierarchy && representors.ContainsKey(mesh.header.goInstanceID)) {
                    MeshRendererSurrogate representor = representors[mesh.header.goInstanceID];
                    if (representor.isVisible && representor.isEnabled)
                        return true;
                }
            }
            return false;
        }



        private void DrawCurrentWOsBones() {
            if (currentWorldObjects == null)
                return;

            //--------------- Draw bones --------------------
            if (Event.current.type == EventType.layout) {
                foreach (SWorldObject obj in currentWorldObjects) {
                    if (!CheckVisibility(obj))
                        continue;

                    DrawLineOverlays(obj, currentFrameNumber);
                    if (isMouseOverID == obj.instanceID) {
                        if (obj.meshes != null && obj.meshes.Count > 0) {
                            foreach (SMesh mesh in obj.meshes) {
                                if (representors.ContainsKey(mesh.header.goInstanceID))
                                    DrawMeshBones(representors[mesh.header.goInstanceID], obj.color, currentFrameNumber);
                            }
                        }
                    }
                }
            }
            //--------------- Draw bones --------------------
        }

        private void DrawMeshBones(MeshRendererSurrogate representor, Color color, int frameNumber) {
            if (_state == SceneVisualState.off)
                return;

            if (representor == null)
                return;

            int halfRange = Math.Max(1, vBugEditorSettings.PlaybackMeshGhostLength / 2);
            int iStart = Mathf.Max(VerticalSliceDatabase.minRange, frameNumber - halfRange);
            int iEnd = Mathf.Min(VerticalSliceDatabase.maxRange, frameNumber + halfRange);

            int increment = vBugEditorSettings.PlaybackMeshBonesInterval;
            for (int i = iStart; i < iEnd; i += increment)
                representor.DrawBones(i, color);
        }


        private void DrawLineOverlays(SWorldObject obj, int frameNumber) {
            if (_state != SceneVisualState.full)
                return;

            Vector3[] points = null;
            Color[] colors = null;
            GetGhostPoints(obj, frameNumber, out points, out colors);

            if (points != null && colors != null) {
                int iMax = points.Length;
                for (int i = 1; i < iMax; i++)
                    EditorHelper.SceneviewDrawLine(points[i], points[i - 1], colors[i], 6f);
            }
        }


        private void DrawSceneBB(Plane[] planes) {
            if (currentWorldObjects == null)
                return;

            foreach (MeshRendererSurrogate representor in mouseOverRepresentors)
                representor.DrawBoundingBox(currentFrameNumber, Color.red, planes);
        }


        private void UpdateMouseOverRepresentors(int mouseOverID) {
            mouseOverRepresentors.Clear();
            if (currentWorldObjects == null)
                return;

            foreach (SWorldObject wo in currentWorldObjects) {
                if (wo.meshes == null || wo.instanceID != mouseOverID)
                    continue;

                foreach (SMesh mesh in wo.meshes) {
                    if (representors.ContainsKey(mesh.header.goInstanceID))
                        mouseOverRepresentors.Add(representors[mesh.header.goInstanceID]);
                }
            }
        }


        #endregion
        //--------------------------------------- RENDER UPDATE --------------------------------------
        //--------------------------------------- RENDER UPDATE --------------------------------------











        //--------------------------------------- GET MESH DATA --------------------------------------
        //--------------------------------------- GET MESH DATA --------------------------------------
        #region GET MESH DATA

         
        private VerticalActivitySlice GetClosestAvailableSlice(int frameNumber) {
            int iMin = Mathf.Max(VerticalSliceDatabase.minRange, frameNumber - vBugEditorSettings.PlaybackMeshSearchRange);
            for (int i = frameNumber; i >= iMin; i--) {
                VerticalActivitySlice result = VerticalSliceDatabase.GetSlice(i);

                if (result != null && result.meshData != null && result.meshData.worldObjects != null && result.meshData.worldObjects.Count > 0)
                    return result;
            }

            return null;
        }


        private SMatrix4x4 GetRootBone(SMesh mesh) {
            if (mesh == null || mesh.bones == null || mesh.bones.parentIDs == null)
                return default(SMatrix4x4);

            for (int i = 0; i < mesh.bones.instanceIDs.Length; i++) {
                if (SceneVisualsHelper.GetBoneByID(mesh.bones, mesh.bones.parentIDs[i]) == -1)
                    return mesh.bones.matrices.GetMatrix(i);
            }
            return default(SMatrix4x4);
        }



        private bool GetBoundsFromVisualizers(SWorldObject obj, int frameNumber, out Bounds result) {

            //--------------- From cache --------------------
            long key = MathHelper.CombineInts(obj.instanceID, frameNumber);
            if (boundsPerFrame.ContainsKey(key)) {
                result = boundsPerFrame[key];
                return true;
            }
            //--------------- From cache --------------------

            Vector3 min = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
            Vector3 max = new Vector3(float.MinValue, float.MinValue, float.MinValue);
            bool found = false;

            foreach (KeyValuePair<int, MeshRendererSurrogate> pair in representors) {
                if (obj.meshes == null)
                    continue;

                int idx = obj.meshes.FindIndex(item => item.header.goInstanceID == pair.Key);
                if (idx != -1) {

                    Bounds bounds;
                    if (pair.Value.GetBounds(frameNumber, out bounds)) {
                        min.x = Mathf.Min(min.x, bounds.min.x);
                        min.y = Mathf.Min(min.y, bounds.min.y);
                        min.z = Mathf.Min(min.z, bounds.min.z);
                        max.x = Mathf.Max(max.x, bounds.max.x);
                        max.y = Mathf.Max(max.y, bounds.max.y);
                        max.z = Mathf.Max(max.z, bounds.max.z);
                    }
                    found = true;
                }
            }

            if (found) {
                result = new Bounds((min + max) / 2f, max - min);
                boundsPerFrame.Add(key, result);
                return true;
            }

            result = default(Bounds);
            return false;
        }



        //TODO: Fix
        private void GetGhostPoints(SWorldObject obj, int frameNumber, out Vector3[] points, out Color[] colors) {
            points = null;
            colors = null;

            if (vBugEditorSettings.PlaybackMeshGhostLength <= 0)
                return;

            int iStart = Mathf.Clamp(frameNumber - (vBugEditorSettings.PlaybackMeshGhostLength / 2), VerticalSliceDatabase.minRange, VerticalSliceDatabase.maxRange);
            int iEnd = Mathf.Clamp(frameNumber + (vBugEditorSettings.PlaybackMeshGhostLength / 2), VerticalSliceDatabase.minRange, VerticalSliceDatabase.maxRange);

            if (iEnd <= iStart)
                return;
            //--------------- Start & End --------------------


            //--------------- Get from cache --------------------
            long key = MathHelper.CombineInts(obj.instanceID, frameNumber);
            if (ghostColorsPerWorldObject.ContainsKey(key)) {
                points = ghostPointsPerWorldObject[key];
                colors = ghostColorsPerWorldObject[key];
                return;
            }
            //--------------- Get from cache --------------------


            //--------------- Generate new --------------------
            List<Vector3> pointList = new List<Vector3>();
            List<Color> colorList = new List<Color>();
            Color cF = new Color(obj.color.r * 2f, obj.color.g * 2f, obj.color.b * 2f, obj.color.a);
            Color cB = new Color(0.666f, 0.666f, 0.666f, obj.color.a / 2f);
            float alphaIncForward = 1f / (frameNumber - iStart - 1);
            float alphaIncBackward = 1f / (iEnd - frameNumber);

            for (int i = iStart; i < iEnd; i++) {
                Vector3 pos;
                bool activeInHierarchy;

                if (GetBestRootByWorldObject(obj.instanceID, i, out pos, out activeInHierarchy)) { //Returns false when missing a frame
                    pointList.Add(pos);

                    if (!activeInHierarchy) {
                        colorList.Add(new Color(0.5f, 0.5f, 0.5f, 0.5f));
                    } else if (i >= frameNumber) {
                        colorList.Add(new Color(cF.r, cF.g, cF.b, (float)(iEnd - i) * alphaIncForward * cF.a));
                    } else {
                        colorList.Add(new Color(cB.r, cB.g, cB.b, (float)(i - iStart) * alphaIncBackward * cF.a));
                    }
                }
            }


            points = pointList.ToArray();
            colors = colorList.ToArray();
            ghostPointsPerWorldObject.Add(key, points);
            ghostColorsPerWorldObject.Add(key, colors);
            //--------------- Generate new --------------------
        }


        private bool GetBestRootByWorldObject(int woInstanceID, int frameNumber, out Vector3 pos, out bool activeInHierarchy) {
            VerticalActivitySlice slice = VerticalSliceDatabase.GetSlice(frameNumber);
            if (slice == null || slice.meshData == null || slice.meshData.worldObjects == null) {
                pos = Vector3.zero;
                activeInHierarchy = false;
                return false;
            }

            SWorldObject obj = slice.meshData.worldObjects.Find(item => item.instanceID == woInstanceID);
            if (obj == null || obj.meshes == null) {
                pos = Vector3.zero;
                activeInHierarchy = false;
                return false;
            }

            activeInHierarchy = obj.activeInHierarchy;
            int bestBonesCount = 0;
            int bestMeshIdx = -1;

            for (int i = 0; i < obj.meshes.Count; i++) {
                SMesh mesh = obj.meshes[i];
                if (mesh.bones != null && mesh.bones.count > bestBonesCount) {
                    bestBonesCount = mesh.bones.count;
                    bestMeshIdx = i;
                }
            }

            if (bestMeshIdx != -1) {
                pos = GetRootBone(obj.meshes[bestMeshIdx]).position;
            } else {
                pos = GetAverageMeshPos(obj.meshes); 
            }
            return true;
        }

        private Vector3 GetAverageMeshPos(List<SMesh> list) {
            Vector3 pos = Vector3.zero;
            foreach (SMesh mesh in list)
                pos += mesh.header.matrix.position;

            return pos / (float)list.Count;
        }



        #endregion
        //--------------------------------------- GET MESH DATA --------------------------------------
        //--------------------------------------- GET MESH DATA --------------------------------------













        //--------------------------------------- CLIPPING --------------------------------------
        //--------------------------------------- CLIPPING --------------------------------------
        #region CLIPPING

        private void UpdateClipping(SceneView view, Plane[] planes) {
            if (representors == null || representors.Count == 0)
                return;

            if (view.camera == null)
                return;

            foreach (KeyValuePair<int, MeshRendererSurrogate> pair in representors)
                pair.Value.UpdateClipping(currentFrameNumber, planes);
        }



        #endregion
        //--------------------------------------- CLIPPING --------------------------------------
        //--------------------------------------- CLIPPING --------------------------------------

    }

}
