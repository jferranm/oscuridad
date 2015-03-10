using System;
using UnityEngine;
using Frankfort.VBug.Internal;
using System.Collections.Generic;
using UnityEditor;

namespace Frankfort.VBug.Editor {
    public class BaseGraphContainer {
        public enum VisualStyle {
            normal,
            dark,
            hidden
        }

        public enum DrawStyle {
            stack,
            addative
        }


        public DrawStyle drawStyle;

        public Color32[] colors;
        public VisualStyle[] visualStyles { get; private set; }
        public string[] names;
        public bool[] availability;

        public float highestVisibleValue = -1;
        protected float absolutMaxValue = float.MaxValue;

        private Texture2D canvas;
        private Color32[] buffer;
        private Dictionary<int, float[]> storedValues;

        private bool isDisposed;
        private bool canvasDirty = false;
        private int currentScrollPos = -1;

        protected bool requiresMemoryCapture;
        protected string notAvailableMessage;

        protected int width;
        protected int height;
        protected Rect graphRect;



        public BaseGraphContainer(Color32[] colors, string[] names) {
            this.colors = colors;
            this.names = names;
            this.storedValues = new Dictionary<int, float[]>();
            this.visualStyles = new VisualStyle[colors.Length];
            this.availability = new bool[names.Length];
        }


        public void Dispose() {
            if (isDisposed)
                return;

            if (canvas != null)
                UnityEngine.Object.DestroyImmediate(canvas);

            canvas = null;
            buffer = null;
            storedValues = null;
            visualStyles = null;
            colors = null;
            isDisposed = true;
        }


        public void SetDirty() {
            canvasDirty = true;
        }


        public void SetStyle(int valueIdx, VisualStyle type) {
            if (valueIdx < 0 || valueIdx >= visualStyles.Length)
                return;

            VisualStyle current = visualStyles[valueIdx];
            if (current == type)
                return;

            if ((current == VisualStyle.normal && type == VisualStyle.hidden) || (current == VisualStyle.hidden && type == VisualStyle.normal))
                highestVisibleValue = -1;

            canvasDirty = true;
            visualStyles[valueIdx] = type;
        }


        public bool DrawGraph(Rect screenArea, int scrollPos, float scaleX, bool forceUpdate) {
            bool result = false;

            if (!VerticalSliceDatabase.isInitialized) {
                EditorHelper.DrawNA(graphRect, "Please select a valid vBug session.");
            } else {

                float maxScale = Mathf.Max(1f, scaleX);
                float maxWidth = (VerticalSliceDatabase.maxRange - VerticalSliceDatabase.minRange) * scaleX;
                this.width = (int)Mathf.Min((screenArea.width / maxScale), maxWidth);
                this.height = (int)screenArea.height;

                if (canvas == null || canvas.width != width || canvas.height != height) {
                    if (canvas != null)
                        UnityEngine.Object.DestroyImmediate(canvas);

                    canvas = new Texture2D(width, height, TextureFormat.RGBA32, false, false);
                    canvas.filterMode = FilterMode.Point;
                    canvas.wrapMode = TextureWrapMode.Clamp;
                    buffer = new Color32[width * height];
                    canvasDirty = true;
                }

                if (CheckAniValueVisible()) {
                    if (highestVisibleValue == -1 || currentScrollPos != scrollPos)
                        CalcMaxValue(scrollPos, scrollPos);

                    if (highestVisibleValue != -1 && (forceUpdate || canvasDirty || currentScrollPos != scrollPos)) {
                        currentScrollPos = scrollPos;
                        result = RebuildCanvasBuffer(scrollPos, width, scaleX);
                        if (result) {
                            canvas.SetPixels32(buffer);
                            canvas.Apply(false, false);
                            canvasDirty = false;
                        }
                    } else {
                        result = highestVisibleValue != -1;
                    }

                    graphRect = new Rect(screenArea.x, screenArea.y, canvas.width * maxScale, canvas.height);
                    if (result) {
                        VisualResources.DrawTexture(graphRect, canvas);
                    } else {
                        EditorHelper.DrawNA(graphRect, "Data not available.\n" + notAvailableMessage);
                    }
                }
            }
            return result;
        }


        private bool CheckAniValueVisible() {
            if (visualStyles != null && visualStyles.Length > 0) {
                foreach (VisualStyle style in visualStyles) {
                    if (style != VisualStyle.hidden)
                        return true;
                }
            }
            return false;
        }



        private void CalcMaxValue(int scrollPos, int newScroll) {
            int minRange = VerticalSliceDatabase.minRange;
            int maxRange = VerticalSliceDatabase.maxRange;

            int startIdx = Mathf.Max(0, minRange);
            int endIdx = Mathf.Min(maxRange, startIdx + width);

            if (highestVisibleValue == -1) { // First time
                startIdx += scrollPos;
                endIdx += scrollPos;
            } else {
                startIdx = scrollPos > newScroll ? newScroll : scrollPos;
                endIdx = scrollPos > newScroll ? scrollPos : newScroll;
            }


            for (int i = startIdx; i < endIdx; i++) {
                float[] values = GetCachedFrameData(i, minRange, maxRange, false);
                if (values != null && values.Length > 1) {
                    if (drawStyle == DrawStyle.stack) {
                        float total = 0;
                        for (int j = 0; j < values.Length; j++) {
                            if (visualStyles[j] == VisualStyle.hidden)
                                continue;

                            float value = values[j];
                            if (float.IsNaN(value) || float.IsInfinity(value) || value <= 0 || value > absolutMaxValue)
                                continue;

                            total += value;
                        }
                        highestVisibleValue = Mathf.Max(highestVisibleValue, total);

                    } else if (drawStyle == DrawStyle.addative) {

                        for (int j = 0; j < values.Length; j++) {
                            if (visualStyles[j] == VisualStyle.hidden)
                                continue;

                            float value = values[j];
                            if (float.IsNaN(value) || float.IsInfinity(value) || value <= 0 || value > absolutMaxValue)
                                continue;

                            highestVisibleValue = Mathf.Max(highestVisibleValue, values[j]);
                        }
                    }
                }
            }
            //Debug.Log("Max value: " + highestVisibleValue);
        }













        //--------------------------------------- Generate graph texture canvas --------------------------------------
        //--------------------------------------- Generate graph texture canvas --------------------------------------
        #region Generate graph texture canvas


        private bool RebuildCanvasBuffer(int scrollPos, int width, float scaleX) {
            bool succes = false;
            int minRange = VerticalSliceDatabase.minRange;
            int maxRange = VerticalSliceDatabase.maxRange;

            int iMax = Mathf.Min(width, (int)(maxRange - minRange));
            int pMax = buffer.Length;
            float totalScale = height / highestVisibleValue;
            float iStepping = Mathf.Max(1f, 1f / scaleX);
            float iScaled = minRange;
            int iOld = 0;

            Array.Clear(buffer, 0, buffer.Length);
            Color32 emptyColor = Color.clear;

            //--------------- Build graph --------------------
            for (int iDest = 0; iDest < iMax; iDest++) {
                iScaled += iStepping;
                int iSource = (int)Mathf.Floor(iScaled);
                if (iOld == iSource)
                    continue;

                iOld = iSource;
                float[] values = GetCachedFrameData(iSource + scrollPos, minRange, maxRange, true);

                int pStart = iDest;
                int p = pStart;

                if (values == null || values.Length == 0) {
                    for (int y = 0; y < height && p < pMax; y++) {
                        buffer[p] = emptyColor;
                        p += width;
                    }
                } else {

                    for (int j = 0; j < values.Length; j++) {
                        float value = values[j];
                        if (float.IsNaN(value) || float.IsInfinity(value) || value <= 0 || value > absolutMaxValue)
                            continue;

                        Color32 color = colors[j];
                        VisualStyle type = visualStyles[j];
                        if (type == VisualStyle.hidden) {
                            continue;
                        } else if (type == VisualStyle.dark) {
                            color.r /= 3;
                            color.g /= 3;
                            color.b /= 3;
                        }

                        int yMax = (int)Mathf.Clamp(value * totalScale, 0f, height);
                        if (drawStyle == DrawStyle.stack) {
                            succes = true;
                            for (int y = 0; y < yMax && p < pMax; y++) {
                                buffer[p] = color;
                                p += width;
                            }
                        } else if (drawStyle == DrawStyle.addative) {
                            succes = true;
                            p = pStart;
                            byte bMax = 255;
                            for (int y = 0; y < yMax && p < pMax; y++) {
                                buffer[p].r += color.r;
                                buffer[p].g += color.g;
                                buffer[p].b += color.b;
                                buffer[p].a = bMax;
                                p += width;
                            }
                        }
                    }
                }
            }
            //--------------- Build graph --------------------
            return succes;
        }


        private float[] GetCachedFrameData(int frameIdx, int minRange, int maxRange, bool searchClosestAvailable) {
            float[] result = null;
            if (frameIdx >= minRange && frameIdx < maxRange) {
                if (storedValues.ContainsKey(frameIdx)) {
                    result = storedValues[frameIdx];
                } else {
                    result = CollectFrameData(frameIdx, searchClosestAvailable);
                    if (result != null) {
                        storedValues.Add(frameIdx, result);

                        for (int i = 0; i < result.Length; i++) {
                            if (result[i] != -1f)
                                availability[i] = true;
                        }
                    }
                }
            }
            return result;
        }



        private float[] CollectFrameData(int framenumber, bool searchClosestAvailable) {
            if (searchClosestAvailable) {
                VerticalActivitySlice slice = GetClosestAvailableFrame(framenumber);
                return slice != null ? GetSliceGraphData(slice) : null;
            } else {
                VerticalActivitySlice slice = VerticalSliceDatabase.GetSlice(framenumber);
                if (slice != null && slice.systemInfo != null) {
                    if (requiresMemoryCapture) {
                        if (slice.systemInfo.memoryProfiled)
                            return GetSliceGraphData(slice);

                    } else {
                        return GetSliceGraphData(slice);
                    }
                }
                return null;
            }
        }


        private VerticalActivitySlice GetClosestAvailableFrame(int framenumber) {
            VerticalActivitySlice slice = null;
            int searchRange = requiresMemoryCapture ? vBugEditorSettings.PlaybackSystemMemorySearchRange : vBugEditorSettings.PlaybackSystemDefaultSearchRange;
            int iMin = Mathf.Max(VerticalSliceDatabase.minRange, framenumber - searchRange);

            for (int i = framenumber; i >= iMin; i--) {
                slice = VerticalSliceDatabase.GetSlice(i);
                if (slice != null && slice.systemInfo != null) {
                    if (requiresMemoryCapture) {
                        if (slice.systemInfo.memoryProfiled)
                            return slice;

                    } else {
                        return slice;
                    }
                }
            }
            return null;
        }

        #endregion
        //--------------------------------------- Generate graph texture canvas --------------------------------------
        //--------------------------------------- Generate graph texture canvas --------------------------------------












        //--------------------------------------- Mouse hover --------------------------------------
        //--------------------------------------- Mouse hover --------------------------------------
        #region Mouse hover


        public void DrawMouseHover(int scrollPos, float scaleX, bool showIndicator) {
            if (Event.current == null)
                return;

            Vector2 mousePos = Event.current.mousePosition;
            if (graphRect.Contains(mousePos)) {
                //--------------- Vertical indicator --------------------
                if (showIndicator) {
                    int snapPosX = (int)(mousePos.x - (mousePos.x % scaleX));

                    GUI.color = new Color(1f, 1f, 1f, 0.5f);
                    VisualResources.DrawTexture(new Rect(snapPosX, graphRect.y, scaleX, graphRect.height), VisualResources.white);
                    VisualResources.DrawIcon(EditorIcon.graphArrows, 0, new Vector2(snapPosX + (scaleX / 2), graphRect.y - 16), 1f, true);
                    VisualResources.DrawIcon(EditorIcon.graphArrows, 1, new Vector2(snapPosX + (scaleX / 2), graphRect.y + graphRect.height + 16), 1f, true);
                    GUI.color = Color.white;
                }
                //--------------- Vertical indicator --------------------


                //--------------- Floating data --------------------
                int hoverFrame = VerticalSliceDatabase.minRange + scrollPos + (int)((mousePos.x - graphRect.x) / scaleX);
                string label = vBugEnvironment.GetNiceFrameNumber(hoverFrame);
                string content = string.Empty;

                VerticalActivitySlice slice = GetClosestAvailableFrame(hoverFrame);
                if (slice == null || slice.systemInfo == null) {
                    content += "\n\nN.A.";
                } else {
                    content += CollectHoverData(slice);
                }

                Vector2 labelSize = EditorHelper.styleLabelLightGray10.CalcSize(new GUIContent(label));
                Vector2 contentSize = EditorHelper.styleLabelLightGray10.CalcSize(new GUIContent(content));
                float maxWidth = Mathf.Max(contentSize.x, labelSize.x);
                float yPos = graphRect.y + 20;

                Rect labelBG = new Rect(mousePos.x - (maxWidth / 2), yPos - labelSize.y - 5, maxWidth, contentSize.y);//new Rect(mousePos.x - (maxWidth / 2), yPos - labelSize.y + 15, maxWidth, labelSize.y);
                VisualResources.DrawTexture(labelBG, VisualResources.proConsoleRowLight);

                TextAnchor bAlign = EditorHelper.styleLabel.alignment;
                EditorHelper.styleLabel.alignment = TextAnchor.UpperCenter;
                GUI.Label(labelBG, label, EditorHelper.styleLabel);
                EditorHelper.styleLabel.alignment = bAlign;

                Rect contentBG = new Rect(mousePos.x - (maxWidth / 2), yPos, maxWidth, contentSize.y);
                GUI.color = new Color(1f, 1f, 1f, 0.9f);
                VisualResources.DrawTexture(contentBG, VisualResources.proConsoleRowDark);
                GUI.Label(contentBG, content, EditorHelper.styleLabelLightGray10);
                GUI.color = Color.white;
                //--------------- Floating data --------------------
            }
        }

        #endregion
        //--------------------------------------- Mouse hover --------------------------------------
        //--------------------------------------- Mouse hover --------------------------------------














        //--------------------------------------- Custom implementation --------------------------------------
        //--------------------------------------- Custom implementation --------------------------------------
        #region Custom implementation


        protected virtual float[] GetSliceGraphData(VerticalActivitySlice slice) {
            throw new NotImplementedException();
        }

        protected virtual string CollectHoverData(VerticalActivitySlice slice) {
            throw new NotImplementedException();
        }

        #endregion
        //--------------------------------------- Custom implementation --------------------------------------
        //--------------------------------------- Custom implementation --------------------------------------


    }

}
