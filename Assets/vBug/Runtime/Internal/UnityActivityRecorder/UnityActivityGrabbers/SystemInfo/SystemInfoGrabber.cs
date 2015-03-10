using System;
using UnityEngine;
using System.Collections.Generic;
using System.Reflection;
using System.Collections;


namespace Frankfort.VBug.Internal
{
    public class SystemInfoGrabber : BaseActivityGrabber<SystemInfoSnapshot>
    {

        private float updateFps;
        private float avUpdateFps;
        private float fixedFps;

        private List<float> avUpdateFpsTimestamps = new List<float>();
        private List<float> avFixedFpsTimestamps = new List<float>();

        private float updateTimestamp;
        private float fixedTimestamp;

        private bool doDisableProfiler;
        private int memoryUpdateCount = 0;
        private bool isFirstTime = true;


        
        protected override void Start(){
 	        base.Start();
            isFirstTime = true;
            if (!Profiler.enabled) //It was turned off by user, so lets not leave it on when we are not recording.
                doDisableProfiler = true;
        }

        public override void AbortAndPruneCache()
        {
            base.AbortAndPruneCache();
            if (doDisableProfiler && Profiler.supported)
                Profiler.enabled = false;

            memoryUpdateCount = 0;
        }








        //--------------------------------------- FPS Measurement --------------------------------------
        //--------------------------------------- FPS Measurement --------------------------------------
        #region FPS Measurement

        private void Update()
        {
            float time = Time.realtimeSinceStartup;
        
            //--------------- Average FPS --------------------
            RemoveOldTimestamps(avUpdateFpsTimestamps, time);
            RemoveOldTimestamps(avFixedFpsTimestamps, time);

            avUpdateFps = avUpdateFpsTimestamps.Count;
            fixedFps = avFixedFpsTimestamps.Count;
            //--------------- Average FPS --------------------

            //--------------- Both fps, based on deltaTime --------------------
            avUpdateFpsTimestamps.Add(time);
            updateFps = 1f / Time.deltaTime;
            //--------------- Both fps, based on deltaTime --------------------
        }


        private void FixedUpdate()
        {
            avFixedFpsTimestamps.Add(Time.realtimeSinceStartup);
        }


        private void RemoveOldTimestamps(List<float> timeStamps, float currentTime)
        {
            int i = timeStamps.Count;
            while (--i > -1)
            {
                if (currentTime - timeStamps[i] > 1d) //if timestamp got set more than 1 second ago.
                    timeStamps.RemoveAt(i);
            }
        }


        #endregion
        //--------------------------------------- FPS Measurement --------------------------------------
        //--------------------------------------- FPS Measurement --------------------------------------








        //--------------------------------------- SYSTEM FRAME INFO --------------------------------------
        //--------------------------------------- SYSTEM FRAME INFO --------------------------------------
        #region SYSTEM FRAME INFO

        protected override void GrabResultEndOfFrame() {
            base.GrabResultEndOfFrame();
            if (resultReadyCallback != null)
                resultReadyCallback(currentFrame, CollectSystemFrameInfo(), 0);
        }


        public SystemInfoSnapshot CollectSystemFrameInfo()
        {
            SystemInfoSnapshot result = new SystemInfoSnapshot();
            result.updateFps    = updateFps;
            result.fixedFps     = fixedFps;
            result.averageUpdateFps = avUpdateFps;

            if (isFirstTime || memoryUpdateCount % vBug.settings.recording.systemInfoRecording.memoryUpdateInterval == 0) {
                isFirstTime = false;
                
                if (Profiler.supported && !Profiler.enabled && vBug.settings.recording.systemInfoRecording.autoEnableProfiler)
                    Profiler.enabled = true;

                if (Profiler.supported && Profiler.enabled) {
                    result.memoryProfiled = true; //not a bool perf reasons
                    result.usedHeapSize = Profiler.usedHeapSize;
                    result.gcTotalSize = System.GC.GetTotalMemory(vBug.settings.recording.systemInfoRecording.gcForceFullCollection);

                    if (vBug.settings.recording.systemInfoRecording.recordTexture2Ds) {
                        foreach (Texture2D obj in Resources.FindObjectsOfTypeAll<Texture2D>()) {
                            result.activeTextures++;
                            result.activeTextureMemory += Profiler.GetRuntimeMemorySize(obj);
                            result.runtimeMemory += result.activeTextureMemory;
                        }
                    }
                    if (vBug.settings.recording.systemInfoRecording.recordRenderTextures) {
                        foreach (RenderTexture obj in Resources.FindObjectsOfTypeAll<RenderTexture>()) {
                            result.activeRenderTextures++;
                            result.activeRenderTextureMemory += Profiler.GetRuntimeMemorySize(obj);
                            result.runtimeMemory += result.activeRenderTextureMemory;
                        }
                    }
                    if (vBug.settings.recording.systemInfoRecording.recordMaterials) {
                        foreach (Material obj in Resources.FindObjectsOfTypeAll<Material>()) {
                            result.activeMaterials++;
                            result.activeMaterialsMemory += Profiler.GetRuntimeMemorySize(obj);
                            result.runtimeMemory += result.activeMaterialsMemory;
                        }
                    }
                    if (vBug.settings.recording.systemInfoRecording.recordMeshes) {
                        foreach (Mesh obj in Resources.FindObjectsOfTypeAll<Mesh>()) {
                            result.activeMeshes++;
                            result.activeMeshMemory += Profiler.GetRuntimeMemorySize(obj);
                            result.runtimeMemory += result.activeMeshMemory;
                        }
                    }
                    if (vBug.settings.recording.systemInfoRecording.recordAnimations) {
                        foreach (Animation obj in Resources.FindObjectsOfTypeAll<Animation>()) {
                            result.activeAnimations++;
                            result.activeAnimationMemory += Profiler.GetRuntimeMemorySize(obj);
                            result.runtimeMemory += result.activeAnimationMemory;
                        }
                    }
                    if (vBug.settings.recording.systemInfoRecording.recordAudioClips) {
                        foreach (AudioClip obj in Resources.FindObjectsOfTypeAll<AudioClip>()) {
                            result.activeAudioClips++;
                            result.activeAudioClipMemory += Profiler.GetRuntimeMemorySize(obj);
                            result.runtimeMemory += result.activeAudioClipMemory;
                        }
                    }
                }
            }

            memoryUpdateCount ++;
            return result;
        }

        #endregion
        //--------------------------------------- SYSTEM FRAME INFO --------------------------------------
        //--------------------------------------- SYSTEM FRAME INFO --------------------------------------

    }
}