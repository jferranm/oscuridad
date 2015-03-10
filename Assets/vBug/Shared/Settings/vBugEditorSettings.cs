using UnityEngine;
using System;
using Frankfort.VBug.Internal;


namespace Frankfort.VBug.Editor
{
    public static class vBugEditorSettings
    {
        public static bool DebugMode = false;
        public static bool MultithreadedLoading = true;

        public static int PlaybackMeshGhostLength = 400;
        public static int PlaybackMeshBonesInterval = 1;
        public static Color PlaybackMeshBoundingBoxColor = new Color(1f, 0.75f, 0.5f);
        public static int PlaybackRenderLayer = 31;

        public static int PlaybackMeshSearchRange = 200;
        public static int PlaybackParticleSearchRange = 150;

        public static int PlaybackSystemDefaultSearchRange = 30;
        public static int PlaybackSystemMemorySearchRange = 500;

        public static int PlaybackOverlaySearchRange = 30;
        public static int PlaybackMaterialSearchRange = 100;


        public static int PlaybackHierarchySearchRange = 30;
        public static int PlaybackHierarchyEOLBirthSearchRange = 10;

        public static int BackgroundLoadTheadBundleSize = 200; //2 x 60fps per load update
        public static int BackgroundLoadThreadSleepTime = 10;
        
    }
}