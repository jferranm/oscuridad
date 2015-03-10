using System;
using UnityEngine;
using System.Collections.Generic;
using System.Threading;
using System.Collections;




namespace Frankfort.VBug.Internal
{
    public class WorkloadDistributionManager : vBugHiddenUniqueComponent<WorkloadDistributionManager>
    {
        public delegate bool WorkLoadExecutor(IAbortableWorkObject arg);
        public delegate void WorkLoadComplete(IAbortableWorkObject arg);


        private class AsyncWorkloadData
        {
            public WorkloadExecutorType executionType;
            public WorkLoadExecutor executor;
            public IAbortableWorkObject arg;
            public bool complete;
            public WorkLoadComplete onCompleteCallback;

            public AsyncWorkloadData(WorkloadExecutorType executionType, WorkLoadExecutor executor, IAbortableWorkObject arg, WorkLoadComplete onCompleteCallback)
            {
                this.executionType = executionType;
                this.executor = executor;
                this.arg = arg;
                this.onCompleteCallback = onCompleteCallback;
            }
        }













        //--------------------------------------- STATIC --------------------------------------
        //--------------------------------------- STATIC --------------------------------------
        #region STATIC


        private static int runningCycles;
        private static List<int> availableCycles = new List<int>();
        private static List<int> claimedCycles = new List<int>();
        private static List<AsyncWorkloadData> finishedThreadObjects = new List<AsyncWorkloadData>();


        public static void AddWork(int cycleIdx, WorkloadExecutorType executionType, WorkLoadExecutor executor, IAbortableWorkObject arg, WorkLoadComplete onCompleteCallback)
        {
            runningCycles = Mathf.Max(runningCycles, cycleIdx);
            if (availableCycles.Contains(cycleIdx))
                availableCycles.Remove(cycleIdx);

            SetHelper();
            helper.AddToWorkList(cycleIdx, executionType, executor, arg, onCompleteCallback);
        }


        public static int GetNextAvailableCycle()
        {
            if (availableCycles.Count > 0)
            {
                int idx = availableCycles[0];
                availableCycles.RemoveAt(0);
                return idx;
            }
            else
            {
                runningCycles++;
                while (claimedCycles.Contains(runningCycles))
                    runningCycles++;

                return runningCycles;
            }
        }

        public static void ClaimCycleID(int cycleID)
        {
            if (claimedCycles.Contains(cycleID)) {
                if (vBug.settings.general.debugMode)
                    Debug.Log("Cycle " + cycleID + " already claimed! Release it first!");
            } else {
                claimedCycles.Add(cycleID);
            }
        }

        public static void ReleaseCycleID(int cycleID)
        {
            if (!claimedCycles.Contains(cycleID)) {
                if (vBug.settings.general.debugMode)
                    Debug.Log("Cycle " + cycleID + " is not claimed. Release not needed!");
            } else {
                claimedCycles.Remove(cycleID);
            }
        }

        private static void ExecuteWorkLoad_Threaded(object cycleObj)
        {
            AsyncWorkloadCycle cycle = (AsyncWorkloadCycle)cycleObj;

            while (true)
            {
                if (cycle == null || cycle.workList == null)
                    break; //Stops thread;

                int i = 500; //Wait 500 x 10ms (5 seconds) for the worklist to become populated before terminating this thread.
                while ((cycle.locked || cycle.workList.Count == 0 || cycle.workList[0].executionType != WorkloadExecutorType.thread) && --i > -1)
                    Thread.Sleep(10);

                if (cycle.locked || cycle.workList.Count == 0 || cycle.workList[0].executionType != WorkloadExecutorType.thread) {
                    break;
                } else {
                    HandleNextItem(cycle, false);
                }
            }

            if (vBug.settings.general.debugMode)
                Debug.Log("Thread " + cycle.idx + " dies...");
        }


        #endregion
        //--------------------------------------- STATIC --------------------------------------
        //--------------------------------------- STATIC --------------------------------------
				













        //--------------------------------------- MONOBEHAVIOUR --------------------------------------
        //--------------------------------------- MONOBEHAVIOUR --------------------------------------
        #region MONOBEHAVIOUR

        private class AsyncWorkloadCycle
        {
            public int idx;
            public bool locked;
            public List<AsyncWorkloadData> workList = new List<AsyncWorkloadData>();
            public Thread thread;

            public AsyncWorkloadCycle(int idx)
            {
                this.idx = idx;
            }
        }

        
        private List<AsyncWorkloadCycle> workloadData = new List<AsyncWorkloadCycle>();
        private void Awake()
        {
 	        StartCoroutine(WaitForEOF());
        }


        //--------------- ABORT RUNNING THREADS --------------------
        protected override void OnApplicationQuit(){
 	        base.OnApplicationQuit();
            AbortRunningThreads();
        }

        private void OnDestroy() {
            AbortRunningThreads();
        }

        private void AbortRunningThreads()
        {
            runningCycles = 0;
            if (availableCycles != null)
                availableCycles.Clear();

            if (workloadData != null && workloadData.Count > 0)
            {
                foreach (AsyncWorkloadCycle cycle in workloadData)
                {
                    if (cycle.thread != null && cycle.thread.IsAlive && cycle.thread.ThreadState != ThreadState.Stopped) {
                        cycle.thread.Interrupt();
                        cycle.thread.Join(100);
                        if (vBug.settings.general.debugMode)
                            Debug.LogWarning("Close " + this.GetType().Name + " thread");
                    }

                    cycle.thread = null;
                }
            
                workloadData = null;
            }      
        }
        //--------------- ABORT RUNNING THREADS --------------------



        public void AddToWorkList(int cycleIdx, WorkloadExecutorType executionType, WorkLoadExecutor executor, IAbortableWorkObject arg, WorkLoadComplete onCompleteCallback)
        {
            AsyncWorkloadCycle cycle = workloadData.Find(item => item.idx == cycleIdx);
            if (cycle == null)
            {
                cycle = new AsyncWorkloadCycle(cycleIdx);
                workloadData.Add(cycle);
            }

            cycle.workList.Add(new AsyncWorkloadData(executionType, executor, arg, onCompleteCallback));

            if (executionType == WorkloadExecutorType.thread && vBug.settings.general.multiThreading)
            {
                if (cycle.thread == null || !cycle.thread.IsAlive)
                    cycle.thread = StartNewWorkerThread(cycle);
            }
        }

        private Thread StartNewWorkerThread(AsyncWorkloadCycle cycle)
        {
            Thread result = new Thread(ExecuteWorkLoad_Threaded);
            result.Start(cycle);
            if (vBug.settings.general.debugMode)
                Debug.LogWarning("WorkloadDistibutionManager-> thread created: " + result.ManagedThreadId);
            return result;
        }





        //--------------- WORKLOAD EXECUTION LOOPS --------------------
        /// <summary>
        /// All cycles have their own Thread and can be executed in parallel, unlike the Update and LateUpdate routines
        /// </summary>
        /// <param name="workArg"></param>
        private void Update()
        {
            if (finishedThreadObjects != null && finishedThreadObjects.Count > 0)
            {
                lock (finishedThreadObjects)
                {
                    for (int i = 0; i < finishedThreadObjects.Count; i++)
                    {
                        if (finishedThreadObjects[i].onCompleteCallback != null)
                            finishedThreadObjects[i].onCompleteCallback(finishedThreadObjects[i].arg);
                    }
                    finishedThreadObjects.Clear();
                }
            }

            if (workloadData != null)
                ExecuteNextItemSequential(WorkloadExecutorType.update);
        }
        private void LateUpdate()
        {
            if (workloadData == null)
                return;

            ExecuteNextItemSequential(WorkloadExecutorType.lateUpdate);
        }
        private void FixedUpdate()
        {
            if (workloadData == null)
                return;

            ExecuteNextItemSequential(WorkloadExecutorType.fixedUpdate);
        }

        private IEnumerator WaitForEOF()
        {
            while (true)
            {
                if (workloadData == null)
                    continue;

                ExecuteNextItemSequential(WorkloadExecutorType.waitForEndOfFrame);
                yield return new WaitForEndOfFrame();
            }
        }
        private void OnGUI()
        {
            if (workloadData == null || Event.current.type == EventType.Repaint)
                return;

            ExecuteNextItemSequential(WorkloadExecutorType.gui);
        }


        private void ExecuteNextItemSequential(WorkloadExecutorType workloadExecutorType)
        {
            for (int i = 0; i < workloadData.Count; i++)
            {
                AsyncWorkloadCycle cycle = workloadData[i];
                if (cycle.locked || cycle.workList == null || cycle.workList.Count == 0)
                    continue;

                if (!vBug.settings.general.multiThreading)
                    cycle.workList[0].executionType = WorkloadExecutorType.waitForEndOfFrame;

                if (cycle.workList[0].executionType != workloadExecutorType)
                    continue;

                HandleNextItem(cycle, true);
            }
        }
        private static void HandleNextItem(AsyncWorkloadCycle cycle, bool isMainThread)
        {
            try
            {
                cycle.locked = true;

                AsyncWorkloadData nextItem = cycle.workList[0];
                if (nextItem.arg == null || !nextItem.arg.isAborted)
                {
                    bool succes = false;
                    if (nextItem.executor != null)
                        succes = nextItem.executor(nextItem.arg);
                    
                    cycle.workList.RemoveAt(0);
                    nextItem.complete = true;

                    if (cycle.workList.Count == 0 && !availableCycles.Contains(cycle.idx))
                    {
                        lock (availableCycles)
                        {
                            availableCycles.Add(cycle.idx);
                        }
                    }
                    //MAKE SURE THIS GETS EXECUTED ON THE MAIN THREAD!
                    if (succes && nextItem.onCompleteCallback != null)
                    {
                        if (isMainThread)
                        {
                            nextItem.onCompleteCallback(nextItem.arg);
                        }
                        else
                        {
                            lock (finishedThreadObjects)
                            {
                                finishedThreadObjects.Add(nextItem);
                            }
                        }
                    }
                }
                cycle.locked = false;
            } catch (Exception e) {
                if (vBug.settings.general.debugMode)
                    Debug.LogError(e.Message + e.StackTrace);
            }
        }
        //--------------- WORKLOAD EXECUTION LOOPS --------------------



        #endregion
        //--------------------------------------- MONOBEHAVIOUR --------------------------------------
        //--------------------------------------- MONOBEHAVIOUR --------------------------------------


    }
}