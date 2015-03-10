using System;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using Frankfort.VBug;
using UnityEngine;

namespace Frankfort.VBug.Internal
{
    [Serializable]
    public class DataProcessingManager : vBugHiddenUniqueComponent<DataProcessingManager>
    {
        //--------------------------------------- STATIC EXTERNAL CALLS --------------------------------------
        //--------------------------------------- STATIC EXTERNAL CALLS --------------------------------------
        #region STATIC EXTERNAL CALLS

        public static void ProcessData(VerticalActivitySlice slice)
        {
            SetHelper();
            helper.StartProcessingData(slice);
        }

        #endregion
        //--------------------------------------- STATIC EXTERNAL CALLS --------------------------------------
        //--------------------------------------- STATIC EXTERNAL CALLS --------------------------------------







         

        //--------------------------------------- MONOBEHAVIOUR --------------------------------------
        //--------------------------------------- MONOBEHAVIOUR --------------------------------------
        #region MONOBEHAVIOUR
        
        private const int distributedWorkCycleID = 1000;

        private bool isAlive = false;
        private Thread[] dataProcessThreads;
        private int currentFrame;
        private List<string> threadLogs = new List<string>();
        private List<VerticalActivitySlice> unprocessedPackageQueue = new List<VerticalActivitySlice>();
        private int pendingCount = 0;

        private void Start(){
            WorkloadDistributionManager.ClaimCycleID(distributedWorkCycleID);

            isAlive = true;
            if (vBug.settings.general.multiThreading){
                int iMax = Mathf.Max(1, SystemInfo.processorCount - 1);
                dataProcessThreads = new Thread[iMax];
                for (int i = 0; i < iMax; i++) {
                    Thread dataProcessThread = new Thread(HanleUnprocessedPackageThreaded);
                    dataProcessThread.Priority = System.Threading.ThreadPriority.BelowNormal ;
                    dataProcessThread.Start();
                    dataProcessThreads[i] = dataProcessThread;
                }
                if (vBug.settings.general.debugMode)
                    Debug.LogWarning("DataProcessingManager-> " + iMax + " dataProcessThreads reated!");
            }
        }


        protected override void OnApplicationQuit() {
            base.OnApplicationQuit();
            AbortRunningThreads();
        }

        private void OnDestroy() {
            AbortRunningThreads();
        }

        private void AbortRunningThreads()
        {
            pendingCount = 0;
            isAlive = false;
            if (dataProcessThreads != null)
            {
                foreach (Thread dataProcessThread in dataProcessThreads) {
                    if (dataProcessThread.IsAlive && dataProcessThread.ThreadState != ThreadState.Stopped) {
                        dataProcessThread.Interrupt();
                        dataProcessThread.Join(100);
                        if (vBug.settings.general.debugMode)
                            Debug.LogWarning("Close " + this.GetType().Name + " thread");
                    }
                }

                if (vBug.settings.general.debugMode)
                    Debug.LogWarning("DataProcessingManager-> " + dataProcessThreads.Length + " dataProcessThreads terminated!");
                dataProcessThreads = null;
            }

            if (threadLogs != null)
            {
                threadLogs.Clear();
                threadLogs = null;
            }

            if (unprocessedPackageQueue != null)
            {
                unprocessedPackageQueue.Clear();
                unprocessedPackageQueue = null;
            }
        }



        private void PruneLowPrioSlice(List<VerticalActivitySlice> list)
        {
            lock (list)
            {
                list.Sort(SortForPruning);
                VerticalActivitySlice prune = list[0];
                prune.isAborted = true;
                list.RemoveAt(0);
                if (vBug.settings.general.debugMode)
                    Debug.Log("Prune " + prune.header.frameNumber);
            }
        }

        private int SortForPruning(VerticalActivitySlice x, VerticalActivitySlice y)
        {
            int a = x.header.frameNumber + x.streamPriority;
            int b = y.header.frameNumber + y.streamPriority;
            return b - a;
        }
        private int SortForProcessing(VerticalActivitySlice x, VerticalActivitySlice y)
        {
            int a = x.header.frameNumber - x.streamPriority;
            int b = y.header.frameNumber - y.streamPriority;
            return b - a;
        }


        public void StartProcessingData(VerticalActivitySlice slice)
        {
            if (unprocessedPackageQueue.Count >= Mathf.Max(1, vBug.settings.general.dataProcessing.processPackageMaxQueueSize))
                PruneLowPrioSlice(unprocessedPackageQueue);

            unprocessedPackageQueue.Add(slice);
        }


        private void Update()
        {
            if (!vBug.settings.general.multiThreading)
                HanleUnprocessedPackage();
        
            PrintThreadLogs();
        }


        //--------------- SEPERATE THREAD --------------------
        private void HanleUnprocessedPackageThreaded()
        {
            while (isAlive)
            {
                HanleUnprocessedPackage();
                if(unprocessedPackageQueue.Count == 0)
                    Thread.Sleep(vBug.settings.general.dataProcessing.threadSleepTime);
            }
        } 

        //45 MS, met profiler aan, in FollowExample
        private void HanleUnprocessedPackage(){
            if (unprocessedPackageQueue.Count > 0){
                if (pendingCount >= Math.Max(1, vBug.settings.general.dataProcessing.processPackageMaxPendingSize)) {
                    lock (unprocessedPackageQueue) {
                        PruneLowPrioSlice(unprocessedPackageQueue);
                    }
                } else {
                    VerticalActivitySlice nextPackage = null;
                    lock (unprocessedPackageQueue){
                        if (unprocessedPackageQueue.Count > 0) { //Check again, cause it will throw errors every now and then cause of multithreading
                            unprocessedPackageQueue.Sort(SortForProcessing);
                            nextPackage = unprocessedPackageQueue[0];
                            unprocessedPackageQueue.RemoveAt(0);
                        }
                    }

                    if (nextPackage != null)
                    {
                        pendingCount++;
                        try{
                            if (!ProcessDataPackage(nextPackage))
                                AddThreadLog("Hmmm, something went wrong with package " + nextPackage.header.frameNumber + ", prio: " + nextPackage.streamPriority);
                        } catch (Exception e) {
                            AddThreadLog(e.Message + e.StackTrace);
                        }
                        pendingCount--;
                    }
                }
            }
        }
        //--------------- SEPERATE THREAD --------------------



        //--------------- Handle ThreadLogs --------------------
        private void PrintThreadLogs()
        {
            if (threadLogs != null && threadLogs.Count > 0)
            {
                lock (threadLogs)
                {
                    foreach (string message in threadLogs) {
                        if (vBug.settings.general.debugMode)
                            Debug.LogWarning(message);
                    }
                    threadLogs.Clear();
                }
            }
        }

        private void AddThreadLog(string message)
        {
            if (vBug.settings.general.multiThreading){
                lock (threadLogs){
                    threadLogs.Add(message);
                }
            } else {
                if (vBug.settings.general.debugMode)
                    Debug.LogWarning(message);
            }
        }
        //--------------- Handle ThreadLogs --------------------
			



        private bool ProcessDataPackage(IAbortableWorkObject arg)
        {
            VerticalActivitySlice package = (VerticalActivitySlice)arg;
            if (package.isAborted)
                return false;

            byte[] packedData = null;
            try
            {
                packedData = VerticalActivitySlice.Serialize(package);
                ComposedByteStream.BlitBuffers();
            }
            catch(Exception e)
            {
                AddThreadLog(e.Message + e.StackTrace);
                return false;
            }

            if (packedData == null)
                return false;

            ProcessHandlingType type = vBug.settings.general.dataProcessing.processingType;
            if((type & ProcessHandlingType.saveToDisk) == ProcessHandlingType.saveToDisk)
            {
                string path = vBugEnvironment.GetFullSessionPath(package.header);
                path += "/" + vBugEnvironment.GetPaddedNumber(package.header.frameNumber, vBug.settings.general.framePadding) + ".vBugSlice";

                try{
                    IORuntimeHelper.WriteAllBytes(path, packedData);
                } catch(Exception e) {
                    AddThreadLog(e.Message + e.StackTrace);
                }
            }
            if ((type & ProcessHandlingType.streamToEditor) == ProcessHandlingType.streamToEditor)
            {
                AddThreadLog("Put some pure and sparklin magic here dear dear Eamon ;-) ");
                return false;
            }
            if ((type & ProcessHandlingType.streamToServer) == ProcessHandlingType.streamToServer)
            {
                AddThreadLog("Put some pure and sparklin magic here dear dear Eamon ;-) ");
                return false;
            }
            return true;
        }


        #endregion
        //--------------------------------------- MONOBEHAVIOUR --------------------------------------
        //--------------------------------------- MONOBEHAVIOUR --------------------------------------

    }
}