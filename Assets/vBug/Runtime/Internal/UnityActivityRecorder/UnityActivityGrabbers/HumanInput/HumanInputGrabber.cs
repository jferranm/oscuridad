using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Frankfort.VBug.Internal
{

    public class HumanInputGrabber : BaseActivityGrabber<HumanInputSnapshot>
    {
        private IHumanInputProvider[] providers ;


        protected override void Start()
        {
            base.Start();
            providers = DataProviderHelper.InitProviders<IHumanInputProvider>(vBug.settings.recording.humanInputRecording.activeProviders);
        }

        public override void AbortAndPruneCache()
        {
            base.AbortAndPruneCache();

            if (providers != null)
            {
                for (int i = 0; i < providers.Length; i++)
                    providers[i].Reset();
            }
        }

        public override bool GrabActivity(int currentFrame)
        {
            if (base.GrabActivity(currentFrame))
            {

                if (providers == null || providers.Length == 0)
                    return false;

                for (int i = 0; i < providers.Length; i++)
                    providers[i].InitCurrentFrame();

                return true;
            }
            return false;
        }



        protected override void GrabResultEndOfFrame()
        {
            base.GrabResultEndOfFrame();
            if (providers == null || providers.Length == 0)
                return;
            
            HumanInputProviderData[] result = new HumanInputProviderData[providers.Length];
            for (int i = 0; i < providers.Length; i++)
                result[i] = providers[i].GetResultEOF();

            if (resultReadyCallback != null)
                resultReadyCallback(currentFrame, new HumanInputSnapshot(result), 0);
        }
    }
}