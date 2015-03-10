using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Frankfort.VBug.Internal
{

    public class MeshDataGrabber : BaseActivityGrabber<MeshDataSnapshot>
    {
        private IMeshDataProvider[] providers;
        private int captureFrameCount = 0;
        private Dictionary<int, int> forceKeyframeStartFrame = new Dictionary<int, int>();
        
        protected override void Start()
        {
            base.Start();
            providers = DataProviderHelper.InitProviders<IMeshDataProvider>(vBug.settings.recording.meshDataRecording.activeProviders);
        }

        public override void AbortAndPruneCache() {
            base.AbortAndPruneCache();
            MeshDataChangeDetector.Reset();
        }


        protected override void OnDestroy() {
            base.OnDestroy();

            if (providers != null) {
                for (int i = 0; i < providers.Length; i++)
                    providers[i].Dispose();
            }
            providers = null;
        }


        protected override void GrabResultEndOfFrame()
        {
            if (providers == null || providers.Length == 0)
                return;

            MeshDataSnapshot currentSnapshot = new MeshDataSnapshot();
            vBugMeshRecordable[] allRecordable = GameObject.FindObjectsOfType<vBugMeshRecordable>();

            int prio = 0;
            int forceInterval = Math.Max(1, vBug.settings.recording.meshDataRecording.forceKeyFrameInterval / vBug.settings.recording.meshDataRecording.captureInterval.targetInterval);
            captureFrameCount++;

            foreach (vBugMeshRecordable comp in allRecordable) {
                GameObject go = comp.gameObject;
                if (!go.activeInHierarchy || go.isStatic)
                    continue;

                SWorldObject wo = new SWorldObject(go);
                wo.color = comp.color;
                wo.meshes = new List<SMesh>();

                for (int i = 0; i < providers.Length; i++) {

                    //--------------- Random force keyframe offset --------------------
                    int offset = -1;
                    if (forceKeyframeStartFrame.ContainsKey(wo.instanceID)) {
                        offset = forceKeyframeStartFrame[wo.instanceID];
                    } else {
                        forceKeyframeStartFrame.Add(wo.instanceID, UnityEngine.Random.Range(0, forceInterval)); //Set a random offset to the keyframe capture, to evenely distribute Key-frame capturing.
                    } 
                    //--------------- Random force keyframe offset --------------------
			
                    bool forceKeyFrame = levelChanged || offset == -1 || (captureFrameCount + offset) % forceInterval == 0f;
                    providers[i].GetResultEOF(ref prio, go, comp.captureMethod, forceKeyFrame, wo.meshes);
                }
                currentSnapshot.worldObjects.Add(wo);
            }

            if (resultReadyCallback != null)
                resultReadyCallback(currentFrame, currentSnapshot, prio);
        }

    }

}