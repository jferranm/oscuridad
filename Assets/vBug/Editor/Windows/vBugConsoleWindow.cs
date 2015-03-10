using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Frankfort.VBug.Internal;

namespace Frankfort.VBug.Editor
{

    public class vBugConsoleWindow : vBugBaseWindow {

        public struct CallCachdeData
        {
            public DebugLogCall[] calls;
            public int y;
            public int height;
            public int frameNumber;
            public string niceFrameNumber;
            public string[] niceTimeStamp;
        }



        private const int BatchCount = 3;
        private const int ElementHeight = 35;
        private const int BoxRightSize = 75;

        private Rect dragBarRect;
        private float dragBarCurrentWinHeight;
        private bool dragBarResize = false;
        
        private Vector2 scrollPos = Vector2.zero;
        

        private DebugLogCall selectedCall;
        private float selectedCallPos;
        private int selectedFrame;
        private int timelineFrame;

        private int totalBatchHeight;
        private List<CallCachdeData> cachedCallsData = new List<CallCachdeData>();
        private float currentSize;
        private double downTimeStamp = -1d;
        private double doubleTapTimeStamp;


        protected override void OnEnable()
        {
            base.OnEnable();
            this.title = "vBug Console";

            string dragKey = this.GetType().Name + "_dragBarY";
            if (EditorPrefs.HasKey(dragKey)) {
                dragBarRect.y = EditorPrefs.GetFloat(dragKey);
            } else {
                dragBarRect.y = this.position.height * 0.8f;
            }
            RebuildCachedData();
        }

        protected override void OnDisable() {
            base.OnDisable();
            EditorPrefs.SetFloat(this.GetType().Name + "_dragBarY", dragBarRect.y);
        }

        protected override void DisposeAll()
        {
            base.DisposeAll();
            cachedCallsData.Clear();
            selectedCall = null;
            selectedCallPos = 0;
            selectedFrame = 0;
            timelineFrame = 0;
        }






        //--------------------------------------- NOTIFICATIONS --------------------------------------
        //--------------------------------------- NOTIFICATIONS --------------------------------------
        #region NOTIFICATIONS


        public override void NotifyTimelineChange(int newFrame, int senderID) {
            base.NotifyTimelineChange(newFrame, senderID);
            if (senderID == this.GetInstanceID())
                return;

            FocusFrame(newFrame);
            Repaint();
        }

        public override void NotifySessionChange(int startFrame, int senderID) {
            base.NotifySessionChange(startFrame, senderID);
            if (senderID == this.GetInstanceID())
                return;

            RebuildCachedData();
            Repaint();
        }

        public override void NotifyVerticalSliceBundleLoaded(VerticalActivitySlice[] slices) {
            base.NotifyVerticalSliceBundleLoaded(slices);
            RebuildCachedData();
            Repaint();
        }
        #endregion
        //--------------------------------------- NOTIFICATIONS --------------------------------------
        //--------------------------------------- NOTIFICATIONS --------------------------------------
			










        //--------------------------------------- CACHE BUILDING --------------------------------------
        //--------------------------------------- CACHE BUILDING --------------------------------------
        #region CACHE BUILDING

        private void PruneCache() {
            cachedCallsData.Clear();
            selectedCall = null;
            totalBatchHeight = 0;
            selectedCallPos = 0;
            selectedFrame = 0;
            totalBatchHeight = 0;
        }


        private void RebuildCachedData() {
            VerticalActivitySlice[] slices = VerticalSliceDatabase.GetAllAvailableSlices();
            if (slices == null || slices.Length == 0)
                return;

            //--------------- Reset --------------------
            int iMax = slices.Length;
            PruneCache();
            cachedCallsData = new List<CallCachdeData>(iMax);
            //--------------- Reset --------------------

            for (int i = 0; i < iMax; i++) {
                VerticalActivitySlice slice = slices[i];

                if (slice != null && slice.debugLogs != null) {
                    string niceFrameNumber = vBugEnvironment.GetNiceFrameNumber(slice.header.frameNumber);
                    if (slice.debugLogs.calls.Length > 0) {
                        int jMax = slice.debugLogs.calls.Length;
                        string[] niceTimeStamps = new string[jMax];
                        for (int j = 0; j < jMax; j++)
                            niceTimeStamps[j] = vBugEnvironment.ToUnixDateTime(slice.debugLogs.calls[j].unixTimeStamp).ToString("yyyy-MM-dd  HH:mm:ss:fff");

                        int newHeight = slice.debugLogs.calls.Length * ElementHeight;
                        cachedCallsData.Add(new CallCachdeData() {
                            y = totalBatchHeight,
                            height = newHeight,
                            calls = slice.debugLogs.calls,
                            frameNumber = slice.header.frameNumber,
                            niceFrameNumber = niceFrameNumber,
                            niceTimeStamp = niceTimeStamps
                        });

                        totalBatchHeight += newHeight;
                    }
                }
            }
        }

        #endregion
        //--------------------------------------- CACHE BUILDING --------------------------------------
        //--------------------------------------- CACHE BUILDING --------------------------------------













        //--------------------------------------- UPDATE & ONGUI --------------------------------------
        //--------------------------------------- UPDATE & ONGUI --------------------------------------
        #region UPDATE & ONGUI

        void OnGUI() {
            try {
                if (EditorApplication.isPlayingOrWillChangePlaymode)
                    return;

                VisualResources.DrawWindowBGWaterMark(this.position);
                UpdateKeyInput();
                DrawGUI();
            } catch (Exception e) {
                if (vBugEditorSettings.DebugMode)
                    Debug.LogError(e.Message + e.StackTrace);
            }
        }

        #endregion
        //--------------------------------------- UPDATE & ONGUI --------------------------------------
        //--------------------------------------- UPDATE & ONGUI --------------------------------------
			













        //--------------------------------------- DRAW GUI --------------------------------------
        //--------------------------------------- DRAW GUI --------------------------------------
        #region DRAW GUI

        private void DrawGUI()
        {
            GUI.SetNextControlName("vBugConsole");
            EditorHelper.DrawDragBar(this, ref dragBarRect, ref dragBarResize, ref dragBarCurrentWinHeight);
            
            GUILayout.BeginVertical();
            scrollPos = GUILayout.BeginScrollView(scrollPos, GUILayout.Height(dragBarRect.y));

            if (!VerticalSliceDatabase.isInitialized){
                EditorHelper.DrawNotInitializedLabel(this.position);
            } else {
                DrawAllAvailableLogs(scrollPos.y);
            }
            
            GUILayout.EndScrollView();
            DrawStackTrace();
            GUILayout.EndVertical();
            //EditorHelper.DrawTopRightLogo(this.position);
        }



        private void DrawAllAvailableLogs(float scrollPos)
        {
            if (cachedCallsData.Count == 0)
                return;

            int startX = 32;
            int halfHeight = ElementHeight / 2;
            float minus = totalBatchHeight > dragBarRect.y ? 15 : 0;
            float winWidth = this.position.width - minus;
            bool pingPong = false;

            Texture2D tA = VisualResources.proConsoleRowDark;
            Texture2D tB = VisualResources.proConsoleRowLight;
            Texture2D tSelection = VisualResources.proSelection;
            Texture2D tOrange = VisualResources.timelineOrange;
            Color alphaColor = new Color(1f, 1f, 1f, 0.35f);

            bool mouseUp = Event.current != null && Event.current.type == EventType.mouseUp;
            Vector2 mousePos = Event.current != null ? Event.current.mousePosition : Vector2.zero;

            int iMax = cachedCallsData.Count;
            GUILayoutUtility.GetRect(winWidth, cachedCallsData[iMax - 1].y + cachedCallsData[iMax - 1].height);


            for (int i = 0; i < iMax; i++)
            {
                CallCachdeData cached = cachedCallsData[i];
                float screenPos = cached.y - scrollPos;

                Rect rect = new Rect(0, cached.y, winWidth, cached.height);

                if (screenPos > -cached.height && screenPos < dragBarRect.y)
                {
                    pingPong = i % 2 == 0;
                    VisualResources.DrawTexture(rect, pingPong ? tA : tB);
                    Texture2D line = pingPong ? tB : tA;

                    //--------------- Box on right --------------------
                    float boxRightX = winWidth - BoxRightSize;
                    GUI.color = alphaColor;
                    if (cached.frameNumber == timelineFrame){
                        VisualResources.DrawTexture(new Rect(boxRightX, cached.y, BoxRightSize, cached.height), tOrange);
                    } else if (cached.frameNumber == selectedFrame && selectedCall != null) { //if selected - box on right
                        VisualResources.DrawTexture(new Rect(boxRightX, cached.y, BoxRightSize, cached.height), tSelection);
                    }
                    GUI.color = Color.white;
                    //--------------- Box on right --------------------
			

                    int cMax = cached.calls.Length;
                    for (int j = 0; j < cMax; j++) {

                        //--------------- Draw logs, selections & timestamp --------------------
                        DebugLogCall call = cached.calls[j];
                        float subPosY = cached.y + (j * ElementHeight);
                        Rect logRect = new Rect(0, subPosY, winWidth - BoxRightSize, ElementHeight);

                        if (mouseUp && logRect.Contains(mousePos))
                        {
                            double tapTimeStamp = vBugEnvironment.GetUnixTimestamp();

                            if (selectedCall == call && tapTimeStamp - doubleTapTimeStamp < 0.33f)
                                vBugWindowMediator.NotifyTimelineChange(cached.frameNumber, this.GetInstanceID());

                            selectedCall = call;
                            selectedCallPos = cached.y;
                            selectedFrame = cached.frameNumber;
                            doubleTapTimeStamp = tapTimeStamp;
                        }

                        if (selectedCall == call) //if selected
                            VisualResources.DrawTexture(logRect, tSelection);

                        Rect horLine = new Rect(0, logRect.y - 1, logRect.width - 4, 1);
                        if (j > 0)
                            VisualResources.DrawTexture(horLine, line);

                        logRect.height = halfHeight;
                        logRect.x = startX;
                        logRect.width -= startX;
                        GUI.Label(logRect, call.logString, EditorHelper.styleLabelLightGray12);

                        logRect.y += halfHeight;
                        GUI.color = alphaColor;
                        GUI.Label(logRect, cached.niceTimeStamp[j], EditorHelper.styleLabelLightGray12);
                        GUI.color = Color.white;
                        //--------------- Draw logs, selections & timestamp --------------------

                        //--------------- Vert line --------------------
                        Rect vertRect = new Rect(boxRightX, cached.y + 4, 1, cached.height - 8);
                        VisualResources.DrawTexture(vertRect, line);
                        //--------------- Vert line --------------------
			
                        //--------------- framenumber --------------------
                        Vector2 labelSize = EditorHelper.styleLabelLightGray12.CalcSize(new GUIContent(cached.niceFrameNumber));
                        GUI.Label(new Rect(
                            vertRect.x + (BoxRightSize / 2) - (labelSize.x / 2),
                            cached.y + (cached.height / 2) - (labelSize.y / 2),
                            100, ElementHeight), cached.niceFrameNumber, EditorHelper.styleLabelLightGray12);
                        //--------------- framenumber --------------------


                        //--------------- Draw icons --------------------
                        int subIconIdx = 0;
                        switch (call.type)
                        {
                            case LogType.Warning:
                                subIconIdx = 0;
                                break;
                            case LogType.Log:
                                subIconIdx = 1; //goed
                                break;
                            case LogType.Exception:
                                subIconIdx = 2;
                                break;
                            case LogType.Assert:
                            case LogType.Error:
                                subIconIdx = 3;
                                break;
                        }
                        VisualResources.DrawIcon(EditorIcon.consoleLogs, subIconIdx, new Vector2(0, subPosY), 1f, false);
                        //--------------- Draw icons --------------------
                    }

                }
            }
        }

        private void DrawStackTrace() {
            if (selectedCall == null)
                return;

            Rect rect = new Rect(0, dragBarRect.yMax, this.position.width - 25, this.position.height - dragBarRect.yMax);
            if (!string.IsNullOrEmpty(selectedCall.stackTrace)) {
                EditorGUI.SelectableLabel(rect, selectedCall.logString + "\n\n" + selectedCall.stackTrace, EditorHelper.styleSelectableLabel);
            } else {
                EditorHelper.DrawNA(rect, "There is no stacktrace available.\nPlease enable 'Development Build' & 'Script Debugging' in your build-settings.");
            }
        }

        private void FocusFrame(int newFrame)
        {
            timelineFrame = -1;
            for(int i = 0; i < cachedCallsData.Count; i++)
            {
                if(cachedCallsData[i].frameNumber >= newFrame)
                {
                    timelineFrame = cachedCallsData[i].frameNumber;
                    scrollPos.y = cachedCallsData[i].y;
                    return;
                }
            }
        }
        #endregion
        //--------------------------------------- DRAW GUI --------------------------------------
        //--------------------------------------- DRAW GUI --------------------------------------
			
















        //--------------------------------------- KEY UP & DOWN --------------------------------------
        //--------------------------------------- KEY UP & DOWN --------------------------------------
        #region KEY UP & DOWN

        private void UpdateKeyInput()
        {
            if (!hasFocus || Event.current == null || selectedCall == null)
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

                if (selectedCall != null && cachedCallsData.Count > 0)
                {

                    //--------------- Get Key-direction --------------------
                    int direction = 0;
                    KeyCode keyCode = Event.current.keyCode;
                    if (keyCode == KeyCode.UpArrow)
                    {
                        direction = -1;
                    }
                    else if (keyCode == KeyCode.DownArrow)
                    {
                        direction = +1;
                    } 
                    //--------------- Get Key-direction --------------------
			

                    if (direction != 0)
                    {

                        int cacheIdx = 0;
                        int callIdx = 0;
                        if (FindContainingCache(selectedCall, out cacheIdx, out callIdx))
                        {

                            //--------------- Increment / decrement --------------------
                            if (direction < 0 && cacheIdx > 0 && callIdx == 0)
                            {
                                cacheIdx -= 1;
                                callIdx = cachedCallsData[cacheIdx].calls.Length - 1;
                            }
                            else if (direction > 0 && cacheIdx < cachedCallsData.Count - 1 && callIdx == cachedCallsData[cacheIdx].calls.Length - 1)
                            {
                                cacheIdx += 1;
                                callIdx = 0;
                            }
                            else
                            {
                                callIdx = Mathf.Clamp(callIdx + direction, 0, cachedCallsData[cacheIdx].calls.Length - 1);
                            }
                            selectedCall = cachedCallsData[cacheIdx].calls[callIdx];
                            selectedCallPos = cachedCallsData[cacheIdx].y + (callIdx * ElementHeight);
                            selectedFrame = cachedCallsData[cacheIdx].frameNumber;
                            //--------------- Increment / decrement --------------------
			


                            //--------------- Snap scroll based on selection --------------------
                            if (selectedCall != null)
                            {
                                float selectedScreenPos = selectedCallPos - scrollPos.y;
                                if (direction < 0 && selectedScreenPos < 0)
                                {
                                    scrollPos.y -= ElementHeight;
                                }
                                else if (direction > 0 && selectedScreenPos > dragBarRect.y - ElementHeight)
                                {
                                    scrollPos.y += Mathf.Abs(selectedScreenPos - dragBarRect.y) + ElementHeight;
                                }
                            }
                            //--------------- Snap scroll based on selection --------------------
                        }
                    }			
                }
            }
        }


        private bool FindContainingCache(DebugLogCall selectedCall, out int cacheIdx, out int callIdx)
        {
            int i = cachedCallsData.Count;
            while (--i > -1)
            {
                CallCachdeData compair = cachedCallsData[i];
                int j = compair.calls.Length;
                while (--j > -1)
                {
                    if (compair.calls[j] == selectedCall)
                    {
                        cacheIdx = i;
                        callIdx = j;
                        return true;
                    }
                }
            }
            cacheIdx = -1;
            callIdx = -1;
            return false;
        }

        #endregion
        //--------------------------------------- KEY UP & DOWN --------------------------------------
        //--------------------------------------- KEY UP & DOWN --------------------------------------
			


    }
}
