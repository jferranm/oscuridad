using System;
using System.Collections;
using UnityEngine;


namespace Frankfort.VBug.Internal {
    [AddComponentMenu("")]
    public class BaseActivityGrabber<T> : MonoBehaviour
    {

        //--------------------------------------- STATIC --------------------------------------
        //--------------------------------------- STATIC --------------------------------------
        #region STATIC

        public static float deltaRecordTime = 0f;
        public CaptureIntervalSettings intervalSettings;
        private int captureFrameNumber = -1;
        private float captureTimeStamp = -1;
        
        protected ResultReadyCallback<T> resultReadyCallback;
        protected T currentGrabbedResult;
        protected bool currentFrameActive;
        protected int currentFrame;
        protected string currentLevelName;

        public bool isAlive { get; private set; }
        public bool isDisposed { get; private set; }
        public bool levelChanged { get; private set; }

        #endregion
        //--------------------------------------- STATIC --------------------------------------
        //--------------------------------------- STATIC --------------------------------------









        //--------------------------------------- MONOBEHAVIOUR --------------------------------------
        //--------------------------------------- MONOBEHAVIOUR --------------------------------------
        #region MONOBEHAVIOUR

        protected virtual void Start(){
            isAlive = true;
            StartCoroutine(UpdateEndOfFrame());
        }

        protected virtual void OnApplicationQuit() {
            UnityEngine.GameObject.DestroyImmediate(this.gameObject);
            Dispose();
        }

        protected virtual void OnDestroy(){
            Dispose();
        }

        protected virtual void Dispose() {
            if (isDisposed)
                return;

            isAlive = false;
            AbortAndPruneCache();
            isDisposed = true;
        }

        public virtual void AbortAndPruneCache() {
            captureTimeStamp = -1;
            captureFrameNumber = -1;
            currentGrabbedResult = default(T);
        }



        public void SetResultReadyCallback(ResultReadyCallback<T> callback){
            this.resultReadyCallback = callback;
        }

        protected virtual void GrabResultEndOfFrame()
        { }


        public virtual bool GrabActivity(int currentFrame){
            this.currentFrame = currentFrame;
            this.currentFrameActive = false;

            if (this.currentLevelName != Application.loadedLevelName) {
                levelChanged = true;
                this.currentLevelName = Application.loadedLevelName;
            } else {
                levelChanged = false;
            }
            
            if (intervalSettings == null){
                //vBug.Log("captureIntervalSettings NULL");
                return false;
            }
            if (!CheckRecordInterval()){
                //vBug.Log("skip:  " + currentFrame);
                return false;
            }

            if (this.resultReadyCallback == null){
                //vBug.LogError(this.GetType().Name + " Error: Please set the 'resultReadyCallback' first!");
                return false;
            }

            //vBug.Log("do it!:  " + currentFrame);
            this.currentFrameActive = true;
            return true;
        }


        private IEnumerator UpdateEndOfFrame() {
            while (isAlive){
                yield return new WaitForEndOfFrame();
                if (currentFrameActive)
                {
                    float time = Time.realtimeSinceStartup;
                    GrabResultEndOfFrame();           
                    deltaRecordTime = Time.realtimeSinceStartup - time;
                    
                    currentFrameActive = false;
                }
            }
        }


        protected bool CheckRecordInterval()
        {
            if (intervalSettings == null)
                return true;

            float maxDelta = intervalSettings.targetInterval <= 0 ? 0 : 1f / intervalSettings.maxFrameRate;
            int frameCount = currentFrame - intervalSettings.frameOffset;
            int frameDiff = frameCount - captureFrameNumber;
            float time = Time.realtimeSinceStartup;

            if (captureTimeStamp == -1f) {
                captureTimeStamp = time;
                return true; //makes sure the first frame is always captured!
            }

            if(captureFrameNumber == -1) {
                captureFrameNumber = currentFrame;
                return true; //makes sure the first frame is always captured!
            }

            float timeDiff = time - captureTimeStamp;
            if (frameDiff >= intervalSettings.targetInterval && timeDiff > maxDelta) {
                captureFrameNumber = frameCount;
                captureTimeStamp = time;
                return true;
            }
            return false;
        }

        #endregion
        //--------------------------------------- MONOBEHAVIOUR --------------------------------------
        //--------------------------------------- MONOBEHAVIOUR --------------------------------------
			
    }
}
