using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace Frankfort.VBug.Internal
{
    public class UnityActivityRecorder : vBugHiddenUniqueComponent<UnityActivityRecorder>
    {
        
        //--------------------------------------- STATIC CALLS --------------------------------------
        //--------------------------------------- STATIC CALLS --------------------------------------
        #region STATIC CALLS

        public static void StartRecording()
        {
            helper.EnableRecording();
        }


        public static void StopRecording()
        {
            helper.DisableRecording();
        }

        public static bool recordingEnabled
        {
            get
            {
                return helper.recording_Enabled;
            }
        }
        #endregion
        //--------------------------------------- STATIC CALLS --------------------------------------
        //--------------------------------------- STATIC CALLS --------------------------------------










        //--------------------------------------- MONOBEHAVIOUR HELPER --------------------------------------
        //--------------------------------------- MONOBEHAVIOUR HELPER  --------------------------------------
        #region MONOBEHAVIOUR HELPER 

        private bool recording_Enabled;
        private bool disposeScheduled;

        public static float fps_ScreenCapture;
        public static float fps_MeshData;

        private Dictionary<int, VerticalActivitySlice> pendingSlices = new Dictionary<int, VerticalActivitySlice>();
        private Dictionary<int, int> waitForActivityChart = new Dictionary<int, int>();

        //--------------- Activity Grabbers --------------------
        private bool grabbersInitialized = false;
        private DebugLogsGrabber debugLogsGrabber;
        private HumanInputGrabber humanInputGrabber;
        private MeshDataGrabber meshDataGrabber;
        private ScreenGrabber screenGrabber;
        private SystemInfoGrabber systemInfoGrabber;
        private GameObjectReflectionGrabber gameObjectReflectionGrabber;
        private ParticleDataGrabber particleDataGrabber;
        private MaterialDataGrabber materialDataGrabber;
        //--------------- Activity Grabbers --------------------
			

        private void EnableRecording()
        {
            if (recording_Enabled)
                return;

            recording_Enabled = true;
            vBugEnvironment.StartNewSession();
            InitGrabbers();
        }

        
        private void DisableRecording()
        {
            if (!recording_Enabled)
                return;

            recording_Enabled = false;

            TexturePool.PruneCache();
            DisposeGrabbers();
            DisposeRecording();
        }


        private void DisposeGrabbers()
        {
            if (!grabbersInitialized)
                return;

            grabbersInitialized = false;
        }






        //--------------------------------------- INIT GRABBERS --------------------------------------
        //--------------------------------------- INIT GRABBERS --------------------------------------
        #region INIT GRABBERS

        private void InitGrabbers()
        {
            if (grabbersInitialized)
                return;

            screenGrabber               = FromObject<ScreenGrabber, ScreenCaptureSnapshot>              (vBug.settings.recording.screenCapture.captureInterval,                     OnScreenCaptureResultReady);
            debugLogsGrabber            = FromObject<DebugLogsGrabber, DebugLogSnapshot>                (vBug.settings.recording.debugLogsRecording.captureInterval,                OnDebugLogResultReady);
            humanInputGrabber           = FromObject<HumanInputGrabber, HumanInputSnapshot>             (vBug.settings.recording.humanInputRecording.captureInterval,               OnHumanInputResultReady);
            meshDataGrabber             = FromObject<MeshDataGrabber, MeshDataSnapshot>                 (vBug.settings.recording.meshDataRecording.captureInterval,                 OnMeshDataResultReady);
            systemInfoGrabber           = FromObject<SystemInfoGrabber, SystemInfoSnapshot>             (vBug.settings.recording.systemInfoRecording.captureInterval,               OnSystemInfoResultReady);
            gameObjectReflectionGrabber = FromObject<GameObjectReflectionGrabber, GameObjectsSnapshot>  (vBug.settings.recording.gameObjectReflection.captureInterval,              OnGameObjectReflectionResultReady);
            particleDataGrabber         = FromObject<ParticleDataGrabber, ParticleDataSnapshot>         (vBug.settings.recording.particleDataRecording.captureInterval,             OnParticleDataResultReady);
            materialDataGrabber         = FromObject<MaterialDataGrabber, MaterialDataSnapshot>         (vBug.settings.recording.materialDataRecording.captureInterval,             OnMaterialDataResultReady);
            grabbersInitialized = false;
        }



        public static T FromObject<T, K>(CaptureIntervalSettings intervalSettings, ResultReadyCallback<K> callback) where T : BaseActivityGrabber<K>
        {
            System.Type type = typeof(T);
            string goName = type.Name;
            GameObject newGO = new GameObject(goName);

            T grabber = newGO.GetComponent<T>();
            if (grabber == null)
                grabber = (T)newGO.AddComponent(type);

            grabber.intervalSettings = intervalSettings;
            grabber.SetResultReadyCallback(callback);
            
            GameObjectUtility.SetHideFlagsRecursively(newGO);
            return grabber;
        }


        #endregion
        //--------------------------------------- INIT GRABBERS --------------------------------------
        //--------------------------------------- INIT GRABBERS --------------------------------------
			



        private void DisposeRecording()
        {
            disposeScheduled = true;
        }


        private void Update()
        {
            int currentFrame = Time.frameCount;
            if (!ValidatePendingSlices(currentFrame))
                return;

            if (recording_Enabled)
                CaptureCurrentFrame(currentFrame);
        }



        private bool ValidatePendingSlices(int currentFrame)
        {
            if (pendingSlices == null || pendingSlices.Count == 0)
                return true;
        
            //--------------- Lifetime validation --------------------
            List<int> outOfTimeSlices = new List<int>();
            float lifeTimeSeconds = vBug.settings.general.verticalSlice.maxPendingLifetimeMS / 1000f;
            
            foreach(KeyValuePair<int, VerticalActivitySlice> pair in pendingSlices){
                if (Time.realtimeSinceStartup - pair.Value.header.birthTimeStamp > lifeTimeSeconds){
                    if (vBug.settings.general.debugMode)
                        Debug.LogWarning("Max lifetime reached! Prune frame: " + pair.Key + "\n" + CreateLogScreen(pair.Value));
                    outOfTimeSlices.Add(pair.Key); 
                }
            }

            if(outOfTimeSlices.Count > 0){
                int i = outOfTimeSlices.Count;
                while (--i > -1) {
                    waitForActivityChart.Remove(outOfTimeSlices[i]);
                    pendingSlices.Remove(outOfTimeSlices[i]);
                }
            }
            //--------------- Lifetime validation --------------------
			

            //--------------- Queue size validation --------------------
            VerticalSliceSettings settings = vBug.settings.general.verticalSlice;

            if (vBug.settings.general.verticalSlice.allowMainThreadSleep && waitForActivityChart.Count > vBug.settings.general.verticalSlice.maxPendingSleep) {
                if (vBug.settings.general.debugMode)
                    Debug.LogWarning("Max pending activities reached! sleep mainThread during frame: " + currentFrame);
                System.Threading.Thread.Sleep(settings.sleepTimeMS);
            }

            //If put to sleep, In the meantime the queue could have shrunk...
            if (waitForActivityChart.Count > vBug.settings.general.verticalSlice.maxPendingQueueSize){
                if (vBug.settings.general.debugMode)
                    Debug.LogWarning("Max pending activities [" + waitForActivityChart.Count + "] reached! Prune frame: " + currentFrame);
                return false;
            } 
            //--------------- Queue size validation --------------------

            return true;
        }


        private void CaptureCurrentFrame(int currentFrame)
        {
            GrabScreenCapture(currentFrame);
            GrabDebugLogs(currentFrame);
            GrabHumanInput(currentFrame);
            GrabMeshData(currentFrame);
            GrabSystemInfo(currentFrame);
            GrabGameObjectReflection(currentFrame);
            GrabParticleData(currentFrame);
            GrabMaterialData(currentFrame);

            //--------------- Create new Vertical slice object --------------------
            if (CheckForActivity(currentFrame))
            {
                lock (pendingSlices)
                {
                    VerticalActivitySlice currentSlice = new VerticalActivitySlice(currentFrame);
                    pendingSlices.Add(currentFrame, currentSlice);
                }
            }
            //--------------- Create new Vertical slice object --------------------
        }


        private void GrabScreenCapture(int currentFrame) {
            if (vBug.settings.recording.screenCapture.enabled) {
                if (screenGrabber != null && screenGrabber.GrabActivity(currentFrame))
                    AddActivity(currentFrame);
            }
        }

        private void GrabDebugLogs(int currentFrame)
        {
            if (vBug.settings.recording.debugLogsRecording.enabled){
                if (debugLogsGrabber != null && debugLogsGrabber.GrabActivity(currentFrame))
                    AddActivity(currentFrame);
            }
        }

        private void GrabHumanInput(int currentFrame)
        {
            if (vBug.settings.recording.humanInputRecording.enabled){
                if (humanInputGrabber != null && humanInputGrabber.GrabActivity(currentFrame))
                    AddActivity(currentFrame);
            }
        }

        private void GrabMeshData(int currentFrame)
        {
            if (vBug.settings.recording.meshDataRecording.enabled){
                if (meshDataGrabber != null && meshDataGrabber.GrabActivity(currentFrame))
                    AddActivity(currentFrame);
            }
        }

        private void GrabSystemInfo(int currentFrame) {
            if (vBug.settings.recording.systemInfoRecording.enabled){
                if (systemInfoGrabber != null && systemInfoGrabber.GrabActivity(currentFrame))
                    AddActivity(currentFrame);
            }
        }

        private void GrabGameObjectReflection(int currentFrame)
        {
            if (vBug.settings.recording.gameObjectReflection.enabled){
                if (gameObjectReflectionGrabber != null && gameObjectReflectionGrabber.GrabActivity(currentFrame))
                    AddActivity(currentFrame);
            }
        }

        private void GrabParticleData(int currentFrame)
        {
            if (vBug.settings.recording.particleDataRecording.enabled){
                if (particleDataGrabber != null && particleDataGrabber.GrabActivity(currentFrame))
                    AddActivity(currentFrame);
            }
        }

        private void GrabMaterialData(int currentFrame)
        {
            if (vBug.settings.recording.materialDataRecording.enabled){
                if (materialDataGrabber != null && materialDataGrabber.GrabActivity(currentFrame))
                    AddActivity(currentFrame);
            }
        }

        #endregion
        //--------------------------------------- MONOBEHAVIOUR HELPER  --------------------------------------
        //--------------------------------------- MONOBEHAVIOUR HELPER  --------------------------------------















        //--------------------------------------- ON RESULT READY CALLBACKS --------------------------------------
        //--------------------------------------- ON RESULT READY CALLBACKS --------------------------------------
        #region ON RESULT READY CALLBACKS


        private void OnScreenCaptureResultReady(int frameNumber, ScreenCaptureSnapshot result, int streamPriority)
        {
            //vBug.Log("OnScreenCaptureResultReady: " + frameNumber + " >" + result);
            if (pendingSlices.ContainsKey(frameNumber))
                pendingSlices[frameNumber].screenCapture = result;

            RemoveAndProcessActivity(frameNumber, streamPriority);
            if (disposeScheduled)
            {
                screenGrabber.AbortAndPruneCache();
                disposeScheduled = false;
            }
        }

        private void OnDebugLogResultReady(int frameNumber, DebugLogSnapshot result, int streamPriority)
        {
            //vBug.Log("OnDebugLogResultReady: " + frameNumber + " >" + result);
            if (pendingSlices.ContainsKey(frameNumber))
                pendingSlices[frameNumber].debugLogs = result;

            RemoveAndProcessActivity(frameNumber, streamPriority);
        }

        private void OnHumanInputResultReady(int frameNumber, HumanInputSnapshot result, int streamPriority)
        {
            //vBug.Log("OnHumanInputResultReady: " + frameNumber + " >" + result);
            if (pendingSlices.ContainsKey(frameNumber))
                pendingSlices[frameNumber].humanInput = result;

            RemoveAndProcessActivity(frameNumber, streamPriority);
        }

        private void OnMeshDataResultReady(int frameNumber, MeshDataSnapshot result, int streamPriority)
        {
            //vBug.Log("OnHumanInputResultReady: " + frameNumber + " >" + result);
            if (pendingSlices.ContainsKey(frameNumber))
                pendingSlices[frameNumber].meshData = result;

            RemoveAndProcessActivity(frameNumber, streamPriority);
        }

        private void OnSystemInfoResultReady(int frameNumber, SystemInfoSnapshot result, int streamPriority)
        {
            //vBug.Log("OnSystemInfoResultReady: " + frameNumber + " >" + result);
            if (pendingSlices.ContainsKey(frameNumber))
                pendingSlices[frameNumber].systemInfo = result;

            RemoveAndProcessActivity(frameNumber, streamPriority);
        }

        private void OnGameObjectReflectionResultReady(int frameNumber, GameObjectsSnapshot result, int streamPriority)
        {
            //vBug.Log("OnGameObjectReflectionResultReady: " + frameNumber + " >" + result);
            if (pendingSlices.ContainsKey(frameNumber))
                pendingSlices[frameNumber].gameObjectsSnapshot = result;

            RemoveAndProcessActivity(frameNumber, streamPriority);
        }

        private void OnParticleDataResultReady(int frameNumber, ParticleDataSnapshot result, int streamPriority)
        {
            //vBug.Log("OnParticleDataResultReady: " + frameNumber + " >" + result);
            if (pendingSlices.ContainsKey(frameNumber))
                pendingSlices[frameNumber].particleData = result;

            RemoveAndProcessActivity(frameNumber, streamPriority);
        }

        private void OnMaterialDataResultReady(int frameNumber, MaterialDataSnapshot result, int streamPriority)
        {
            //vBug.Log("OnMaterialDataResultReady: " + frameNumber + " >" + result);
            if (pendingSlices.ContainsKey(frameNumber))
                pendingSlices[frameNumber].materialData = result;

            RemoveAndProcessActivity(frameNumber, streamPriority);
        }

        


        #endregion
        //--------------------------------------- ON RESULT READY CALLBACKS --------------------------------------
        //--------------------------------------- ON RESULT READY CALLBACKS --------------------------------------














        //--------------------------------------- WAIT FOR ACTIVITY --------------------------------------
        //--------------------------------------- WAIT FOR ACTIVITY --------------------------------------
        #region WAIT FOR ACTIVITY

        private void AddActivity(int frameNumber)
        {
            if (!waitForActivityChart.ContainsKey(frameNumber)){
                waitForActivityChart.Add(frameNumber, 1);
            } else {
                waitForActivityChart[frameNumber]++;
            }
        }


        private void RemoveAndProcessActivity(int frameNumber, int streamPriority)
        {
            if (!waitForActivityChart.ContainsKey(frameNumber)) {
                if (vBug.settings.general.debugMode)
                    Debug.LogError("no activity found: " + frameNumber);
                return;
            }
            waitForActivityChart[frameNumber]--;

            //--------------- Slice complete! --------------------
            if (waitForActivityChart[frameNumber] <= 0) {
                waitForActivityChart.Remove(frameNumber);
                VerticalActivitySlice slice = null;
                lock (pendingSlices) {
                    if (!pendingSlices.ContainsKey(frameNumber)) {
                        return;
                    } else {
                        slice = pendingSlices[frameNumber];
                        pendingSlices.Remove(frameNumber);
                    }
                }

                if (slice != null)  {
                    slice.streamPriority = streamPriority;
                    DataProcessingManager.ProcessData(slice);//TAKES A WHILE!!!!
                }
            } 
            //--------------- Slice complete! --------------------
			
        }


        private bool CheckForActivity(int frameNumber)
        {
            return waitForActivityChart.ContainsKey(frameNumber) && waitForActivityChart[frameNumber] > 0;
        }

        #endregion
        //--------------------------------------- WAIT FOR ACTIVITY --------------------------------------
        //--------------------------------------- WAIT FOR ACTIVITY --------------------------------------










        //--------------------------------------- MISC --------------------------------------
        //--------------------------------------- MISC --------------------------------------
        #region MISC


        private string CreateLogScreen(VerticalActivitySlice slice) {
            string logString = "";
            if (vBug.settings.recording.screenCapture.enabled && slice.screenCapture == null) {
                logString += "\n   no screenCapture found";
            } else {
                if (slice == null)
                    logString += "\n   no screenCapture data found";
            }
            if (vBug.settings.recording.gameObjectReflection.enabled && slice.gameObjectsSnapshot == null) {
                logString += "\n   no gameObjectReflectionData found";
            }
            if (vBug.settings.recording.debugLogsRecording.enabled && slice.debugLogs == null) {
                logString += "\n   no debugLogsData found";
            }
            if (vBug.settings.recording.systemInfoRecording.enabled && slice.systemInfo == null) {
                logString += "\n   no systemInfoData found";
            }
            if (vBug.settings.recording.humanInputRecording.enabled && slice.humanInput == null) {
                logString += "\n   no humanInputData found";
            }
            if (vBug.settings.recording.particleDataRecording.enabled && slice.particleData == null) {
                logString += "\n   no particleData found";
            }
            if (vBug.settings.recording.materialDataRecording.enabled && slice.materialData == null) {
                logString += "\n   no materialData found";
            }
            return logString;
        }

        #endregion
        //--------------------------------------- MISC --------------------------------------
        //--------------------------------------- MISC --------------------------------------
			
    }
}