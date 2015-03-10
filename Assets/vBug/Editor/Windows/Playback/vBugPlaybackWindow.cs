using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using Frankfort.VBug.Editor;
using Frankfort.VBug.Internal;



namespace Frankfort.VBug.Editor {
    public enum PlaybackScreenRotationMode {
        none,
        posMainCam,
        negMainCam,
        posAcceleration,
        negAcceleration,
    }

    public class vBugPlaybackWindow : vBugBaseWindow {
        // Add menu named "My Window" to the Window menu
        
        private const bool debugMode = true;
        private const float optionsBarHeight = 45f;

        private int timelineFrame;

        private Rect dragBarRect;
        private float dragBarCurrentWinHeight;
        private bool dragBarResize = false;
        private Vector2 scrollPos = Vector2.zero;
        private Vector3 localScale = Vector3.one;

        private Rect mainRect;
        private Camera cam;
        private WindowRenderer meshManager;
        private Transform quadParent;

        private int rotOptionIdx;
        private bool rotationZOnly = true;
        private PlaybackScreenRotationMode rotationMode;

        private BasePlaybackOverlay[] overlays;
        private string[] rotationsOptions = new string[] {
            "none",
            "[+] MainCam",
            "[-] MainCam",
            "[+] Acceleration",
            "[-] Acceleration"
        };










        //--------------------------------------- INIT & DISPOSE --------------------------------------
        //--------------------------------------- INIT & DISPOSE --------------------------------------
        #region INIT & DISPOSE

        protected override void OnEnable() {
            base.OnEnable();
            this.title = "vBug Playback";

            ReinitSceneVisuals();

            string dragKey = this.GetType().Name + "_dragBarY";
            if (EditorPrefs.HasKey(dragKey)) {
                dragBarRect.y = EditorPrefs.GetFloat(dragKey);
            } else {
                dragBarRect.y = this.position.height * 0.8f;
            }

            if (VerticalSliceDatabase.isInitialized)
                UpdateOverlays(vBugWindowMediator.currentFrameNumber);
        }


        protected override void OnDisable() {
            base.OnDisable();
            EditorPrefs.SetFloat(this.GetType().Name + "_dragBarY", dragBarRect.y);
        }


        protected override void ReinitSceneVisuals() {
            base.ReinitSceneVisuals();

            DestroySceneElements();
            overlays = new BasePlaybackOverlay[] { 
                new PlaybackScreenshotOverlay(),
                new PlaybackMouseOverlay(),
                new PlaybackTouchOverlay(),
                new PlaybackKeyboardOverlay()
                //AxisInputProvider
            };
        }


        protected override void DisposeAll() {
            base.DisposeAll();
            DestroySceneElements();
        }

        protected override void DisposeSceneVisuals() {
            base.DisposeSceneVisuals();
            DestroySceneElements();
        }

        private void DestroySceneElements() {
            if (overlays != null) {
                foreach (BasePlaybackOverlay overlay in overlays)
                    overlay.Dispose();
            }

            if (cam != null)
                UnityEngine.Object.DestroyImmediate(cam.gameObject);


            if (quadParent != null)
                UnityEngine.Object.DestroyImmediate(quadParent.gameObject);

            overlays = null;
            cam = null;
            meshManager = null;
            quadParent = null;
        }
        #endregion
        //--------------------------------------- INIT & DISPOSE --------------------------------------
        //--------------------------------------- INIT & DISPOSE --------------------------------------







        //--------------- Notifications --------------------
        public override void NotifySessionChange(int startFrame, int senderID) {
            base.NotifySessionChange(startFrame, senderID);
            ReinitSceneVisuals();
            UpdateOverlays(startFrame);
            Repaint();
        }

        public override void NotifyTimelineChange(int newFrame, int senderID) {
            base.NotifyTimelineChange(newFrame, senderID);
            UpdateOverlays(newFrame);
            Repaint();
        }

        public void UpdateOverlays(int frameNumber) {
            timelineFrame = frameNumber;
            if (overlays == null)
                ReinitSceneVisuals();

            foreach (BasePlaybackOverlay overlay in overlays)
                overlay.NotifyTimelineChange(frameNumber);
        }

        //--------------- Notifications --------------------







        //--------------------------------------- MESH RENDERING --------------------------------------
        //--------------------------------------- MESH RENDERING --------------------------------------
        #region MESH RENDERING

        private void RedrawQuads() {
            //--------------- Closest VerticalSlice --------------------
            VerticalActivitySlice currentFrameSlice = VerticalSliceDatabase.GetSlice(timelineFrame);
            
            VerticalActivitySlice screenCaptureSlice = currentFrameSlice;
            if (screenCaptureSlice == null || screenCaptureSlice.screenCapture == null || screenCaptureSlice.screenCapture.camRenders == null || screenCaptureSlice.screenCapture.camRenders.Length == 0)
                screenCaptureSlice = GetClosestAvailableSlice(timelineFrame, true);
            //--------------- Closest VerticalSlice --------------------

            if (quadParent == null) 
                quadParent = CreateQuadParent();

            if (screenCaptureSlice != null) {
                //--------------- Localscale --------------------
                float maxSize = Mathf.Max(screenCaptureSlice.header.screenWidth, screenCaptureSlice.header.screenHeight);
                localScale = new Vector3(
                    screenCaptureSlice.header.screenWidth / maxSize,
                    screenCaptureSlice.header.screenHeight / maxSize,
                    1f);
                //--------------- Localscale --------------------

                //--------------- Quad parent --------------------
                UpdateCameraSettings(quadParent, screenCaptureSlice, localScale);
                //--------------- Quad parent --------------------
            }

            //--------------- Prepair render --------------------
            foreach (BasePlaybackOverlay overlay in overlays) {
                overlay.InitCurrentFrame();
                if (overlay.doDrawQuads)
                    overlay.DrawQuads(quadParent, localScale, overlay.requiresScreenCapture ? screenCaptureSlice : currentFrameSlice);
            }

            Frankfort.VBug.Internal.GameObjectUtility.SetHideFlagsRecursively(quadParent.gameObject);
            //--------------- Prepair render --------------------


            //--------------- Render --------------------
            Camera current = Camera.current;
            RenderCam();
            Camera.SetupCurrent(current);
            //--------------- Render --------------------
        }


        private Transform CreateQuadParent() {
            GameObject go = new GameObject("vBugQuadParent");
            Transform result = go.transform;
            Frankfort.VBug.Internal.GameObjectUtility.SetHideFlagsRecursively(go);
            return result;
        }


        private void UpdateCameraSettings(Transform quadParent, VerticalActivitySlice slice, Vector2 localScale) {
            if (slice == null || slice.screenCapture == null || cam == null)
                return;

            ScreenCaptureSnapshot sc = slice.screenCapture;
            Vector3 rotation = Vector3.zero;
            cam.fieldOfView = 70f;
            switch (rotationMode) {
                case PlaybackScreenRotationMode.none:
                    cam.isOrthoGraphic = true;
                    cam.orthographicSize = (localScale.y / 2f) + 0.01f; //(Mathf.Min(localScale.x, localScale.y) / 2f) + 0.01f;
                    break;
                case PlaybackScreenRotationMode.posMainCam:
                    cam.isOrthoGraphic = false;
                    rotation = sc.mainCamWorldRotation;
                    break;

                case PlaybackScreenRotationMode.negMainCam:
                    cam.isOrthoGraphic = false;
                    rotation = -(Vector3)sc.mainCamWorldRotation;
                    break;

                case PlaybackScreenRotationMode.posAcceleration:
                    cam.isOrthoGraphic = false;
                    rotation = sc.inputAcceleration;
                    break;

                case PlaybackScreenRotationMode.negAcceleration:
                    cam.isOrthoGraphic = false;
                    rotation = -(Vector3)sc.inputAcceleration;
                    break;
            }

            if (rotationZOnly)
                rotation.x = rotation.y = 0f;

            quadParent.eulerAngles = rotation;
        }




        private void RenderCam() {
            try {
                if (cam == null) {
                    GameObject go = new GameObject("vBugPlaybackCamera");
                    go.transform.position = new Vector3(0f, 0f, -1f);
                    cam = go.AddComponent<Camera>();
                    cam.enabled = false;
                    cam.clearFlags = CameraClearFlags.SolidColor;
                    cam.backgroundColor = Color.clear;
                    cam.fieldOfView = 30f;
                    cam.nearClipPlane = 0.25f;
                    cam.farClipPlane = 2.5f;
                    
                    cam.cullingMask = 1 << vBugEditorSettings.PlaybackRenderLayer;
                    meshManager = go.AddComponent<WindowRenderer>();
                    Frankfort.VBug.Internal.GameObjectUtility.SetHideFlagsRecursively(go);
                }

                int newWdith = (int)mainRect.width;
                int newHeight = (int)mainRect.height;

                if (newWdith > 0 && newHeight > 0 && meshManager != null) {
                    meshManager.SetObjectsToRender(mainRect, new GameObject[] { quadParent.gameObject });//, gimballParent.gameObject});
                    Handles.DrawCamera(mainRect, cam, DrawCameraMode.Normal);
                }
            } catch (Exception e) {
                if (vBugEditorSettings.DebugMode)
                    Debug.LogError(e.Message + e.StackTrace);
            }
        }
        #endregion
        //--------------------------------------- MESH RENDERING --------------------------------------
        //--------------------------------------- MESH RENDERING --------------------------------------














        //--------------------------------------- GUI --------------------------------------
        //--------------------------------------- GUI --------------------------------------
        #region GUI


        void OnGUI() {
            try {
                if (EditorApplication.isPlayingOrWillChangePlaymode)
                    return;

                VisualResources.DrawWindowBGWaterMark(this.position);
                DrawGUI();
            } catch (Exception e) {
                if (vBugEditorSettings.DebugMode)
                    Debug.LogError(e.Message + e.StackTrace);
            }
        }



        private void DrawGUI() {
            if (!VerticalSliceDatabase.isInitialized) {
                EditorHelper.DrawNotInitializedLabel(this.position);

            } else if (!EditorApplication.isPlayingOrWillChangePlaymode) {
                VerticalActivitySlice slice = GetClosestAvailableSlice(timelineFrame,false);

                if (slice == null || overlays == null) {
                    EditorHelper.DrawNA(new Rect(0, 0, this.position.width, this.position.height));
                } else {
                    EditorHelper.DrawDragBar(this, ref dragBarRect, ref dragBarResize, ref dragBarCurrentWinHeight);

                    //--------------- Main Rect --------------------
                    float maxWidth = slice.header.screenWidth;
                    float maxHeight = slice.header.screenHeight;
                    if (rotationMode != PlaybackScreenRotationMode.none)
                        maxWidth = maxHeight = Mathf.Max(maxWidth, maxHeight);

                    float maxMainRectHeight = dragBarRect.y - optionsBarHeight;
                    if (this.position.width < maxWidth || maxMainRectHeight < maxHeight) {
                        float rato = Mathf.Max(maxWidth / this.position.width, maxHeight / maxMainRectHeight);
                        maxWidth /= rato;
                        maxHeight /= rato;
                    }

                    mainRect = new Rect(
                        (this.position.width / 2f) - (maxWidth / 2f),
                        0, 
                        maxWidth,
                        maxHeight);
                    //--------------- Main Rect --------------------

                    //--------------- Overlays --------------------
                    foreach (BasePlaybackOverlay overlay in overlays) {
                        if (overlay.doDrawMainContainer)
                            overlay.DrawMainContainer(mainRect, timelineFrame, this);
                    }
                    //--------------- Overlays --------------------

                    //--------------- Bottom Rect --------------------
                    Rect bottomRect = new Rect(0, dragBarRect.yMax, this.position.width, this.position.height - dragBarRect.yMax);
                    GUILayout.BeginArea(bottomRect);
                    scrollPos = GUILayout.BeginScrollView(scrollPos);

                    foreach (BasePlaybackOverlay overlay in overlays) {
                        if (overlay.doDrawSubContainer)
                            overlay.DrawSubContainer(timelineFrame, this);
                    }

                    GUILayout.EndScrollView();
                    GUILayout.EndArea();
                    //--------------- Bottom Rect --------------------

                    //--------------- Options bar --------------------
                    DrawSettingsOptions(maxMainRectHeight, slice); 
                    //--------------- Options bar --------------------

                    //--------------- Viewport render --------------------
                    if (Event.current.type == EventType.repaint)
                        RedrawQuads();
                    //--------------- Viewport render --------------------
                }
            }
        }


        private VerticalActivitySlice GetClosestAvailableSlice(int frameNumber, bool requiresScreenCapture) {
            if (frameNumber > VerticalSliceDatabase.maxRange || frameNumber < VerticalSliceDatabase.minRange)
                return null;

            VerticalActivitySlice result = null;
            int iMin = Mathf.Max(VerticalSliceDatabase.minRange, frameNumber - vBugEditorSettings.PlaybackOverlaySearchRange);

            for (int i = frameNumber; i > iMin; i--) {
                result = VerticalSliceDatabase.GetSlice(i);
                if (result == null)
                    continue;
                
                if (requiresScreenCapture) {
                    if (result.screenCapture != null && result.screenCapture.camRenders != null && result.screenCapture.camRenders.Length > 0)
                        return result;
                } else {
                    return result;
                }
            }

            return null;
        }



        private void DrawSettingsOptions(float startY, VerticalActivitySlice slice) {
            VisualResources.DrawTexture(new Rect(0, startY, this.position.width, optionsBarHeight), VisualResources.proConsoleLineDark);
            Rect layoutRect = new Rect(0f, startY, this.position.width, dragBarRect.y - startY);
            GUILayout.BeginArea(layoutRect);
            GUILayout.BeginHorizontal();

            //--------------- Rotations options --------------------
            //GUILayout.Space(45);
            Rect icon = GUILayoutUtility.GetRect(40, optionsBarHeight);
            VisualResources.DrawIcon(EditorIcon.playbackOptions, 1, new Vector2(icon.x + 10, icon.y + 5), 1f, false);

            GUILayout.BeginVertical(GUILayout.Width(100));
            GUILayout.Space(5);

            rotOptionIdx = EditorGUILayout.Popup(rotOptionIdx, rotationsOptions, EditorHelper.stylePopup);
            rotationMode = (PlaybackScreenRotationMode)rotOptionIdx;

            GUI.enabled = rotationMode != PlaybackScreenRotationMode.none;
            rotationZOnly = GUILayout.Toggle(rotationZOnly, "'Z' axis only", EditorHelper.styleToggle);
            GUI.enabled = true;

            GUILayout.EndVertical();
            //--------------- Rotations options --------------------

            //--------------- VertBar --------------------
            GUILayout.Space(10);
            Rect vertBar = GUILayoutUtility.GetRect(5, optionsBarHeight);
            VisualResources.DrawTexture(new Rect(vertBar.x, vertBar.y, 5, optionsBarHeight - 10), VisualResources.proConsoleRowLight);
            GUILayout.Space(10);
            //--------------- VertBar --------------------

            //--------------- Overlay options --------------------
            foreach (BasePlaybackOverlay overlay in overlays) {
                overlay.DrawSettingsLayout(slice);
                //GUILayout.Space(20);
            }
            //--------------- Overlay options --------------------

            GUILayout.EndHorizontal();
            GUILayout.EndArea();
        }


        private bool DrawToggle(bool toggle, int iconIdx) {
            Rect rect = GUILayoutUtility.GetRect(32, 32);
            Vector2 pos = new Vector2(rect.x, rect.y + 5);
            GUI.color = toggle ? Color.green : Color.white;
            VisualResources.DrawIcon(EditorIcon.playbackToggles, iconIdx, pos, 1f, false);
            GUI.color = Color.white;

            if (Event.current.type == EventType.MouseUp && rect.Contains(Event.current.mousePosition))
                return !toggle;

            return toggle;
        }



        #endregion
        //--------------------------------------- GUI --------------------------------------
        //--------------------------------------- GUI --------------------------------------


    }
}