using System;
using System.Collections.Generic;
using UnityEngine;

namespace Frankfort.VBug.Internal
{
    public class ParticleDataGrabber : BaseActivityGrabber<ParticleDataSnapshot>
    {
        private IParticleDataProvider[] providers;
        
        protected override void Start()
        {
            base.Start();
            providers = DataProviderHelper.InitProviders<IParticleDataProvider>(vBug.settings.recording.particleDataRecording.activeProviders);
        }

        protected override void OnDestroy() {
            base.OnDestroy();
            if (providers != null) {
                foreach (IParticleDataProvider provider in providers)
                    provider.Dispose();
            }
            providers = null;
        }

        protected override void GrabResultEndOfFrame()
        {
            if (providers == null || providers.Length == 0)
                return;

            ParticleDataSnapshot currentSnapshot = new ParticleDataSnapshot(); 
            for (int i = 0; i < providers.Length; i++)
                providers[i].GetResultEOF(ref currentSnapshot);

            if (resultReadyCallback != null)
                resultReadyCallback(currentFrame, currentSnapshot, 0);
        }
    }
}
