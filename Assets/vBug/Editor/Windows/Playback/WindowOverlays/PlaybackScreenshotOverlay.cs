using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Frankfort.VBug.Internal;

namespace Frankfort.VBug.Editor
{
    public class PlaybackScreenshotOverlay : BasePlaybackOverlay
    {

        private int screenCaptureFrameNr;
        private List<GameObject> quads = new List<GameObject>();
        private List<Texture2D> textures = new List<Texture2D>();
        private Vector2 scrollPos;

        private float testRotation;
        private List<string> camNames = new List<string>();
        private List<bool> camStates = new List<bool>();
        private bool isDirty = false;

        public PlaybackScreenshotOverlay ()
        {
            this.doDrawMainContainer = false;
            this.doDrawSubContainer = false;
            this.doDrawQuads = true;
            this.requiresScreenCapture = true;
        }



        public override void Dispose()
        {
            base.Dispose();
            DisposeTextures();
            quads.Clear();
            //Debug.Log("Dispose Screenshot overlay");
        }

        public override void DisposeSceneVisuals()
        {
            base.DisposeSceneVisuals();
            Dispose();
        }

        private void DisposeTextures()
        {
            if (textures != null && textures.Count > 0)
            {
                foreach (Texture2D texture in textures)
                    TexturePool.StoreTexture2D(texture);

                textures.Clear();
            }
        }


        public override void DrawQuads(Transform parent, Vector3 localScale, VerticalActivitySlice slice)
        {
            if(slice == null)
                return;

            if (isDirty || screenCaptureFrameNr != slice.header.frameNumber || quads.Count == 0)
            {
                if (slice.screenCapture != null && slice.screenCapture.camRenders != null && slice.screenCapture.camRenders.Length != 0)
                {
                    isDirty = false;
                    screenCaptureFrameNr = slice.header.frameNumber;
                    DisposeTextures();

                    int iMax = slice.screenCapture.camRenders.Length;
                    if (quads.Count > iMax)
                    {
                        int i = quads.Count;
                        while (--i > iMax - 1)
                        {
                            UnityEngine.Object.DestroyImmediate(quads[i]);
                            quads.RemoveAt(i);
                        }
                    }
                    else if (quads.Count < iMax)
                    {
                        for (int i = quads.Count; i < iMax; i++)
                        {
                            GameObject quad = CreateQuad();
                            Transform qTransform = quad.transform;
                            qTransform.parent = parent;
                            qTransform.localPosition = Vector3.zero;
                            qTransform.localRotation = Quaternion.identity;
                            quads.Add(quad);
                        }
                    }


                    //bool isRepaint = Event.current.type == EventType.repaint;
                    //Debug.Log(Event.current.type);
                    for (int i = 0; i < iMax; i++)
                    {
                        ScreenCaptureSnapshot screenShot = slice.screenCapture;
                        
                        Texture2D texture = TextureHelper.SetBytes(screenShot.camRenders[i], i != 0);
                        textures.Add(texture);

                        Rect rect = screenShot.camRects[i];
                        GameObject quad = quads[i];
                        Vector3 subScale = new Vector3(
                            rect.width / slice.header.screenWidth,
                            rect.height / slice.header.screenHeight, 
                            1f);

                        quad.renderer.sharedMaterial.SetTexture("_MainTex", texture);
                        quad.transform.localScale = Vector3.Scale(localScale, subScale);

                        Vector3 screenPos = new Vector3(
                            -(1f - subScale.x) / 2f * localScale.x,
                            -(1f - subScale.y) / 2f * localScale.y,
                            0f);

                        screenPos.x += rect.x / slice.header.screenWidth * localScale.x;
                        screenPos.y += rect.y / slice.header.screenHeight * localScale.y;
                        quad.transform.localPosition = screenPos;

                        string name = screenShot.camNames[i];
                        if (camNames.Contains(name))
                            quad.SetActive(camStates[camNames.IndexOf(name)]);
                    }
                }
            }
        }


        public override void DrawSettingsLayout(VerticalActivitySlice slice)
        {
            Rect icon = GUILayoutUtility.GetRect(32, 32);
            VisualResources.DrawIcon(EditorIcon.playbackOptions, 3, new Vector2(icon.x, icon.y + 5), 1f, false);

            //--------------- Check cam names --------------------
            if (slice != null && slice.screenCapture != null && slice.screenCapture.camNames != null) {
                foreach (string name in slice.screenCapture.camNames) {
                    if (!camNames.Contains(name)) {
                        camNames.Add(name);
                        camStates.Add(true);
                    }
                }
            } 
            //--------------- Check cam names --------------------

            //--------------- Draw toggles --------------------
            scrollPos = GUILayout.BeginScrollView(scrollPos);
            GUILayout.BeginHorizontal();
            
            if (camNames.Count > 0) {
                for (int i = 0; i < camNames.Count; i++)
                    camStates[i] = GUILayout.Toggle(camStates[i], camNames[i], EditorHelper.styleToggle);

                if (GUI.changed)
                    isDirty = true;
            }

            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.EndScrollView(); 
            //--------------- Draw toggles --------------------
        }

    }
}
