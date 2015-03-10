using System;
using UnityEngine;
using System.Collections.Generic;

namespace Frankfort.VBug.Internal
{
    public class LegacyParticleDataProvider : IParticleDataProvider
    {
        private Dictionary<int, bool> flaggedAsCapturable = new Dictionary<int, bool>();


        public void GetResultEOF(ref ParticleDataSnapshot storeTo)
        {
            ParticleEmitter[] emitters = GameObject.FindObjectsOfType<ParticleEmitter>();
            SGenericParticleSystem[] surrogates = new SGenericParticleSystem[emitters.Length];

            int e = 0;
            if (emitters != null && emitters.Length > 0) {
                int i = emitters.Length;
                while (--i > -1){
                    ParticleEmitter emitter = emitters[i];

                    //--------------- Check if captureable --------------------
                    int id = emitter.GetInstanceID();
                    if (flaggedAsCapturable.ContainsKey(id)) {
                        if (!flaggedAsCapturable[id])
                            continue;
                    } else {
                        bool isCaptureable = GameObjectUtility.GetComponentInParent<vBugParticleRecordable>(emitter.gameObject) != null;
                        flaggedAsCapturable.Add(id, isCaptureable);

                        if(!isCaptureable)
                            continue;
                    }
                    //--------------- Check if captureable --------------------


                    ParticleRenderer renderer = emitter.gameObject.GetComponent<ParticleRenderer>();
                    if (renderer != null){
                        Particle[] particles = emitter.particles;

                        if (!emitter.useWorldSpace) {
                            Matrix4x4 m = emitter.transform.localToWorldMatrix;
                            for (int p = 0; p < particles.Length; p++)
                                particles[p].position = m.MultiplyPoint(particles[p].position);
                        }

                        SGenericParticleSystem surrogate = new SGenericParticleSystem(emitter, renderer, particles);
                        surrogate.renderer.materialInstanceID = MaterialDataGrabber.RegisterMaterial(renderer);
                        surrogates[e++] = surrogate;
                    }
                }
            }

            if (e != emitters.Length)
                Array.Resize(ref surrogates, e);

            storeTo.particleSystems.AddRange(surrogates);
        }


        public void Dispose() {
            flaggedAsCapturable.Clear();
            flaggedAsCapturable = null;
        }
    }
}
