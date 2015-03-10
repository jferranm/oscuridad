using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Frankfort.VBug.Internal;

namespace Frankfort.VBug.Editor
{

    public class vBugTimelineWindow : vBugBaseWindow
    {

        private const int scrollHeight = 15;
        private int scrollPos;
        private float graphScaleX = 5f;
        
        private MemoryInfoGraph memoryInfoGraph;
        private FpsInfoGraph fpsInfoGraph;
        private int memoryHighlightedIdx = -1;
        private int fpsHighlightedIdx = -1;

        private float navpanelWidth = 125;
        private float graphX = 175;
        private float graphY = 24;
        private double downTimeStamp = -1d;

        private double updateTimeStamp = -1;
        private bool forceUpdate;

        private GUIStyle myToggleStyle;
        private Dictionary<int, int> frameHasConsoleLog = new Dictionary<int, int>();


        protected override void OnEnable()
        {
            base.OnEnable();
            this.title = "vBug Timeline";

            repaintOnFocus = true;
            InitSystemInfoGraph();
            InitGuiStyles();
        }

        protected override void Update(){
            base.Update();
            //--------------- Get correct DeltaTime --------------------
            double time = vBugEnvironment.GetUnixTimestamp();
            if (updateTimeStamp == -1)
                updateTimeStamp = time;

            updateTimeStamp = time; 
            //--------------- Get correct DeltaTime --------------------
        }


        private void InitSystemInfoGraph()
        {
            if (VerticalSliceDatabase.isInitialized && fpsInfoGraph == null)
            {
                memoryInfoGraph = new MemoryInfoGraph();
                fpsInfoGraph = new FpsInfoGraph();
            }
        }

        private void InitGuiStyles() {
            if (myToggleStyle == null) {
                GUISkin editorSkin = EditorGUIUtility.GetBuiltinSkin(EditorSkin.Scene);
                myToggleStyle = new GUIStyle(editorSkin.toggle);
                myToggleStyle.fontSize = 16;
                myToggleStyle.fontStyle = FontStyle.Bold;
                myToggleStyle.alignment = TextAnchor.MiddleRight;
            }
        }

        //--------------- Playback --------------------
        public void GotoFrame(int frameNumber) {
                vBugWindowMediator.NotifyTimelineChange(frameNumber, this.GetInstanceID());
            SceneView.RepaintAll();
        }
        //--------------- Playback --------------------


        //--------------- Playmode --------------------
        protected override void ReinitSceneVisuals()
        {
            base.ReinitSceneVisuals();
            PruneCache();
        }

        protected override void DisposeSceneVisuals()
        {
            base.DisposeSceneVisuals();
            DisposeAll();
        }
        //--------------- Playmode --------------------
			


        //--------------- Notifications --------------------
        public override void NotifySessionChange(int startFrame, int senderID) {
            base.NotifySessionChange(startFrame, senderID);
            if (senderID == this.GetInstanceID())
                return;

            PruneCache();  
            FocusSelectedFrame();
            InitSystemInfoGraph();
        }

        public override void NotifyTimelineChange(int newFrame, int senderID) {
            base.NotifyTimelineChange(newFrame, senderID);
            if (senderID == this.GetInstanceID())
                return;

            FocusSelectedFrame();
            Repaint();
        }

        public override void NotifyVerticalSliceBundleLoaded(VerticalActivitySlice[] slices) {
            base.NotifyVerticalSliceBundleLoaded(slices);

            if (fpsInfoGraph != null)
                fpsInfoGraph.SetDirty();
            if (memoryInfoGraph != null)
                memoryInfoGraph.SetDirty();
        }

        private void PruneCache()
        {
            if (memoryInfoGraph != null)
                memoryInfoGraph.Dispose();

            if (fpsInfoGraph != null)
                fpsInfoGraph.Dispose();

            memoryInfoGraph = new MemoryInfoGraph();
            fpsInfoGraph = new FpsInfoGraph();
            frameHasConsoleLog.Clear();
        }
        //--------------- Notifications --------------------











		//--------------------------------------- DRAW & UPDATE GUI --------------------------------------
		//--------------------------------------- DRAW & UPDATE GUI --------------------------------------
		#region DRAW & UPDATE GUI

        void OnGUI() {
            try {
                if (EditorApplication.isPlayingOrWillChangePlaymode)
                    return;

                VisualResources.DrawWindowBGWaterMark(this.position);
                UpdateKeyInput();
                DrawTimeLine();
            } catch (Exception e) {
                if (vBugEditorSettings.DebugMode)
                    Debug.LogError(e.Message + e.StackTrace);
            }
        }




        private void DrawTimeLine()
        {

            if (!VerticalSliceDatabase.isInitialized) {
                EditorHelper.DrawNotInitializedLabel(this.position);
            } else {
                EventType eventType = Event.current.type;
                float fpsHeight = 64;
                float memoryHeight = 96;
                float graphWidth = this.position.width - graphX;
                float currentY = 0;
                
                //--------------- SCROLLBAR --------------------
                float scrollBarY = 78;
                scrollBarY += fpsHeight;
                scrollBarY += memoryHeight;

                int minRange = VerticalSliceDatabase.minRange;
                int maxRange = VerticalSliceDatabase.maxRange;
                int diffRange = maxRange - minRange;

                int visRange = (int)(graphWidth / graphScaleX);
                int nonVisRange = (int)Mathf.Floor(diffRange - visRange);
                if (nonVisRange > 0) {
                    float scrollSize = Mathf.Max(15, nonVisRange / graphWidth);
                    Rect scrollRect = new Rect(graphX - 15, scrollBarY, graphWidth + 15, scrollHeight);
                    scrollPos = (int)GUI.HorizontalScrollbar(scrollRect, scrollPos, scrollSize, 0, nonVisRange + scrollSize);
                } else {
                    scrollPos = 0;
                }
                //--------------- SCROLLBAR --------------------


                //--------------- FPS GRAPH --------------------
                GUI.Label(new Rect(0, currentY, this.position.width, 20), "Frames-per-second", EditorHelper.styleLabel);
                if (fpsInfoGraph != null) {
                    Rect navRect = new Rect(0, graphY, navpanelWidth, fpsHeight);
                    bool mouseOver = navRect.Contains(Event.current.mousePosition);

                    for (int i = 0; i < fpsInfoGraph.names.Length; i++)
                        DrawButtons(fpsInfoGraph, navRect, i, ref fpsHighlightedIdx, mouseOver && hasFocus, fpsInfoGraph.availability[i]);
                    
                    Rect graphRect = new Rect(graphX, graphY, graphWidth, fpsHeight);
                    if (fpsInfoGraph.DrawGraph(graphRect, scrollPos, graphScaleX, forceUpdate))
                        DrawGraphHorizontalLines(graphRect.x, graphRect.y, graphRect.height, fpsInfoGraph.highestVisibleValue, " fps");

                    currentY = graphRect.y + graphRect.height;
                }
                GUI.FocusControl(""); //Deselects the foldout, needed for arrow left/right navigation
                //--------------- FPS GRAPH --------------------


                //--------------- Middle line -----1---------------
                GUI.color = GUI.contentColor = GUI.backgroundColor = Color.white;
                VisualResources.DrawTexture(new Rect(graphX, currentY, this.position.width, 20), VisualResources.proConsoleRowDark);
                //--------------- Middle line --------------------
			

                //--------------- MEMORY GRAPH --------------------
                GUI.Label(new Rect(0, currentY + 4, this.position.width, 20), "Memory", EditorHelper.styleLabel);
                if (memoryInfoGraph != null) {
                    currentY += 20;
                    Rect navRect = new Rect(0, currentY, navpanelWidth, memoryHeight);
                    bool mouseOver = navRect.Contains(Event.current.mousePosition);

                    for (int i = 0; i < memoryInfoGraph.names.Length; i++) {
                        DrawButtons(memoryInfoGraph, navRect, i, ref memoryHighlightedIdx, mouseOver && hasFocus, memoryInfoGraph.availability[i]);
                    }

                    Rect graphRect = new Rect(graphX, currentY, graphWidth, memoryHeight);
                    if (memoryInfoGraph.DrawGraph(graphRect, scrollPos, graphScaleX, forceUpdate)) 
                        DrawGraphHorizontalLines(graphRect.x, graphRect.y, graphRect.height, memoryInfoGraph.highestVisibleValue / 1048576, " mb");
                    
                    currentY = graphRect.y + graphRect.height;
                }
                GUI.FocusControl(""); //Deselects the foldout, needed for arrow left/right navigation
                GUI.color = GUI.contentColor = Color.white;
                //--------------- MEMORY GRAPH --------------------


                //--------------- Scale slider --------------------
                forceUpdate = false;
                Rect scaleSlider = new Rect(0, currentY, navpanelWidth, 15);
                GUI.contentColor = Color.white;
                float newScaleX = EditorGUI.Slider(scaleSlider, graphScaleX, 0.1f, 5);
                if (newScaleX != graphScaleX) {
                    graphScaleX = newScaleX;
                    forceUpdate = true;
                }
                //--------------- Scale slider --------------------


                //--------------- Selected frame indicator --------------------
                int nextFrameNumber = -1;
                Event current = Event.current;
                int frameUnderMouse = vBugWindowMediator.currentFrameNumber;
                if (current != null)
                {
                    Rect comninedRect = new Rect(graphX, graphY, this.position.width - graphX, currentY - graphY);
                    Vector2 mousePos = Event.current.mousePosition;

                    float barWidth = Mathf.Max(2, graphScaleX);
                    if (comninedRect.Contains(mousePos))
                    {
                        frameUnderMouse = (int)((mousePos.x - graphX) / graphScaleX) + minRange + scrollPos;
                        if (vBugWindowMediator.currentFrameNumber != frameUnderMouse && (eventType == EventType.mouseDrag || eventType == EventType.MouseUp)) {
                            nextFrameNumber = frameUnderMouse;
                            vBugCoreScenePanel.StopPlayback();
                        }

                        float hoverX = ((frameUnderMouse - minRange - scrollPos) * graphScaleX) + graphX;
                        VisualResources.DrawTexture(new Rect(hoverX, graphY, barWidth, currentY - graphY), VisualResources.black);
                    }
                    
                    float indicatorX = ((vBugWindowMediator.currentFrameNumber - minRange - scrollPos) * graphScaleX) + graphX;
                    if (indicatorX >= graphX && indicatorX < this.position.width)
                    {
                        Rect selectIndicator = new Rect(indicatorX, graphY, barWidth, currentY - graphY);
                        GUI.color = new Color(1f, 1f, 1f, 0.333f);
                        VisualResources.DrawTexture(selectIndicator, VisualResources.white);

                        GUI.color = Color.white;
                        float iconX = selectIndicator.x + (graphScaleX / 2) - 16;
                        VisualResources.DrawIcon(EditorIcon.graphArrows, 2, new Vector2(iconX, selectIndicator.y - 32), 1f, false);
                        VisualResources.DrawIcon(EditorIcon.graphArrows, 3, new Vector2(iconX, selectIndicator.y + selectIndicator.height), 1f, false);
                    }
                }
                //--------------- Selected frame indicator --------------------


                //--------------- Vertical lines & mouse hover --------------------
                Rect subGraphRect = new Rect(graphX, graphY, this.position.width - graphX, currentY - graphY);
                DrawGraphVerticalLines(subGraphRect, 100);

                if (fpsInfoGraph != null)
                    fpsInfoGraph.DrawMouseHover(scrollPos, graphScaleX, nextFrameNumber != -1);

                if (memoryInfoGraph != null)
                    memoryInfoGraph.DrawMouseHover(scrollPos, graphScaleX, nextFrameNumber != -1);

                if (eventType == EventType.repaint || eventType == EventType.mouseDown)
                    DrawEventIndicators(new Vector2(graphX, graphY), new Vector2(graphX, graphY + fpsHeight + 20));
                //--------------- Vertical lines & mouse hover --------------------

                if (nextFrameNumber != -1)
                    GotoFrame(nextFrameNumber);
            }
        }


        private void DrawEventIndicators(Vector2 fpsPos, Vector2 memPos) {
            int startFrame = VerticalSliceDatabase.minRange + scrollPos;
            int endFrame = Mathf.Min(VerticalSliceDatabase.maxRange, startFrame + (int)((this.position.width - fpsPos.x) / graphScaleX));
            string oldLevelName = null;

            for (int i = startFrame; i < endFrame; i++) {
                int consoleIconState = 0;
                bool drawLoadLevel = false;

                VerticalActivitySlice slice = VerticalSliceDatabase.GetSlice(i);
                if (slice == null)
                    continue;

                //--------------- Caching --------------------
                if (frameHasConsoleLog.ContainsKey(i)) {
                    consoleIconState = frameHasConsoleLog[i];
                } else {
                    if (slice.debugLogs != null && slice.debugLogs.calls != null && slice.debugLogs.calls.Length > 0) {
                        foreach (DebugLogCall call in slice.debugLogs.calls) {
                            switch (call.type) {
                                case LogType.Assert:
                                case LogType.Log: consoleIconState = Mathf.Max(consoleIconState, 1); break;
                                case LogType.Warning: consoleIconState = Mathf.Max(consoleIconState, 2); break;
                                case LogType.Error: consoleIconState = Mathf.Max(consoleIconState, 3); break;
                            }

                        }
                    }
                    frameHasConsoleLog.Add(i, consoleIconState);
                }
                //--------------- Caching --------------------


                //--------------- Detect level change --------------------
                string currentLevelName = slice.header.levelName;
                drawLoadLevel = oldLevelName != null && oldLevelName != currentLevelName;
                oldLevelName = currentLevelName;
                //--------------- Detect level change --------------------
			

                //--------------- Draw icons --------------------
                int absFrame = i - startFrame;
                if (consoleIconState > 0) {
                    if (consoleIconState == 2) {
                        GUI.color = Color.yellow;
                    } else if (consoleIconState == 3) {
                        GUI.color = Color.red;
                    } else {
                        GUI.color = Color.white;
                    }
                    DrawTopTimelineIcon(fpsPos, absFrame, 1);
                }

                if (drawLoadLevel) {
                    GUI.color = Color.white;
                    DrawTopTimelineIcon(memPos, absFrame, 3);
                }
                //--------------- Draw icons --------------------
            }
        }


        private void DrawTopTimelineIcon(Vector2 graphPos, int absFrame, int subIconIdx) {
            float halfSize = Mathf.Ceil(graphScaleX / 2f);
            Vector2 pos = new Vector2(graphPos.x + halfSize + (absFrame * graphScaleX), graphPos.y);
            VisualResources.DrawTexture(new Rect(pos.x - 1, pos.y - 4, 1, 4), VisualResources.gray);
            VisualResources.DrawIcon(EditorIcon.timelineIndicators, subIconIdx, pos);
        }




        private void DrawGraphHorizontalLines(float x, float y, float height, float maxValue, string suffix)
        {
            DrawGraphHorizontalLine(x, y + (height * 0.75f), 1, (int)(maxValue * 0.25f) + suffix);
            DrawGraphHorizontalLine(x, y + (height * 0.5f), 1, (int)(maxValue * 0.5f) + suffix);
            DrawGraphHorizontalLine(x, y + (height * 0.25f), 1, (int)(maxValue * 0.75f) + suffix);
            DrawGraphHorizontalLine(x, y, 0, (int)maxValue + suffix);
        }


        private void DrawGraphHorizontalLine(float x, float y, int allign, string label)
        {
            float width = 150;
            float height = 15;
            Rect lineRect = new Rect(x, y, this.position.width - x, 1);
            GUI.color = new Color(0.66f,0.66f,0.66f,0.66f);
            VisualResources.DrawTexture(lineRect, VisualResources.white);

            GUIStyle style = EditorHelper.styleLabel;
            TextAnchor bAlign = style.alignment;

            if(allign == 0){
                style.alignment = TextAnchor.UpperRight;
                GUI.Label(new Rect(lineRect.x - width - 5, lineRect.y, width, height), label, style);
            } else if (allign == 1){
                style.alignment = TextAnchor.MiddleRight;
                GUI.Label(new Rect(lineRect.x - width - 5, lineRect.y - (height / 2), width, height), label, style);
            } else if (allign == 2){
                style.alignment = TextAnchor.LowerRight;
                GUI.Label(new Rect(lineRect.x - width - 5, lineRect.y - height, width, height), label, style);
            }
            style.alignment = bAlign;
        }



        private void DrawGraphVerticalLines(Rect graphRect, int spacing) {
            GUI.color = new Color(1f, 1f, 1f, 0.333f);
            GUIStyle style = EditorHelper.styleLabel;
            TextAnchor bAlign = style.alignment;
            style.alignment = TextAnchor.MiddleCenter;

            int lines = (int)Mathf.Ceil(graphRect.width / spacing);
            for(int i = 0; i < lines; i ++){
                int x = (int)graphRect.x + (i * spacing);
                
                Rect lineRect = new Rect(x, graphRect.y, 1, graphRect.height + 10);
                Rect labelRect = new Rect(x - 50, lineRect.y + lineRect.height + 5, 100, 15);
                VisualResources.DrawTexture(lineRect, VisualResources.white);
                GUI.Label(labelRect, "" + (int)((VerticalSliceDatabase.minRange + scrollPos) + ((i * spacing) / graphScaleX)), style);
            }

            GUI.color = Color.white;
            style.alignment = bAlign;
        }



        private void DrawButtons(BaseGraphContainer infoGraph, Rect navRect, int idx, ref int highlightedIdx, bool mouseOverNavPanel, bool dataAvailable)
        {
            int elementHeight = 16;
            Rect rect = new Rect(navRect.x + 5, navRect.y + (elementHeight * idx), navRect.width - 10, elementHeight);
            BaseGraphContainer.VisualStyle current = infoGraph.visualStyles[idx];

            Vector2 mousePos = Event.current.mousePosition;
            BaseGraphContainer.VisualStyle target = infoGraph.visualStyles[idx];

            bool mouseOver = mouseOverNavPanel && rect.Contains(mousePos);
            bool isHidden = current == BaseGraphContainer.VisualStyle.hidden;

            if (mouseOver)
                highlightedIdx = idx;
            
            if (mouseOverNavPanel && !isHidden) {
                if (highlightedIdx != -1 && highlightedIdx != idx) {
                    target = BaseGraphContainer.VisualStyle.dark;
                } else {
                    target = BaseGraphContainer.VisualStyle.normal;
                }
            } else{
                if (!isHidden)
                    target = BaseGraphContainer.VisualStyle.normal;
            }

            if (dataAvailable) {
                GUI.backgroundColor = infoGraph.colors[idx];
                if (current == BaseGraphContainer.VisualStyle.normal) {
                    GUI.contentColor = Color.white;
                } else if (current == BaseGraphContainer.VisualStyle.dark) {
                    GUI.contentColor = new Color(0.5f, 0.5f, 0.5f, 0.75f);
                } else if (current == BaseGraphContainer.VisualStyle.hidden) {
                    GUI.backgroundColor = new Color(0.6f, 0.6f, 0.6f);
                    GUI.contentColor = Color.black;
                }
            } else {
                GUI.backgroundColor = new Color(0.3f, 0.3f, 0.3f);
                GUI.contentColor = Color.gray;
                GUI.enabled = false;
            }


            bool currentVis = infoGraph.visualStyles[idx] == BaseGraphContainer.VisualStyle.normal;
            bool nextVis = GUIBoxButton(rect, currentVis, infoGraph.names[idx]);
            if(currentVis != nextVis)
                target = nextVis ? BaseGraphContainer.VisualStyle.normal : BaseGraphContainer.VisualStyle.hidden;
                
            infoGraph.SetStyle(idx, target);
            GUI.color = GUI.contentColor = GUI.backgroundColor = Color.white;
            GUI.enabled = true;
        }


        private bool GUIBoxButton(Rect rect, bool value, string context, float scale = 0.7f)
        {
            rect.width /= scale;
            rect.height /= scale;
            Color bgColor = GUI.backgroundColor;
            bgColor.a = 0.25f;
            GUI.color = bgColor;

            Matrix4x4 mBackup = GUI.matrix;
            GUIUtility.ScaleAroundPivot(new Vector2(scale, scale), new Vector2(rect.x, rect.y));
            VisualResources.DrawTexture(new Rect(rect.x, rect.y, rect.width, rect.height - 1), VisualResources.white);
            GUI.color = Color.white;
            bool result = GUI.Toggle(rect, value, context, myToggleStyle);
            GUI.matrix = mBackup;
            return result;
        }

		#endregion
		//--------------------------------------- DRAW & UPDATE GUI --------------------------------------
		//--------------------------------------- DRAW & UPDATE GUI --------------------------------------








        //--------------------------------------- KEY UPDATE --------------------------------------
        //--------------------------------------- KEY UPDATE --------------------------------------
        #region KEY UPDATE


        private void UpdateKeyInput()
        {
            if (!hasFocus || Event.current == null)
                return;

            EventType eventType = Event.current.type;

            //--------------- Check for repaint first --------------------
            if (eventType == EventType.keyDown || eventType == EventType.keyUp || eventType == EventType.mouseUp || eventType == EventType.mouseDrag)
                Repaint();
            //--------------- Check for repaint first --------------------

            if (eventType == EventType.keyDown && downTimeStamp == -1d)
            {
                downTimeStamp = vBugEnvironment.GetUnixTimestamp();
            }
            else if (eventType == EventType.keyUp || (downTimeStamp != -1 && vBugEnvironment.GetUnixTimestamp() - downTimeStamp > 0.3f))
            {
                if (eventType == EventType.keyUp)
                    downTimeStamp = -1d;

                int minRange = VerticalSliceDatabase.minRange;
                int maxRange = VerticalSliceDatabase.maxRange;

                //--------------- Get Key-direction --------------------
                int nextFrame = vBugWindowMediator.currentFrameNumber;
                KeyCode keyCode = Event.current.keyCode;
                if (keyCode == KeyCode.LeftArrow) {
                    nextFrame = Mathf.Clamp(vBugWindowMediator.currentFrameNumber - 1, minRange, maxRange);
                    vBugCoreScenePanel.StopPlayback();
                } else if (keyCode == KeyCode.RightArrow) {
                    nextFrame = Mathf.Clamp(vBugWindowMediator.currentFrameNumber + 1, minRange, maxRange);
                    vBugCoreScenePanel.StopPlayback();
                }
                //--------------- Get Key-direction --------------------

                if (vBugWindowMediator.currentFrameNumber != nextFrame) {
                    FocusSelectedFrame();
                    vBugWindowMediator.NotifyTimelineChange(nextFrame, this.GetInstanceID());
                }

            }
        }

        private void FocusSelectedFrame()
        {
            int minRange = VerticalSliceDatabase.minRange;
            float indicatorX = (vBugWindowMediator.currentFrameNumber - minRange - scrollPos) * graphScaleX;
            float graphWidth = this.position.width - graphX;
            if (indicatorX < 0)
            {
                scrollPos = vBugWindowMediator.currentFrameNumber - minRange - 1;
            }
            else if (indicatorX > graphWidth - 1)
            {
                scrollPos = vBugWindowMediator.currentFrameNumber - minRange - (int)Mathf.Floor(graphWidth / graphScaleX) + 2;
            }
        }


        #endregion
        //--------------------------------------- KEY UPDATE --------------------------------------
        //--------------------------------------- KEY UPDATE --------------------------------------
			

    }
}