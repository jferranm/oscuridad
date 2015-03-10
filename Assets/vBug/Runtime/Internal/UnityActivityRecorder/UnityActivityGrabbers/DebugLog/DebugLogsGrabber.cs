using System;
using UnityEngine;
using System.Collections.Generic;

namespace Frankfort.VBug.Internal
{
    public class DebugLogsGrabber : BaseActivityGrabber<DebugLogSnapshot>
    {
        private List<DebugLogCall> callQueue = new List<DebugLogCall>();

        private void OnEnable()
        {
            Application.RegisterLogCallback(CaptureDebugLog);
        }
        private void OnDisable()
        {
            Application.RegisterLogCallback(null);
        }
        
        protected override void GrabResultEndOfFrame()
        {
            base.GrabResultEndOfFrame();
            if (resultReadyCallback != null)
                resultReadyCallback(currentFrame, new DebugLogSnapshot(callQueue.ToArray()), 0);

            callQueue.Clear();
        }

        public override void AbortAndPruneCache()
        {
            base.AbortAndPruneCache();
            callQueue.Clear();
        }

        private void CaptureDebugLog(string logString, string stackTrace, LogType type)
        {
            if (vBugLogger.Contains(logString))
                return;

            callQueue.Add(new DebugLogCall(type, logString, stackTrace));
        }
    }
}