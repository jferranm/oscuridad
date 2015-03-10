using System;
using System.Collections.Generic;
using UnityEngine;
using Frankfort.VBug;
using Frankfort.VBug.Internal;
using System.Threading;

namespace Frankfort.VBug.Editor
{
    public static class VerticalSliceDatabase
    {
        public class VerticalSlicePointer
        {
            public enum State {
                idle,
                requested,
                loading,
                ready
            }

            public int frameNumber;
            public State state = State.idle;
            public double requestTimeStamp;
            public string path;
            public VerticalActivitySlice slice;

            public VerticalSlicePointer(int frameNumber, string path)
            {
                this.frameNumber = frameNumber;
                this.path = path;
            }

            public void Dispose()
            {
                if (slice != null)
                    slice.Dispose();

                slice = null;
                path = null;
            }
        }

        public static bool isInitialized { get; private set; }
        public static int minRange { get; private set; }
        public static int maxRange { get; private set; }
        public static bool autoGC = true;

        private static int identifier = int.MinValue;
        private static Dictionary<int, VerticalSlicePointer> verticalSlicePointers = new Dictionary<int, VerticalSlicePointer>();
        private static VerticalActivitySlice[] availableSlices; //Set dirty eachtime the slices change!
        private static List<VerticalActivitySlice> loadBundle = new List<VerticalActivitySlice>();
        
        //--------------- Threading --------------------
        private static Thread loaderThread;
        private static double threadStartTimeStamp;
        private static bool readyForNotification = false;
        //--------------- Threading --------------------
		

        public static void Reset()
        {
            minRange = 0;
            maxRange = 0;

            foreach (KeyValuePair<int, VerticalSlicePointer> pair in verticalSlicePointers)
                pair.Value.Dispose();

            TerminateLoaderThread();
            verticalSlicePointers.Clear();
            availableSlices = null;
            isInitialized = false;
            loadBundle.Clear();
            GC.Collect();
        }












        //--------------------------------------- EXTERNAL UPDATE LOOP --------------------------------------
        //--------------------------------------- EXTERNAL UPDATE LOOP --------------------------------------
        #region EXTERNAL UPDATE LOOP

        public static void Update() {
            if (loadBundle == null || loadBundle.Count == 0)
                return;

            if (loadBundle.Count >= vBugEditorSettings.BackgroundLoadTheadBundleSize || (loadBundle.Count > 0 && readyForNotification)) {
                readyForNotification = false;

                VerticalActivitySlice[] notify = null;
                lock (loadBundle) {
                    notify = loadBundle.ToArray();
                    loadBundle.Clear();
                }

                availableSlices = null;
                if (autoGC)
                    GC.Collect();

                vBugWindowMediator.NotifyVerticalSliceBundleLoaded(notify);
            }
        }


        #endregion
        //--------------------------------------- EXTERNAL UPDATE LOOP --------------------------------------
        //--------------------------------------- EXTERNAL UPDATE LOOP --------------------------------------
			













        //--------------------------------------- LOADER THREAD --------------------------------------
        //--------------------------------------- LOADER THREAD --------------------------------------
        #region LOADER THREAD

        /// <summary>
        /// A request never comes alone. Once a missing frame is requested, a whole bunch of frames (called a bundle) is loaded because you can be sertain they are needed anyways.
        /// </summary>
        /// <param name="frameNumber"></param>
        private static void AddLoadRequest(int frameNumber) {
            if (frameNumber < minRange || frameNumber > maxRange)
                return;

            int halfRange = Mathf.Max(1, vBugEditorSettings.BackgroundLoadTheadBundleSize / 2);
            int startRange = frameNumber - halfRange;
            int endRange = frameNumber + halfRange;

            foreach (KeyValuePair<int, VerticalSlicePointer> pair in verticalSlicePointers) {
                if (pair.Key >= startRange && pair.Key < endRange && pair.Value.state == VerticalSlicePointer.State.idle) {
                    pair.Value.state = VerticalSlicePointer.State.requested;
                    pair.Value.requestTimeStamp = vBugEnvironment.GetUnixTimestamp();
                }
            }

            availableSlices = null; //Set to dirty
            InitLoaderThread();
        }


        private static void InitLoaderThread() {
            if (loaderThread != null)
                return;

            loaderThread = new Thread(HandleThreadedLoading);
            loaderThread.IsBackground = true;
            loaderThread.Priority = System.Threading.ThreadPriority.BelowNormal;
            threadStartTimeStamp = vBugEnvironment.GetUnixTimestamp();
            loaderThread.Start();
        }


        private static void TerminateLoaderThread()
        {
            if (loaderThread != null && loaderThread.IsAlive && loaderThread.ThreadState == ThreadState.Running) {
                loaderThread.Interrupt();
                loaderThread.Join(100);
                if (vBugEditorSettings.DebugMode)
                    Debug.LogWarning("Close VerticalSliceDatabase thread");
            }
            loaderThread = null;
        }


        private static void HandleThreadedLoading() {

            double myStartTimeStamp = threadStartTimeStamp;

            while (isInitialized) {
                
                if (threadStartTimeStamp != myStartTimeStamp) {
                    if (vBugEditorSettings.DebugMode)
                        Debug.LogError("Timestamps do not match! Editor windows where reinitialized while this thread remained active..." );
                    
                    break;
                }

                VerticalSlicePointer[] unProcessed = GetRequestedPointers(vBugEditorSettings.BackgroundLoadTheadBundleSize);
                if (unProcessed.Length > 0) {

                    readyForNotification = false;
                    foreach (VerticalSlicePointer pointer in unProcessed) {
                        lock (pointer) {
                            
                            //--------------- Load --------------------
                            pointer.state = VerticalSlicePointer.State.loading;
                            pointer.slice = IOEditorHelper.LoadVBugSliceFromDisk(pointer.path);
                            //--------------- Load --------------------

                            if (pointer.slice == null) {
                                pointer.state = VerticalSlicePointer.State.requested; //back to requested... lets wait for it to be ready!
                                if (vBugEditorSettings.DebugMode)
                                    Debug.LogError("pointer.slice == null @ " + pointer.frameNumber + " [" + pointer.path + "]");

                            } else {
                                pointer.state = VerticalSlicePointer.State.ready;
                                lock (loadBundle)
                                    loadBundle.Add(pointer.slice);
                            }
                        }
                    }
                    readyForNotification = true;
                }

                //TODO: find a way to notify windows within the main-thread.
                Thread.Sleep(vBugEditorSettings.BackgroundLoadThreadSleepTime);
            }

            if (vBugEditorSettings.DebugMode)
                Debug.Log("vBugEditor: VerticalSliceDatabase LoaderThread terminated!");
        }



        private static VerticalSlicePointer[] GetRequestedPointers(int bundleSize) {
            List<VerticalSlicePointer> result = new List<VerticalSlicePointer>();

            lock (verticalSlicePointers) {
                foreach (KeyValuePair<int, VerticalSlicePointer> pair in verticalSlicePointers) {
                    if (pair.Value.state == VerticalSlicePointer.State.requested)
                        result.Add(pair.Value);
                }
            }

            result.Sort((a, b) => a.requestTimeStamp.CompareTo(b.requestTimeStamp));
            if (result.Count > bundleSize)
                result.RemoveRange(bundleSize, result.Count - bundleSize);

            return result.ToArray();
        }



        #endregion
        //--------------------------------------- LOADER THREAD --------------------------------------
        //--------------------------------------- LOADER THREAD --------------------------------------
			




			








        //--------------------------------------- SET POINTER --------------------------------------
        //--------------------------------------- SET POINTER --------------------------------------
        #region SET POINTER

        public static void SetSliceLocations(string[] paths) {
            Reset();
            if (paths == null || paths.Length == 0)
                return;

            int i = paths.Length;
            while (--i > -1)
                AddVerticalSlicePointer(paths[i]);

            isInitialized = true;
            availableSlices = null;
            vBugWindowMediator.NotifySessionChange(minRange, identifier);
        }


        public static void AddVerticalSlicePointer(string path) {
            if (string.IsNullOrEmpty(path))
                return;

            int startIdx = path.LastIndexOf("/") + 1;
            int lastIdx = path.LastIndexOf(".");
            if (startIdx == -1 || lastIdx == -1 || lastIdx < startIdx)
                return;

            string fileName = path.Substring(startIdx, lastIdx - startIdx);
            int frameNumber = 0;

            if (!int.TryParse(fileName, out frameNumber))
                return;

            if (minRange == 0 || frameNumber < minRange)
                minRange = frameNumber;

            if (maxRange == 0 || frameNumber > maxRange)
                maxRange = frameNumber;

            verticalSlicePointers.Add(frameNumber, new VerticalSlicePointer(frameNumber, path));
        }

        #endregion
        //--------------------------------------- SET POINTER --------------------------------------
        //--------------------------------------- SET POINTER --------------------------------------











        //--------------------------------------- GET SLICES --------------------------------------
        //--------------------------------------- GET SLICES --------------------------------------
        #region GET SLICES



        public static VerticalActivitySlice GetSlice(int frameNumber) {
            if (!isInitialized)
                return null;

            if (!verticalSlicePointers.ContainsKey(frameNumber))
                return null;

            
            VerticalSlicePointer pointer = verticalSlicePointers[frameNumber];
            if (pointer.state == VerticalSlicePointer.State.ready) {
                if (pointer.slice != null)
                    return pointer.slice;
            } else if (pointer.state == VerticalSlicePointer.State.idle){
                AddLoadRequest(frameNumber);
            }
            return null;
        }


        public static VerticalActivitySlice[] GetAllAvailableSlices() {
            if (availableSlices != null && availableSlices.Length > 0)
                return availableSlices;

            List<VerticalActivitySlice> result = new List<VerticalActivitySlice>();
            foreach (KeyValuePair<int, VerticalSlicePointer> pair in verticalSlicePointers) {
                if (pair.Value.slice != null)
                    result.Add(pair.Value.slice);
            }

            result.Sort((a, b) => a.header.frameNumber.CompareTo(b.header.frameNumber));
            availableSlices = result.ToArray();
            return availableSlices;
        } 
        #endregion
        //--------------------------------------- GET SLICES --------------------------------------
        //--------------------------------------- GET SLICES --------------------------------------


    }
}
