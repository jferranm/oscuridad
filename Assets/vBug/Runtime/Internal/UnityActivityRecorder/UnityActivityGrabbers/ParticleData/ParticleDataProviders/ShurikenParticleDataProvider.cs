using System;
using UnityEngine;
using System.Collections.Generic;

namespace Frankfort.VBug.Internal
{
    public class ShurikenParticleDataProvider : IParticleDataProvider
    {
        private List<ParticleSystem.Particle[]> buffers = new List<ParticleSystem.Particle[]>();
        private Dictionary<int, bool> flaggedAsCapturable = new Dictionary<int, bool>();

        public void GetResultEOF(ref ParticleDataSnapshot storeTo)
        {
            ParticleSystem[] systems = GameObject.FindObjectsOfType<ParticleSystem>();
            SGenericParticleSystem[] surrogates = new SGenericParticleSystem[systems.Length];

            if(systems != null && systems.Length > 0){
                int i = systems.Length;
                while(--i > -1)                {
                    ParticleSystem system = systems[i];
                    if (system == null)
                        continue;

                    //--------------- Check if captureable --------------------
                    int id = system.GetInstanceID();
                    if (flaggedAsCapturable.ContainsKey(id)) {
                        if (!flaggedAsCapturable[id])
                            continue;
                    } else {
                        bool isCaptureable = GameObjectUtility.GetComponentInParent<vBugParticleRecordable>(system.gameObject) != null;
                        flaggedAsCapturable.Add(id, isCaptureable);

                        if (!isCaptureable)
                            continue;
                    } 
                    //--------------- Check if captureable --------------------
			
                    if (system.GetComponent<Renderer>() == null || !(system.GetComponent<Renderer>() is ParticleSystemRenderer))  
                        continue;

                    ParticleSystemRenderer renderer = (ParticleSystemRenderer)system.GetComponent<Renderer>();
                    ParticleSystem.Particle[] particles = GetParticleBuffer(system.particleCount); //fetch buffer;
                    int maxParticles = system.GetParticles(particles);

                    //--------------- Local to world --------------------
                    Matrix4x4 m = system.transform.localToWorldMatrix;
                    if (system.simulationSpace == ParticleSystemSimulationSpace.Local) {
                        for (int p = 0; p < maxParticles; p++)
                            particles[p].position = m.MultiplyPoint(particles[p].position);
                    }
                    //--------------- Local to world --------------------

                    SGenericParticleSystem surrogate = new SGenericParticleSystem(system, renderer, particles, maxParticles);
                    surrogate.renderer.materialInstanceID = MaterialDataGrabber.RegisterMaterial(system);
                    surrogates[i] = surrogate;
                    buffers.Add(particles); //store buffer
                    
                }
            }

            storeTo.particleSystems.AddRange(surrogates);
        }

         
        private ParticleSystem.Particle[] GetParticleBuffer(int idealSize)
        {
            int maxSize = Math.Max(MathHelper.MakePowerOfTwo(idealSize, true), 128);
            if (buffers == null || buffers.Count == 0)
                return new ParticleSystem.Particle[maxSize];

            ParticleSystem.Particle[] result = buffers.Find(item => item.Length >= idealSize);
            return result != null ? result : new ParticleSystem.Particle[maxSize];
        }

        public void Dispose()
        {
            buffers.Clear();
            flaggedAsCapturable.Clear();
            buffers = null;
            flaggedAsCapturable = null;
        }
    }
}
