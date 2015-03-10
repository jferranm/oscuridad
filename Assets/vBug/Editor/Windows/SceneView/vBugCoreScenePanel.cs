using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using Frankfort.VBug.Internal;


namespace Frankfort.VBug.Editor {
    public static class vBugCoreScenePanel {

        private static int identifier = int.MaxValue;
        private static bool isInited;
        
        private static MeshSceneVisualizer meshSceneVisualizer;
        private static ParticleSceneVisualizer particleSceneVisualizer;
        private static IBaseSceneVisualizer[] allVisualizers;


        private static bool playbackBusy;
        private static string playbackInfoSmall;
        private static string playbackInfoBig;
        private static SceneView favouriteSceneView;
        private static double playbackStartTimestamp;
        private static double playbackStartFrameTimestamp;
        

        private static GUIStyle transpButton;
        private static string currentToolTip;







        //--------------------------------------- INVOKED BY PRIMARY VBUG-BASE-WINDOW --------------------------------------
        //--------------------------------------- INVOKED BY PRIMARY VBUG-BASE-WINDOW --------------------------------------
        #region INVOKED BY PRIMARY VBUG-BASE-WINDOW


        public static void OnEnable() {
            if (isInited) {
                ResetSceneVisuals();
            } else {
                allVisualizers = new IBaseSceneVisualizer[]{
                    particleSceneVisualizer = new ParticleSceneVisualizer(),
                    meshSceneVisualizer = new MeshSceneVisualizer()
                };

                EditorApplication.update += Update;
                SceneView.onSceneGUIDelegate += OnSceneGUIRender;
            
                transpButton = new GUIStyle();
                isInited = true;
            }
        }

        
        public static void Dispose() {
            if (!isInited)
               return;

            ClearVisualizers(); 
            EditorApplication.update -= Update;
            SceneView.onSceneGUIDelegate -= OnSceneGUIRender;
            allVisualizers = null;
            transpButton = null;
            isInited = false;
        }


        public static void ResetSceneVisuals() {
            ClearVisualizers();
            DrawSlice(vBugWindowMediator.currentFrameNumber);
        }



        public static void NotifyTimelineChange(int frameNumber, int senderID) {
            if(senderID == identifier)
                return;

            DrawSlice(frameNumber);
        }


        public static void NotifyVerticalSliceBundleLoaded() {
            if (allVisualizers != null) {
                foreach (IBaseSceneVisualizer visualizer in allVisualizers)
                    visualizer.OnDatabaseUpdated();
            }
            UpdateTopBarInfo();
            DrawSlice(vBugWindowMediator.currentFrameNumber);
        }


        public static void NotifySessionChange(int startFrame) {
            ClearVisualizers();
            UpdateTopBarInfo();
        }


        private static void ClearVisualizers() {
            if (allVisualizers != null) {
                foreach (IBaseSceneVisualizer visualizer in allVisualizers)
                    visualizer.Dispose();
            }
        }
        #endregion
        //--------------------------------------- INVOKED BY PRIMARY VBUG-BASE-WINDOW --------------------------------------
        //--------------------------------------- INVOKED BY PRIMARY VBUG-BASE-WINDOW --------------------------------------












        //--------------------------------------- PLAYBACK --------------------------------------
        //--------------------------------------- PLAYBACK --------------------------------------
        #region PLAYBACK

        public static void StartPlayback() {
            playbackBusy = true;
            VerticalSliceDatabase.autoGC = false;
            playbackStartTimestamp = vBugEnvironment.GetUnixTimestamp();

            for (int i = vBugWindowMediator.currentFrameNumber; i < VerticalSliceDatabase.maxRange; i++) {
                VerticalActivitySlice slice = VerticalSliceDatabase.GetSlice(i);
                if (slice != null) {
                    playbackStartFrameTimestamp = slice.header.dateTimeStamp;
                    break;
                }
            }
        }

        public static void StopPlayback() {
            playbackBusy = false;
            VerticalSliceDatabase.autoGC = true;
        }



        private static void UpdatePlayback() {
            if (vBugWindowMediator.currentFrameNumber >= VerticalSliceDatabase.maxRange) {
                StopPlayback();
                return;
            }

            double currentTime = vBugEnvironment.GetUnixTimestamp();
            double timeDiff = currentTime - playbackStartTimestamp;

            for (int i = vBugWindowMediator.currentFrameNumber; i <= VerticalSliceDatabase.maxRange; i++) {
                VerticalActivitySlice slice = VerticalSliceDatabase.GetSlice(i);
                /*
                if (slice == null) {
                    StopPlayback();
                    return;
                }
                 */

                if (slice != null && slice.header.dateTimeStamp - playbackStartFrameTimestamp > timeDiff) {
                    vBugWindowMediator.NotifyTimelineChange(i, identifier);
                    DrawSlice(i);
                    break;
                }
            }
        }


        #endregion
        //--------------------------------------- PLAYBACK --------------------------------------
        //--------------------------------------- PLAYBACK --------------------------------------












        //--------------------------------------- UPDATE --------------------------------------
        //--------------------------------------- UPDATE --------------------------------------
        #region UPDATE

        public static void OnSceneGUIRender(SceneView view) {
            Handles.BeginGUI();
            if (CheckPrimarySceneView(view))
                DrawGUI(view);
              
            if (CheckMouseOverView(view)) {
                foreach (IBaseSceneVisualizer visualizer in allVisualizers)
                    visualizer.RenderMouseOverSceneView(view);
            }

            foreach (IBaseSceneVisualizer visualizer in allVisualizers)
                visualizer.RenderAnySceneView(view);

            Handles.EndGUI();
        }


        internal static void Update() {
            if (playbackBusy) {
                UpdatePlayback();
                UpdateTopBarInfo();
            }
        }

        #endregion
        //--------------------------------------- UPDATE --------------------------------------
        //--------------------------------------- UPDATE --------------------------------------














        //--------------------------------------- SCENE VISUALIZERS --------------------------------------
        //--------------------------------------- SCENE VISUALIZERS --------------------------------------
        #region SCENE VISUALIZERS


        private static void DrawSlice(int frameNumber) {
            if (allVisualizers != null) {
                foreach (IBaseSceneVisualizer visualizer in allVisualizers)
                    visualizer.DrawSlice(frameNumber);
            }
            UpdateTopBarInfo();
        }

        #endregion
        //--------------------------------------- SCENE VISUALIZERS --------------------------------------
        //--------------------------------------- SCENE VISUALIZERS --------------------------------------
















        //--------------------------------------- GUI --------------------------------------
        //--------------------------------------- GUI --------------------------------------
        #region GUI

        private static void DrawGUI(SceneView view) {
            if (EditorApplication.isPlayingOrWillChangePlaymode)
                return;

            EventType eventType = Event.current.type;
            currentToolTip = null;

            VisualResources.DrawTexture(new Rect(0, 0, Screen.width, 32), VisualResources.coreNavBG);
            GUILayout.BeginArea(new Rect(0, 0, Screen.width, 80));
            GUILayout.BeginHorizontal();

            //--------------- Logo --------------------
            GUILayout.Space(10);
            Rect logoRect = GUILayoutUtility.GetRect(64, 32);
            VisualResources.DrawIcon(EditorIcon.vBugLogoSolid, new Vector2(logoRect.x, logoRect.y), 1f, false);
            //--------------- Logo --------------------

            DrawVerticalBar();

            //--------------- Window toggles --------------------
            GUILayout.Space(2);
            DrawWindowToggle<vBugRepositoryWindow>(EditorIcon.coreNavPanel2, 1, "Repository window");
            DrawWindowToggle<vBugTimelineWindow>(EditorIcon.coreNavPanel2, 0, "Timeline window");
            DrawWindowToggle<vBugConsoleWindow>(EditorIcon.coreNavPanel2, 3, "Console window");
            DrawWindowToggle<vBugHierarchyWindow>(EditorIcon.coreNavPanel3, 1, "Hierarchy window");
            DrawWindowToggle<vBugPlaybackWindow>(EditorIcon.coreNavPanel2, 2, "Playback window");
            //--------------- Window toggles --------------------

            DrawVerticalBar();

            //--------------- Visualizer toggles --------------------
            if (DrawHighlightedIconButton(EditorIcon.playbackToggles, 3, !VerticalSliceDatabase.isInitialized ? SceneVisualState.off : meshSceneVisualizer.state, VerticalSliceDatabase.isInitialized, true, "Toggle meshes\n[on/semi/off]")) {
                meshSceneVisualizer.state = SceneVisualsHelper.DecrementState(meshSceneVisualizer.state);
                SceneView.RepaintAll();
            }

            if (DrawHighlightedIconButton(EditorIcon.playbackToggles, 1, !VerticalSliceDatabase.isInitialized ? SceneVisualState.off : particleSceneVisualizer.state, VerticalSliceDatabase.isInitialized, true, "Toggle particles\n[on/semi/off]")){
                particleSceneVisualizer.state = SceneVisualsHelper.DecrementState(particleSceneVisualizer.state);
                SceneView.RepaintAll();
            }
            //--------------- Visualizer toggles --------------------
			
            DrawVerticalBar();

            //--------------- Playback buttons --------------------
            if (DrawHighlightedIconButton(EditorIcon.coreNavPanel1, playbackBusy ? 0 : 1, VerticalSliceDatabase.isInitialized ? SceneVisualState.full : SceneVisualState.off, VerticalSliceDatabase.isInitialized, false, "Toggle play/pauze")) {
                if (playbackBusy) {
                    StopPlayback();
                } else {
                    StartPlayback();
                }
            }

            bool inRange = !playbackBusy && vBugWindowMediator.currentFrameNumber > VerticalSliceDatabase.minRange;
            if (DrawHighlightedIconButton(EditorIcon.coreNavPanel1, 2, inRange ? SceneVisualState.full : SceneVisualState.off, inRange, false, "Previous frame")) {
                vBugWindowMediator.NotifyTimelineChange(vBugWindowMediator.currentFrameNumber - 1, identifier);
                DrawSlice(vBugWindowMediator.currentFrameNumber);
            }

            inRange = !playbackBusy && vBugWindowMediator.currentFrameNumber < VerticalSliceDatabase.maxRange;
            if (DrawHighlightedIconButton(EditorIcon.coreNavPanel1, 3, inRange ? SceneVisualState.full : SceneVisualState.off, inRange, false, "Next frame")) {
                vBugWindowMediator.NotifyTimelineChange(vBugWindowMediator.currentFrameNumber + 1, identifier);
                DrawSlice(vBugWindowMediator.currentFrameNumber);
            }
            //--------------- Playback buttons --------------------

            DrawVerticalBar();

            //--------------- Session info --------------------
            GUILayout.Label(playbackInfoBig, EditorHelper.styleLabelCoreBig, GUILayout.MinWidth(75));
            DrawVerticalBar();
            GUILayout.Label(playbackInfoSmall, EditorHelper.styleLabelCoreSmall);
            GUILayout.Space(10);
            GUILayout.FlexibleSpace();
            //--------------- Session info --------------------

            //--------------- Tooltip --------------------
            if (!string.IsNullOrEmpty(currentToolTip) && eventType == EventType.repaint)
                DrawToolTip();
            //--------------- Tooltip --------------------
			
            GUILayout.EndHorizontal();
            GUILayout.EndArea();

            if (eventType == EventType.mouseMove)
                view.Repaint(); //Extra repaint to trigger mouse-over visuals
        }


        private static void DrawVerticalBar() {
            Rect rect = GUILayoutUtility.GetRect(16, 32);
            VisualResources.DrawTexture(new Rect(rect.x + 7, rect.y + 2, 2, 28), VisualResources.black);
        }


        private static void DrawWindowToggle<T>(EditorIcon editorIcon, int subIdx, string toolTip = null) where T : vBugBaseWindow {
            bool isOpen = vBugBaseWindow.CheckIfOpen(typeof(T));
            if (DrawHighlightedIconButton(editorIcon, subIdx, isOpen ? SceneVisualState.off : SceneVisualState.full, !isOpen, false, toolTip)) {
                vBugBaseWindow window = EditorWindow.GetWindow<T>();
                window.Show();
            }
        }


        private static bool DrawHighlightedIconButton(EditorIcon editorIcon, int subIdx, SceneVisualState state, bool isClickable, bool drawBG) {
            return DrawHighlightedIconButton(editorIcon, subIdx, state, isClickable, drawBG, null);
        }

        private static bool DrawHighlightedIconButton(EditorIcon editorIcon, int subIdx, SceneVisualState state, bool isClickable, bool drawBG, string toolTip) {
            bool result = false;
            Rect iconRect = GUILayoutUtility.GetRect(34, 32);
            
            bool doHighlight = false;
            if (iconRect.Contains(Event.current.mousePosition)) {
                currentToolTip = toolTip;
                doHighlight = isClickable;
                if (isClickable && GUI.Button(iconRect, "", transpButton))
                    result = true;
            }

            float c = 1f;
            int bgIdx = 0;
            switch (state) {
                case SceneVisualState.full:  c = 1f;   bgIdx = 1; break;
                case SceneVisualState.semi:  c = 0.8f;  bgIdx = 0; break;
                case SceneVisualState.off:   c = 0.6f;   bgIdx = 3; break;
            }

            if (!doHighlight)
                c -= 0.2f;

            Vector2 pos = new Vector2(iconRect.x, iconRect.y);
            if (drawBG) {
                float cBG = c - 0.15f;
                GUI.color = new Color(cBG, cBG, cBG);
                VisualResources.DrawIcon(EditorIcon.navbarToggleBGs, bgIdx, pos, 1f, false);
            }

            GUI.color = new Color(c, c, c);
            VisualResources.DrawIcon(editorIcon, subIdx, pos, 1f, false);
            GUI.color = Color.white;
            return result;
        }



        private static void UpdateTopBarInfo() {
            VerticalActivitySlice slice = VerticalSliceDatabase.GetSlice(vBugWindowMediator.currentFrameNumber);
            if (slice != null && slice.header != null) {
                playbackInfoSmall =
                    slice.header.userNiceID + "\n" +
                    vBugEnvironment.ToUnixDateTime(slice.header.dateTimeStamp).ToString("ddd d MMM yyyy") + " | " +
                    slice.header.levelName;

                playbackInfoBig = vBugEnvironment.GetNiceFrameNumber(slice.header.frameNumber);
            }
        }



        private static void DrawToolTip() {
            Vector2 mousePos = Event.current.mousePosition;
            Rect toolTipRect = new Rect(mousePos.x - 80, 36, 160, currentToolTip.Contains("\n") ? 38 : 24);
            GUI.color = new Color(1f, 1f, 1f, 0.5f);
            GUI.DrawTexture(toolTipRect, VisualResources.proBGGray);
            GUI.Label(toolTipRect, currentToolTip, EditorHelper.styleLabelCoreTooltip);
            GUI.color = Color.white;
        }

        #endregion
        //--------------------------------------- GUI --------------------------------------
        //--------------------------------------- GUI --------------------------------------
			

















        //--------------------------------------- MISC --------------------------------------
        //--------------------------------------- MISC --------------------------------------
        #region MISC

        private static bool CheckPrimarySceneView(SceneView view) {
            if(favouriteSceneView != null && favouriteSceneView == view)
                return true;

            if (SceneView.sceneViews.Count <= 1) { //This is the only one anyways
                favouriteSceneView = view;
                return true;

            } else { //More then one sceneviews active!
                FindBestView();
                return view == favouriteSceneView;
            }
        }

        private static void FindBestView() {
            int bestScore = 0;
            SceneView bestView = null;
            
            foreach (SceneView view in SceneView.sceneViews) {
                int score = 0;
                if (view == null || view.camera == null)
                    continue;

                if (!view.camera.isOrthoGraphic)
                    score++;

                if (view.renderMode == DrawCameraMode.Textured || view.renderMode == DrawCameraMode.TexturedWire)
                    score++;

                if (bestView != null && view.camera.pixelWidth + view.camera.pixelHeight > bestView.camera.pixelWidth + bestView.camera.pixelHeight)
                    score++;

                if (score > bestScore) {
                    bestScore = score;
                    bestView = view;
                }
            }
            favouriteSceneView = bestView;
        }


        private static bool CheckMouseOverView(SceneView view) {
            if (view == null || EditorWindow.mouseOverWindow == null)
                return false;

            try {
                return view.GetInstanceID() == EditorWindow.mouseOverWindow.GetInstanceID();
            } catch {
                return false;
            }
        }

        #endregion
        //--------------------------------------- MISC --------------------------------------
        //--------------------------------------- MISC --------------------------------------


    }
}
