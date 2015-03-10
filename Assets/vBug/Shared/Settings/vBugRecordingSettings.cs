using Frankfort.VBug.Internal;
using System;
using System.Xml.Serialization;
using UnityEngine;


namespace Frankfort.VBug
{
    [Serializable]
    public class CaptureIntervalSettings
    {
        /// <summary>
        /// The max-framerate cap with which the current recorder is running
        /// </summary>
        public float maxFrameRate = -1;
        /// <summary>
        /// The interval the recorder captures frames. In case this still exceeds the max-framerate, it will be capped to that value
        /// </summary>
        public int targetInterval = -1;
        /// <summary>
        /// The frame-offset with which a recorder will start. 
        /// By setting each recorder to a slightly different offset combined with a larger targetInterval will help distribute the workload among multiple frames
        /// </summary>
        public int frameOffset = 0;

        public CaptureIntervalSettings() //Default constructor
        { }
        public CaptureIntervalSettings(float maxFrameRate, int targetInterval, int frameOffset)
        {
            this.maxFrameRate = maxFrameRate;
            this.targetInterval = targetInterval;
            this.frameOffset = frameOffset;
        }
    }


    [Serializable]
    public class SystemInfoSettings {
        /// <summary>
        /// Enable / Disable recording
        /// </summary>
        public bool enabled = true;
        /// <summary>
        /// Capture interval settings
        /// </summary>
        public CaptureIntervalSettings captureInterval = new CaptureIntervalSettings(30, 1, 0);
        
        /// <summary>
        /// Set this settings to 'true' if you also want the memory-usage to captured. This is an Unity-PRO feature only
        /// Enabling this setting will cost additional performance and to be fair, Unity's own Deep-profiler is absolutely 
        /// </summary>
        public bool autoEnableProfiler = false;
        /// <summary>
        /// This 'sub-capture-interval' will spread the memory capture even further (on top of the 'captureInterval' settings)
        /// This is needed because scanning all the memory takes-up a lot of the performance (much like Unity's own deep-profiling)
        /// </summary>
        public int memoryUpdateInterval = 30;
        /// <summary>
        /// In order to get a correct estimate of internal memory usage, this boolean should be enabled. However, if set to 'true', performance will be killed!
        /// </summary>
        public bool gcForceFullCollection = false;
        
        /// <summary>
        /// Turning off individual types will save a lot of performance each time memory is measured.
        /// This is especially useful when 'memoryUpdateInterval' is set to a low value and you experience performance dorps.
        /// </summary>
        public bool recordTexture2Ds = true;
        public bool recordRenderTextures = true;
        public bool recordMaterials = true;
        public bool recordMeshes = true;
        public bool recordAnimations = true;
        public bool recordAudioClips = true;
    }


    [Serializable]
    public class DebugLogGrabSettings {
        /// <summary>
        /// Enable / Disable recording
        /// </summary>
        public bool enabled = true;
        /// <summary>
        /// Capture interval settings
        /// </summary>
        public CaptureIntervalSettings captureInterval = new CaptureIntervalSettings(-1, -1, 0);
    }


    [Serializable]
    public class HumanInputSettings {
        /// <summary>
        /// Enable / Disable recording
        /// </summary>
        public bool enabled = true;
        /// <summary>
        /// Capture interval settings
        /// </summary>
        public CaptureIntervalSettings captureInterval = new CaptureIntervalSettings(-1, -1, 0);


        [Serializable]
        public class MouseSettings {
            /// <summary>
            /// The amount of pixels a mouse needs to move before it will appear as 'moving' in the 'vBug PlayBackWindow'
            /// </summary>
            public float moveDistanceThreshold = 5f;
            public float holdDurationThreshold = 0.25f;
        }


        [Serializable]
        public class AxisSettings
        {
            [Serializable]
            public class Definition
            {
                public string x;
                public string y;

                public Definition()//Default Constructor
                { }

                public Definition(string x, string y)
                {
                    this.x = x;
                    this.y = y;
                }
            }

            /// <summary>
            /// If set to false it will use the normal 'GetAxis'. If set tot true, it will use GetAxisRaw instead/
            /// </summary>
            public bool useGetAxisRaw = false;
            /// <summary>
            /// The minimal threshold movement per second in order to appear as 'moving'
            /// </summary>
            public float detectMotionThreshold = 0.5f;

            /// <summary>
            /// A list of combined X-Y values per capture type for easy generic storage
            /// </summary>
            public Definition[] definitions = new Definition[]
            {
                new Definition("Horizontal", "Vertical"),
                new Definition("Fire1", null),
                new Definition("Fire2", null),
                new Definition("Fire3", null),
                new Definition("Jump", null),
                new Definition("Mouse X", "Mouse Y"),
                new Definition("Mouse X", "Mouse Y"),
                new Definition("Mouse ScrollWheel", null),
                new Definition("Window Shake X", "Window Shake Y")
            };
        }

        /// <summary>
        /// A list of the enabled data-providers to be included. Please disable by commenting the lines.
        /// </summary>
        public string[] activeProviders = new string[]
        {
            "MouseInputProvider",
            "KeyboardInputProvider",
            "TouchInputProvider",
            "AxisInputProvider"
        };

        public MouseSettings mouse = new MouseSettings();
        public AxisSettings axis = new AxisSettings();
    }


    [Serializable]
    public class MeshDataSettings {
        /// <summary>
        /// Enable / Disable recording
        /// </summary>
        public bool enabled = true;
        /// <summary>
        /// Capture interval settings
        /// </summary>
        public CaptureIntervalSettings captureInterval = new CaptureIntervalSettings(30, 3, 1);
        /// <summary>
        /// During playback, vBug reconstructs meshes by searching for the closest available keyframe-data, much like modern video-data-compressions
        /// Each keyframe contains a full meshcapture, ensuring relevant meshdata to work with during playback
        /// </summary>
        public int forceKeyFrameInterval = 120;
        /// <summary>
        /// Only objects with a 'vBugMeshRecordable' component will be captured. This flag will also capture all of its children.
        /// If you only want to capture the object containing the 'vBugMeshRecordable' component and not his children, then set this boolean to 'false'
        /// </summary>
        public bool scanRecursiveUp = true;
        /// <summary>
        /// When performing a full mesh capture, we can drastically gain performance by limiting the amount of bone-influences (much like Unity's own quality setting)
        /// </summary>
        public int maxBlendWeights = 1;

        /// <summary>
        /// A list of the enabled data-providers to be included. Please disable by commenting the lines.
        /// </summary>
        public string[] activeProviders = new string[]
        {
            "MeshFilterProvider",
            "SkinnedMeshProvider"
        };
    }


    [Serializable]
    public class ParticleDataSettings {
        /// <summary>
        /// Enable / Disable recording
        /// </summary>
        public bool enabled = true;
        /// <summary>
        /// Capture interval settings
        /// </summary>
        public CaptureIntervalSettings captureInterval = new CaptureIntervalSettings(30, 3, 0);

        /// <summary>
        /// A list of the possible data-providers to be included. Please disable by commenting the lines.
        /// </summary>
        public string[] activeProviders = new string[] {
            "ShurikenParticleDataProvider",
            "LegacyParticleDataProvider"
        };
    }


    [Serializable]
    public class MaterialCaptureSettings {
        /// <summary>
        /// Enable / Disable recording
        /// </summary>
        public bool enabled = true;
        /// <summary>
        /// Capture interval settings
        /// </summary>
        public CaptureIntervalSettings captureInterval = new CaptureIntervalSettings(30, 3, 0);
    }

    [Serializable]
    public class GameObjectReflectionSettings {
        /// <summary>
        /// Enable / Disable recording
        /// </summary>
        public bool enabled = true;
        /// <summary>
        /// Capture interval settings
        /// </summary>
        public CaptureIntervalSettings captureInterval = new CaptureIntervalSettings(30, 3, 2);

        /// <summary>
        /// This will also scan all static game-objects. Since these will not change, its set to 'false' by default because they are not that interesting to capture anyways.
        /// </summary>
        public bool allowStaticGameObjectScanning = false;
        /// <summary>
        /// [EXPERIMENTAL]
        /// The recursive-component reflection will (just like Unity) only capture public exposed (or flagged as [SerializeField]) fields. 
        /// This options allows you to capture properties as well, calling the 'getter' which might trigger some logic...
        /// </summary>
        public bool allowCustomClassPropertyScanning = false;
        /// <summary>
        /// When scanning nested member collections or objects recursively, it needs to end at some point, otherwise capturing will be to costly and burn to much storage-space.
        /// </summary>
        public int objectMemberScanDepth = 3;
        /// <summary>
        /// When scanning collections, we want to make sure its memory-footprint and performance-cost are kept sane. Therefore we limit the content using this value.
        /// Please increase this value if you depend on collections to be properly reflected/captured.
        /// </summary>
        public int collecionMemberMaxSize = 25;
    }


    [Serializable]
    public class ScreenCaptureSettings {
        /// <summary>
        /// Enable / Disable recording
        /// </summary>
        public bool enabled = true;
        /// <summary>
        /// Capture interval settings
        /// </summary>
        public CaptureIntervalSettings captureInterval = new CaptureIntervalSettings(30, 3, 2);
        /// <summary>
        /// The maximum x/y dimension the captured texture will be stored (and rendered when using jitRender)
        /// The lower the value, the sooner storage/rendering will be finished, drastically improving performance
        /// This also greatly influences the required space to store a single .vBugSlalice file
        /// </summary>
        public int maxScreenCaptureSize = 256;
        /// <summary>
        /// EXPERIMENTAL:
        /// This will force the used Texture2D's  & RenderTextures to be initialized at POT values only. In some situations/devices this might save a lot of performance
        /// </summary>
        public ScreenCropMethod cropMethod = ScreenCropMethod.none;

        /// <summary>
        /// Storage quality & compression settings  
        /// lzf: Fast and simlpe type of compression - save a LOT of space and thanks to multithreading not a big deal performance-wise
        /// 24 bits: 8 bits per color (256)
        /// 32 bits: 8 bits per color (256), including alpha
        /// 16 bits: 4 bits per color (16)
        /// 8 bits: 8 bits for one color (grayscale)
        /// </summary>
        public ScreenCaptureQuality quality = ScreenCaptureQuality.lzfRGBA16;
        /// <summary>
        /// The main capture method:
        ///  - endOfFrame: If you DONT have a pro-licence, or you want to capture the Unity.OnGUI drawings, this is the way to go.
        ///  - jitRender: Just-In-Time render allows for great performance if 'maxScreenSize' is kept small. This will render each Camera again, right before the capture occures.
        /// </summary>
        public ScreenCaptureMethod captureMethod = ScreenCaptureMethod.jitRender;
        /// <summary>
        /// By default set to 'ARGB32', but 'RGB565' results in better performance, especially on Android-devices. However, this will reduce the quality and it will cause some errors on certain OIS-devices
        /// </summary>
        public RenderTextureFormat rtFormat = RenderTextureFormat.ARGB32; //RenderTextureFormat.RGB565;
        /// <summary>
        /// The render-texture depth setting: 0, 16 or 24 bits
        /// </summary>
        public int rtDepth = 16;
        /// <summary>
        /// [EXPERIMENTAL]
        /// This will change the render-settings (like AA etc) to its absolute minimum right before it jitRenders a scene, and resotres it afterwards.
        /// </summary>
        public bool jitRenderLowQuality = false;
    }

}