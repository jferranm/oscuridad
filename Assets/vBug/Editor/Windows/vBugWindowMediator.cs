using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Reflection;
using Frankfort.VBug.Internal;

namespace Frankfort.VBug.Editor
{


    public static class vBugWindowMediator
    {

        public static int currentFrameNumber { get; private set; }


        //--------------- Notifications --------------------
        public static void NotifySessionChange(int startFrame, int senderID){
            currentFrameNumber = startFrame;
            if (EditorApplication.isPlaying || EditorApplication.isPlayingOrWillChangePlaymode)
                return;

            foreach (vBugBaseWindow window in vBugBaseWindow.GetAllWindows())
                window.NotifySessionChange(startFrame, senderID);
        }


        public static void NotifyTimelineChange(int frameNumber, int senderID){
            currentFrameNumber = frameNumber;
            if (EditorApplication.isPlaying || EditorApplication.isPlayingOrWillChangePlaymode)
                return;

            foreach (vBugBaseWindow window in vBugBaseWindow.GetAllWindows())
                window.NotifyTimelineChange(frameNumber, senderID);
        }


        public static void NotifyRecordingStart(int senderID){
            if (EditorApplication.isPlaying || EditorApplication.isPlayingOrWillChangePlaymode)
                return;

            foreach (vBugBaseWindow window in vBugBaseWindow.GetAllWindows())
                window.NotifyRecordingStart(senderID);
        }


        public static void NotifyRecordingStop(int senderID){
            if (EditorApplication.isPlaying || EditorApplication.isPlayingOrWillChangePlaymode)
                return;

            foreach (vBugBaseWindow window in vBugBaseWindow.GetAllWindows())
                window.NotifyRecordingStop(senderID);
        }


        public static void NotifyDeviceConnected(int senderID){
            if (EditorApplication.isPlaying || EditorApplication.isPlayingOrWillChangePlaymode)
                return;

            foreach (vBugBaseWindow window in vBugBaseWindow.GetAllWindows())
                window.NotifyDeviceConnected(senderID);
        }


        public static void NotifyDeviceDisconnected(int senderID){
            if (EditorApplication.isPlaying || EditorApplication.isPlayingOrWillChangePlaymode)
                return;

            foreach (vBugBaseWindow window in vBugBaseWindow.GetAllWindows())
                window.NotifyDeviceDisconnected(senderID);
        }


        public static void NotifyVerticalSliceBundleLoaded(VerticalActivitySlice[] slices) {
            if (EditorApplication.isPlaying || EditorApplication.isPlayingOrWillChangePlaymode)
                return;

            foreach (vBugBaseWindow window in vBugBaseWindow.GetAllWindows())
                window.NotifyVerticalSliceBundleLoaded(slices);
        }
        //--------------- Notifications --------------------
    }
}