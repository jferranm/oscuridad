using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using UnityEngineInternal;
using System.Runtime.InteropServices;
using System.Reflection;
using System;



namespace Frankfort.VBug.Internal
{
    
    public class ScreenGrabber : BaseActivityGrabber<ScreenCaptureSnapshot>
    {
        private List<ScreenCaptureRequest> requestedCaptures = new List<ScreenCaptureRequest>();
        private Camera jitRenderCam;
        private bool running = false;


        protected override void Start()
        {
            base.Start();
            StartCoroutine(OnEndOfFrame());
        }

        public override void AbortAndPruneCache() {
            base.AbortAndPruneCache();
            if (!running)
                return;

            if (requestedCaptures != null && requestedCaptures.Count > 0) {
                lock (requestedCaptures) {
                    foreach (ScreenCaptureRequest request in requestedCaptures)
                        request.Dispose();

                    requestedCaptures.Clear();
                }
            }

            if (jitRenderCam != null) {
                DestroyImmediate(jitRenderCam.gameObject);
                jitRenderCam = null;
            }
            running = false;
        }





        public override bool GrabActivity(int currentFrame)
        {
            if (base.GrabActivity(currentFrame))
            {
                //--------------- Check untiy licence first --------------------
                if (vBug.settings.recording.screenCapture.captureMethod == ScreenCaptureMethod.jitRender && !Application.HasProLicense()) {
                    Debug.LogWarning("User does not have a Pro-licence, therefore 'Just-In-Time' rendering is not supported... vBug will now switch to 'EndOfFrame' capturing, this may cause additional performance costs.");
                    vBug.settings.recording.screenCapture.captureMethod = ScreenCaptureMethod.endOfFrame;
                }
                //--------------- Check untiy licence first --------------------
			

                //--------------- Calculate optimal texturesize --------------------
                int max = Math.Min(Math.Max(Screen.height, Screen.width), vBug.settings.recording.screenCapture.maxScreenCaptureSize);
                int width = Screen.width;
                int height = Screen.height;
                if (width > height) {
                    width = max;
                    height = (int)((float)height * ((float)max / (float)Screen.width));
                }  else if (height > width) {
                    height = max;
                    width = (int)((float)width * ((float)max / (float)Screen.height));
                }
                //--------------- Calculate optimal texturesize --------------------


                //--------------- POT --------------------
                if (vBug.settings.recording.screenCapture.cropMethod != ScreenCropMethod.none){
                    width = MathHelper.MakePowerOfTwo(width);
                    height = MathHelper.MakePowerOfTwo(height);

                    if (width != height && vBug.settings.recording.screenCapture.cropMethod == ScreenCropMethod.squarePOT)
                        width = height = Mathf.Max(width, height);
                }
                //--------------- POT --------------------


                //--------------- Collect all active camera's --------------------
                List<Camera> activeCams = new List<Camera>();
                if (vBug.settings.recording.screenCapture.captureMethod == ScreenCaptureMethod.endOfFrame) {
                    activeCams.Add(Camera.main);
                } else {
                    foreach (Camera cam in Camera.allCameras){
                        if (cam.gameObject.activeInHierarchy && cam.enabled && !cam.name.Contains("vBug"))
                            activeCams.Add(cam);
                    }

                    activeCams.Sort((a, b) => a.depth.CompareTo(b.depth));
                }
                //--------------- Collect all active camera's --------------------

                requestedCaptures.Add(new ScreenCaptureRequest(activeCams.ToArray(), width, height, vBug.settings.recording.screenCapture.quality));
                running = true;

                return true;
            }
            return false;
        }






        //--------------------------------------- UPDATE LOOPS --------------------------------------
        //--------------------------------------- UPDATE LOOPS --------------------------------------
        #region UPDATE LOOPS

        private IEnumerator OnEndOfFrame()
        {
            while (isAlive)
            {
                yield return new WaitForEndOfFrame();
                if (requestedCaptures.Count > 0)
                {
                    ScreenCaptureMethod method = vBug.settings.recording.screenCapture.captureMethod;
                    if (method == ScreenCaptureMethod.endOfFrame)
                    {
                        GrabPixelsEndOfFrame();
                    }
                    else if (method == ScreenCaptureMethod.jitRender)
                    {
                        GrabJitRender();
                    }
                }
            }
        }

        #endregion
        //--------------------------------------- UPDATE LOOPS --------------------------------------
        //--------------------------------------- UPDATE LOOPS --------------------------------------
			







        //--------------------------------------- SCREENSHOT GRABBERS --------------------------------------
        //--------------------------------------- SCREENSHOT GRABBERS --------------------------------------
        #region SCREENSHOT GRABBERS


        private ScreenCaptureRequest PopNextRequestData(){
            ScreenCaptureRequest requestData = requestedCaptures[0];
            requestedCaptures.RemoveAt(0);
            requestData.frameNumber = Time.frameCount;
            requestData.cycleID = WorkloadDistributionManager.GetNextAvailableCycle();
            return requestData;
        }


        private void GrabPixelsEndOfFrame(){
            ScreenCaptureRequest requestData = PopNextRequestData();
            if(requestData.renderLayers == null || requestData.renderLayers.Length == 0) {
                requestData.state = ScreenCaptureRequest.State.complete;
            } else {
                requestData.renderLayers[0].destTexture = TexturePool.GetTexture2D(Screen.width, Screen.height);
                requestData.renderLayers[0].destTexture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0, false);
                requestData.scaleDown = (int)(Mathf.Max(Mathf.Ceil((float)Screen.width / (float)requestData.resultWidth), Mathf.Ceil((float)Screen.height / (float)requestData.resultHeight)));
                requestData.resultWidth = Screen.width;
                requestData.resultHeight = Screen.height;

                requestData.state = ScreenCaptureRequest.State.getColors;
            }    
            HandleRequestState(requestData);
        }


        private void GrabJitRender()
        { 
            ScreenCaptureRequest requestData = PopNextRequestData();

            if (requestData != null && requestData.renderCams != null)
            {
                int iMax = requestData.renderCams.Length;

                for (int i = 0; i < iMax; i++)
                {
                    Camera cam = requestData.renderCams[i];
                    if (cam == null)
                        continue;

                    if (jitRenderCam == null)
                        jitRenderCam = InitCam();

                    jitRenderCam.CopyFrom(cam);
                    jitRenderCam.clearFlags = CameraClearFlags.Color;
                    jitRenderCam.backgroundColor = Color.clear;

                    ScreenCaptureRequest.RenderLayer layer = requestData.renderLayers[i];
                    if (layer == null)
                        continue;

                    layer.sourceTexture = TexturePool.GetRenderTexture(requestData.resultWidth, requestData.resultHeight, vBug.settings.recording.screenCapture.rtFormat);
                    layer.sourceTexture.name = "vBugRescaleBuffer";
                    layer.sourceTexture.hideFlags = HideFlags.HideAndDontSave;

                    if (vBug.settings.recording.screenCapture.jitRenderLowQuality){
                        RenderSettingsHelper.StoreRenderQuality();
                        RenderSettingsHelper.SetLowRenderQuality();
                    }

                    jitRenderCam.targetTexture = layer.sourceTexture;
                    jitRenderCam.Render();

                    if (vBug.settings.recording.screenCapture.jitRenderLowQuality)
                        RenderSettingsHelper.RestoreRenderQuality();
                }
                requestData.state = ScreenCaptureRequest.State.readPixels;
            } else {
                requestData.state = ScreenCaptureRequest.State.complete;
            }

            HandleRequestState(requestData);
        }


        private Camera InitCam() {
            GameObject camGO = new GameObject("vBugRenderCam");
            Camera renderCam = camGO.AddComponent<Camera>();
            renderCam.enabled = false;
            GameObjectUtility.SetHideFlagsRecursively(camGO);
            return renderCam;
        }


        #endregion
        //--------------------------------------- SCREENSHOT GRABBERS --------------------------------------
        //--------------------------------------- SCREENSHOT GRABBERS --------------------------------------













        //--------------------------------------- ASYNC OPERATIONS --------------------------------------
        //--------------------------------------- ASYNC OPERATIONS --------------------------------------
        #region ASYNC OPERATIONS


        private void HandleRequestState(IAbortableWorkObject requestDataObj)
        {
            ScreenCaptureRequest requestData = (ScreenCaptureRequest)requestDataObj;
            switch (requestData.state)
            {
                case ScreenCaptureRequest.State.readPixels:
                    WorkloadDistributionManager.AddWork(requestData.cycleID, WorkloadExecutorType.waitForEndOfFrame, Execute_readPixels, requestData, HandleRequestState);
                    break;
                case ScreenCaptureRequest.State.getColors:
                    WorkloadDistributionManager.AddWork(requestData.cycleID, WorkloadExecutorType.lateUpdate, Execute_getColors, requestData, HandleRequestState);
                    break;
                case ScreenCaptureRequest.State.getBytes:
                    WorkloadDistributionManager.AddWork(requestData.cycleID, WorkloadExecutorType.thread, Execute_getBytes, requestData, HandleRequestState);
                    break;
                case ScreenCaptureRequest.State.complete:
                    WorkloadDistributionManager.AddWork(requestData.cycleID, WorkloadExecutorType.fixedUpdate, Execute_complete, requestData, HandleRequestState);
                    break;
            }
        }


        private bool Execute_readPixels(IAbortableWorkObject requestDataObj)
        {
            if (requestDataObj == null || requestDataObj.isAborted)
                return false;

            ScreenCaptureRequest requestData = (ScreenCaptureRequest)requestDataObj;
            //vBug.Log("1 readPixels: " + requestData.frameNumber + ", cycleID: " + requestData.cycleID + ", currentFrame " + Time.frameCount); 
            requestData.state = ScreenCaptureRequest.State.getColors; // Move to nextState before any crashes can occure

            if (requestData.renderLayers != null)
            {
                foreach (ScreenCaptureRequest.RenderLayer layer in requestData.renderLayers)
                {
                    RenderTexture restore = RenderTexture.active;
                    RenderTexture.active = layer.sourceTexture;

                    layer.destTexture = TexturePool.GetTexture2D(layer.sourceTexture.width, layer.sourceTexture.height);
                    layer.destTexture.ReadPixels(new Rect(0, 0, layer.sourceTexture.width, layer.sourceTexture.height), 0, 0, false);

                    RenderTexture.active = restore;
                    layer.sourceTexture.DiscardContents(true, true);
                    TexturePool.StoreRenderTexture(layer.sourceTexture);
                }
            }
            else
            {
                if (vBug.settings.general.debugMode)
                    Debug.LogError("readPixels ERROR: sourceTexture NULL!");
                requestData.state = ScreenCaptureRequest.State.complete;
            }
            return true;
        }

        private bool Execute_getColors(IAbortableWorkObject requestDataObj)
        {
            if (requestDataObj == null || requestDataObj.isAborted)
                return false;

            ScreenCaptureRequest requestData = (ScreenCaptureRequest)requestDataObj;
            requestData.state = ScreenCaptureRequest.State.getBytes; // Move to nextState before any crashes can occure
            if (requestData.renderLayers != null)
            {
                foreach (ScreenCaptureRequest.RenderLayer layer in requestData.renderLayers)
                    layer.colorBuffer = layer.destTexture.GetPixels32();
            }
            else
            {
                requestData.state = ScreenCaptureRequest.State.complete;
            }
            return true;
        }


        private bool Execute_getBytes(IAbortableWorkObject requestDataObj)
        {
            if (requestDataObj == null || requestDataObj.isAborted)
                return false;

            ScreenCaptureRequest requestData = (ScreenCaptureRequest)requestDataObj;
            requestData.state = ScreenCaptureRequest.State.complete; // Move to nextState before any crashes can occure

            foreach (ScreenCaptureRequest.RenderLayer layer in requestData.renderLayers)
            {
                if (layer.colorBuffer != null) {
                    layer.byteBuffer = TextureHelper.GetBytes(layer.colorBuffer, requestData.resultWidth, requestData.resultHeight, requestData.scaleDown, requestData.quality);
                } else {
                    if (vBug.settings.general.debugMode)
                        Debug.LogError("getBytes ERROR! colorBuffer NULL! " + requestData.cycleID);
                }
            }
            return true;
        }


        private bool Execute_complete(IAbortableWorkObject requestDataObj)
        {
            if (requestDataObj == null || requestDataObj.isAborted)
                return false;

            ScreenCaptureRequest requestData = (ScreenCaptureRequest)requestDataObj;
            requestData.state = ScreenCaptureRequest.State.idle; // Move to nextState before any crashes can occure

            if (resultReadyCallback != null)
                resultReadyCallback(requestData.frameNumber, SetSnapshot(requestData), 0);

            requestData.Dispose();
            return true;
        }

        private ScreenCaptureSnapshot SetSnapshot(ScreenCaptureRequest requestData)
        {
            if (requestData == null || requestData.renderCams == null || requestData.renderLayers == null)
                return null;

            int iMax = requestData.renderCams.Length;
            string[] camNames = new string[iMax];
            byte[][] camRenders = new byte[iMax][];
            Rect[] camRects = new Rect[iMax];
            for(int i = 0; i < iMax; i++)
            {
                camNames[i] = requestData.renderCams[i].name;
                camRects[i] = requestData.renderCams[i].pixelRect;
                camRenders[i] = requestData.renderLayers[i].byteBuffer;
            }
            return new ScreenCaptureSnapshot(camNames, camRenders, camRects, Camera.main);
        }

        #endregion
        //--------------------------------------- ASYNC OPERATIONS --------------------------------------
        //--------------------------------------- ASYNC OPERATIONS --------------------------------------

	
    }
}