using Frankfort.VBug.Internal;
using System;
using UnityEngine;

namespace Frankfort.VBug
{

    [Serializable]
    public class vBugGlobalSettings
    {

        //--------------- Sub-setting groups --------------------
        [Serializable]
        public class General {
            /// <summary>
            /// Enable 'debugMode' to receive logs and errors by vBug itself.
            /// </summary>
            public bool debugMode = false;
            /// <summary>
            /// Enable/Disable multi-threading during recording. This speeds-up data-encoding and capturing.
            /// </summary>
            public bool multiThreading = true;
            /// <summary>
            /// Each frame is saved as 'vBugSlice' file, and framePadding ensures the amount of digits are constant (ex: 000000001.vBugSlice)
            /// </summary>
            public int framePadding = 10;
            
            /// <summary>
            /// Settings regarding frame-processing
            /// </summary>
            public VerticalSliceSettings verticalSlice = new VerticalSliceSettings();
            /// <summary>
            /// Settings regarding data-processing
            /// </summary>
            public DataProcessingSettings dataProcessing = new DataProcessingSettings();
        }


        [Serializable]
        public class Recording {
            public SystemInfoSettings systemInfoRecording = new SystemInfoSettings();
            public DebugLogGrabSettings debugLogsRecording = new DebugLogGrabSettings();
            public HumanInputSettings humanInputRecording = new HumanInputSettings();
            public MeshDataSettings meshDataRecording = new MeshDataSettings();
            public ParticleDataSettings particleDataRecording = new ParticleDataSettings();
            public MaterialCaptureSettings materialDataRecording = new MaterialCaptureSettings();
            public GameObjectReflectionSettings gameObjectReflection = new GameObjectReflectionSettings();
            public ScreenCaptureSettings screenCapture = new ScreenCaptureSettings();
        }


        /// <summary>
        /// All general settings regarding frame-processing & data-processing
        /// </summary>
        public General general = new General();

        /// <summary>
        /// All settings regarding activity-recording bundeld
        /// </summary>
        public Recording recording = new Recording();
        //--------------- Sub-setting groups --------------------
    }



    [Serializable]
    public class DataProcessingSettings
    {
        /// <summary>
        /// Sleep-time per worker thread between processing-jobs
        /// </summary>
        public int threadSleepTime = 5;
        /// <summary>
        /// Max pending data-packages ready to be processed
        /// </summary>
        public int processPackageMaxQueueSize = 64;
        /// <summary>
        /// Max amount of packages being processed in parallel
        /// </summary>
        public int processPackageMaxPendingSize = 64;
        public ProcessHandlingType processingType = ProcessHandlingType.saveToDisk;// | ProcessHandlingType.streamToEditor;

        public bool standaloneStoreToApplicationFolder = true; 
    }


    [Serializable]
    public class VerticalSliceSettings
    {
        /// <summary>
        /// Number of unprocessed slices in the queue
        /// </summary>
        public int maxPendingQueueSize = 15;
        /// <summary>
        /// Maximum lifetime per slice to be processed
        /// </summary>
        public int maxPendingLifetimeMS = 5000;

        /// <summary>
        /// Incase the max queue is reached, the main thread will be 'slowed down' allowing the processing-threads to catch-up. If its set to false, the unprocessed slices will be pruned
        /// </summary>
        public bool allowMainThreadSleep = false;
        public int maxPendingSleep = 5;
        /// <summary>
        /// maximum  mainthread sleep-time
        /// </summary>
        public int sleepTimeMS = 30;
    }

}