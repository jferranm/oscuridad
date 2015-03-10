using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Frankfort.VBug.Internal;


namespace Frankfort.VBug.Editor
{
    public class ParticleSceneVisualizer : IBaseSceneVisualizer
    {

        //--------------- Tweakables --------------------
        Color colorQuad = new Color(0.5f, 0f, 0f);
        Color colorForward = Color.red;
        //--------------- Tweakables --------------------
			

        private Dictionary<int, ParticleSystemSurrogate> representors = new Dictionary<int, ParticleSystemSurrogate>();
        private Dictionary<long, Bounds> boundsPerFrame = new Dictionary<long, Bounds>();

        private ParticleDataSnapshot currentFrameData;
        private string currentLevelName;
        private int currentFrameNumber;
        private int isMouseOverGoID = -1;
        private int isMouseOverCompID =-1;
        private int isMouseTapID = -1;

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
                    SetRepresentorsVisualState(_state == SceneVisualState.full);
                    currentFrameNumber = -1;
                    DrawSlice(vBugWindowMediator.currentFrameNumber);
                }
            }
        }

        //--------------- Enable / Disable --------------------



        //--------------- External commands --------------------
        public void Dispose() {
            PruneAllVisualizers();
            currentLevelName = null;
        }


        public void OnDatabaseUpdated() {
            Reset();
            DrawSlice(vBugWindowMediator.currentFrameNumber);
        }


        public void PruneAllVisualizers() {

            ParticleSystemSurrogate[] leftovers = Resources.FindObjectsOfTypeAll<ParticleSystemSurrogate>();
            int i = leftovers.Length;
            while (--i > -1)
                GameObject.DestroyImmediate(leftovers[i].gameObject);

            representors.Clear();
            currentFrameNumber = 0;
            currentFrameData = null;
            Reset();
        }

        private void Reset() {
            boundsPerFrame.Clear();
        }
        //--------------- External commands --------------------






        public void DrawSlice(int frameNumber){
            if (state == SceneVisualState.off || currentFrameNumber == frameNumber)
                return;

            VerticalActivitySlice slice = GetClosestAvailableSlice(frameNumber);
            if (slice == null) {
                Dispose();
                return;
            }
            currentFrameNumber = slice.header.frameNumber;

            if (currentLevelName != slice.header.levelName) {
                PruneAllVisualizers();
                currentLevelName = slice.header.levelName;
            }

            if(slice.particleData == null) {
                return;

            } else {
                ParticleDataSnapshot data = slice.particleData;
                if (data.particleSystems != null && data.particleSystems.Count > 0){
                    currentFrameData = data;
                    int iMax = data.particleSystems.Count;
                    List<int> updatedIDs = new List<int>();
                    List<int> missingIDs = new List<int>();

                    //--------------- Update/Add representors --------------------
                    for (int i = 0; i < iMax; i++) {
                        SGenericParticleSystem sParticles = data.particleSystems[i];
                        if (sParticles == null || sParticles.particles == null || sParticles.particles.positions == null || sParticles.particles.positions.Length == 0)
                            continue;

                        if (!representors.ContainsKey(sParticles.instanceID))
                            representors.Add(sParticles.instanceID, ParticleSystemSurrogate.Create(sParticles.name));

                        ParticleSystemSurrogate representor = representors[sParticles.instanceID];
                        representor.SetParticles(slice.header.frameNumber, sParticles.renderer, sParticles.particles);
                        updatedIDs.Add(sParticles.instanceID);
                    }
                    //--------------- Update/Add representors --------------------
			
                    //--------------- Remove missing --------------------
                    foreach (KeyValuePair<int, ParticleSystemSurrogate> pair in representors) {
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
        }


        public void RenderMouseOverSceneView(SceneView view) {
            if (_state == SceneVisualState.off)
                return;

            DrawSceneGUI(view);
            if (_state == SceneVisualState.full) {
                if(representors.ContainsKey(isMouseOverCompID))
                    representors[isMouseOverCompID].DrawWireframe(colorQuad, colorForward);
            }
        }

        public void RenderAnySceneView(SceneView view) {
            if (_state == SceneVisualState.semi) {
                foreach (KeyValuePair<int, ParticleSystemSurrogate> pair in representors)
                    pair.Value.DrawWireframe(colorQuad, colorForward);
            }
        }


        private void DrawSceneGUI(SceneView view) {
            isMouseOverGoID = -1;
            isMouseOverCompID = -1;
            Rect isMouseOverRect = default(Rect);

            if (currentFrameData != null && currentFrameData.particleSystems != null) {
                //--------------- GUI --------------------

                foreach (SGenericParticleSystem system in currentFrameData.particleSystems) {
                    if (system == null || !system.emit || !system.enabled || system.particles == null || system.particles.positions == null || !representors.ContainsKey(system.instanceID))
                        continue;

                    Bounds bounds = new Bounds(system.position, Vector3.zero);
                    if (system.particles.positions.Length > 0)
                        bounds = GetBoundsFromParticlePositions(system.instanceID, system.particles);

                    if (Vector3.Dot(view.camera.transform.forward, bounds.center - view.camera.transform.position) > 0) {
                        Rect bbRect = SceneVisualsHelper.GetRectFromBounds(bounds);

                        if (isMouseOverCompID != -1 && isMouseOverRect.Contains(bbRect.center))
                            continue;

                        Rect iconRect = VisualResources.DrawIcon(EditorIcon.playbackToggles, 1, bbRect.center, 1f, true);
                        if (iconRect.Contains(Event.current.mousePosition)) {

                            isMouseOverCompID = system.instanceID;
                            isMouseOverGoID = system.goInstanceID;

                            //--------------- Arrow & BB Rect --------------------
                            SceneVisualsHelper.DrawBBArrow(bbRect);
                            //SceneVisualsHelper.DrawBBRect(bbRect);
                            //--------------- Arrow & BB Rect --------------------

                            //--------------- Layout init --------------------
                            float width = 200f;
                            float infoHeight = 70f;
                            float left = bbRect.xMax + 16;
                            float top = bbRect.y + (bbRect.height / 2f) - (infoHeight / 2f);

                            GUI.contentColor = Color.white;
                            EditorHelper.styleLabelSceneVisuals.fontSize = 12;
                            //--------------- Layout init --------------------

                            //--------------- Label --------------------
                            Rect labelRect = new Rect(left, top, width, 20);
                            VisualResources.DrawTexture(labelRect, VisualResources.proConsoleLineDark);
                            GUI.Label(labelRect, !string.IsNullOrEmpty(system.name) ? system.name : "N/A", EditorHelper.styleLabelSceneVisuals);
                            //--------------- Label --------------------

                            //--------------- Particle Info --------------------
                            GUI.contentColor = Color.black;
                            EditorHelper.styleLabelSceneVisuals.fontSize = 9;
                            Rect contentRect = new Rect(left, top + 20, width, infoHeight - 20);
                            isMouseOverRect = contentRect;

                            VisualResources.DrawTexture(contentRect, VisualResources.gray);
                            string infoLabel =
                                (system.isLegacy ? "[LEGACY]\n" : "[SHURIKEN]\n") +
                                "Particle count : " + (system.particles.positions.Length / 3) + "\n" +
                                "Rendermode : " + system.renderer.renderMode + "\n" +
                                "Simulation space : " + (system.isWorldSpace ? "world" : "local") + "\n";

                            GUI.Label(new Rect(left, top + 15, width, infoHeight), infoLabel, EditorHelper.styleLabelSceneVisuals);
                            //--------------- Particle Info --------------------



                            //--------------- Navigation --------------------
                            //if (Event.current.type == EventType.MouseDown) {
                            if (Handles.Button(bounds.center, Quaternion.identity, 10, 10, Handles.CubeCap)) {
                                if (isMouseTapID == isMouseOverCompID) { //already selected
                                    Transform camTrans = view.camera.transform;
                                    view.LookAt(bounds.center, Quaternion.LookRotation(bounds.center - camTrans.position, Vector3.up), Mathf.Max(1, bounds.extents.magnitude * 2f));

                                    if (vBugBaseWindow.CheckIfOpen(typeof(vBugHierarchyWindow))) {
                                        vBugHierarchyWindow hierarchyWin = EditorWindow.GetWindow<vBugHierarchyWindow>();
                                        if (hierarchyWin != null)
                                            hierarchyWin.NavigateToID(isMouseOverGoID);
                                    }
                                }
                                isMouseTapID = isMouseOverCompID;
                            }
                            //--------------- Navigation --------------------
                        }
                    }
                }
                //--------------- GUI --------------------
                GUI.contentColor = Color.white;
            }


            if (isMouseOverCompID == -1)
                isMouseTapID = -1;
        }



        private void SetRepresentorsVisualState(bool state) {
            if (representors != null && representors.Count > 0) {
                foreach (KeyValuePair<int, ParticleSystemSurrogate> pair in representors)
                    pair.Value.isVisible = state;
            }
        }

        private Bounds GetBoundsFromParticlePositions(int instanceID, SGenericParticleArray particles) {
            long key = MathHelper.CombineInts(currentFrameNumber, instanceID);
            if (boundsPerFrame.ContainsKey(key)) {
                return boundsPerFrame[key];
            } else {

                Vector3 min = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
                Vector3 max = new Vector3(float.MinValue, float.MinValue, float.MinValue);

                for (int i = 0; i < particles.positions.Length; i += 3) {

                    float halfSize = particles.sizes[i / 3]/ 2f;
                    Vector3 pos = new Vector3(particles.positions[i], particles.positions[i + 1], particles.positions[i + 2]);
                    min.x = Math.Min(min.x, pos.x - halfSize);
                    min.y = Math.Min(min.y, pos.y - halfSize);
                    min.z = Math.Min(min.z, pos.z - halfSize);

                    max.x = Mathf.Max(max.x, pos.x + halfSize);
                    max.y = Mathf.Max(max.y, pos.y + halfSize);
                    max.z = Mathf.Max(max.z, pos.z + halfSize);
                }

                Bounds bounds = new Bounds((min + max) / 2f, max - min);
                boundsPerFrame.Add(key, bounds);
                return bounds;
            }
        }



        private VerticalActivitySlice GetClosestAvailableSlice(int frameNumber) {
            int iMin = Mathf.Max(VerticalSliceDatabase.minRange, frameNumber - vBugEditorSettings.PlaybackParticleSearchRange);

            for (int i = frameNumber; i > iMin; i--) {
                VerticalActivitySlice slice = VerticalSliceDatabase.GetSlice(i);
                if (slice != null && slice.particleData != null && slice.particleData.particleSystems != null && slice.particleData.particleSystems.Count > 0)
                    return slice;
            }

            return null;
        }

    }
    
}
