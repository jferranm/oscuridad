using System;
using System.Collections.Generic;
using UnityEngine;

namespace Frankfort.VBug.Internal
{

    public class ParticleDataSnapshot
    {
        public List<SGenericParticleSystem> particleSystems = new List<SGenericParticleSystem>();


        public void Dispose()
        {
            if (particleSystems != null && particleSystems.Count > 0)
            {
                foreach (SGenericParticleSystem system in particleSystems) {
                    if (system != null)
                        system.Dispose();
                }
                particleSystems = null;
            }
        }

        public static byte[] Serialize(ParticleDataSnapshot input)
        {
            if (input == null)
                return null;

            ComposedByteStream stream = ComposedByteStream.FetchStream(input.particleSystems.Count);
            int iMax = input.particleSystems.Count;
            for (int i = 0; i < iMax; i++)
                stream.AddStream(SGenericParticleSystem.Serialize(input.particleSystems[i]));

            return stream.Compose();
        }


        public static ParticleDataSnapshot Deserialize(byte[] input)
        {
            if (input == null || input.Length == 0)
                return null;

            ComposedByteStream stream = ComposedByteStream.FromByteArray(input);
            if (stream == null)
                return null;

            ParticleDataSnapshot result = new ParticleDataSnapshot();
            int iMax = stream.streamCount;
            result.particleSystems = new List<SGenericParticleSystem>(iMax);
            
            for (int i = 0; i < iMax; i++)
                result.particleSystems.Add(SGenericParticleSystem.Deserialize(stream.ReadNextStream<byte>()));
            
            stream.Dispose();  
            return result;
        }
    }



    [Serializable]
    public enum SParticleRenderMode
    {
        Billboard,
        SortedBillboard,
        Stretch,
        HorizontalBillboard,
        VerticalBillboard,
        Mesh
    }


    [Serializable]
    public class SParticleRenderer
    {
        public Vector3 camPos;
        public Vector3 camUp;
        public int materialInstanceID = -1;
        public SParticleRenderMode renderMode;
        public float cameraVelocityScale;
        public float lengthScale;
        public float velocityScale;


        public static byte[] Serialize(SParticleRenderer input)
        {
            ComposedPrimitives prims = new ComposedPrimitives();
            prims.AddValue(input.camPos.x); prims.AddValue(input.camPos.y); prims.AddValue(input.camPos.z);
            prims.AddValue(input.camUp.x); prims.AddValue(input.camUp.y); prims.AddValue(input.camUp.z);
            prims.AddValue(input.materialInstanceID);
            prims.AddValue(input.renderMode);
            prims.AddValue(input.cameraVelocityScale);
            prims.AddValue(input.lengthScale);
            prims.AddValue(input.velocityScale);

            return prims.Compose();
        }

        public static SParticleRenderer Deserialize(byte[] input)
        {
            SParticleRenderer result = new SParticleRenderer();
            ComposedPrimitives prims = ComposedPrimitives.FromByteArray(input);
            result.camPos = new Vector3(prims.ReadNextValue<float>(), prims.ReadNextValue<float>(), prims.ReadNextValue<float>());
            result.camUp = new Vector3(prims.ReadNextValue<float>(), prims.ReadNextValue<float>(), prims.ReadNextValue<float>());
            result.materialInstanceID = prims.ReadNextValue<int>();
            result.renderMode = prims.ReadNextValue<SParticleRenderMode>();
            result.cameraVelocityScale = prims.ReadNextValue<float>();
            result.lengthScale = prims.ReadNextValue<float>();
            result.velocityScale = prims.ReadNextValue<float>();

            prims.Dispose();
            return result;
        }
    }



    [Serializable]
    public class SGenericParticleArray
    {
        public float[] positions;
        //public float[] lifetimes;
        //public float[] rotations;
        public float[] sizes;
        public byte[] colors;
        public float[] velocities; //Vector3

        internal void Dispose()
        {
            this.colors = null;
            this.positions = null;
            //this.rotations = null;
            this.sizes = null;
            this.velocities = null;
        }




        public SGenericParticleArray() //Default Constructor
        { }

        public SGenericParticleArray(Particle[] particles)
        {
            if (particles == null || particles.Length == 0)
            {
                InitArrays(0);
                return;
            }

            int iMax = particles.Length;
            InitArrays(iMax);

            int i = iMax;
            while(--i > -1)
            {
                int iThree = i * 3;
                int iFour = i * 4;

                Particle p = particles[i];
                AddColor(p.color, iFour);
                AddPosition(p.position, iThree);
                AddVelocity(p.velocity, iThree);

                //rotations[i] = p.rotation;
                sizes[i] = p.size;
            }
        }

        //TODO: Creates a relative big spike @ self-ms.
        public SGenericParticleArray(ParticleSystem.Particle[] particles, int particleCount)
        {
            if (particles == null || particles.Length == 0)
            {
                InitArrays(0);
                return;
            }

            int iMax = particleCount;
            InitArrays(iMax);

            int iThree = 0;
            int iFour = 0;
            
            for(int i = 0; i < iMax; i++){
                ParticleSystem.Particle p = particles[i];
                AddColor(p.color, iFour);
                AddPosition(p.position, iThree);
                AddVelocity(p.velocity, iThree);

                //rotations[i] = p.rotation;
                sizes[i] = p.size;

                iThree += 3;
                iFour += 4;
            }
        }

        private void InitArrays(int size)
        {
            colors = new byte[size * 4];
            positions = new float[size * 3];
            //rotations = new float[size];
            sizes = new float[size];
            velocities = new float[size * 3];
        }


        private void AddColor(Color32 color, int iFour)
        {
            colors[iFour] = color.r;
            colors[iFour + 1] = color.g;
            colors[iFour + 2] = color.b;
            colors[iFour + 3] = color.a;
        }

        private void AddPosition(Vector3 pos, int iThree)
        {
            positions[iThree] = pos.x;
            positions[iThree + 1] = pos.y;
            positions[iThree + 2] = pos.z;
        }

        private void AddVelocity(Vector3 velocity, int iThree)
        {
            velocities[iThree] = velocity.x;
            velocities[iThree + 1] = velocity.y;
            velocities[iThree + 2] = velocity.z;
        }

        public static byte[] Serialize(SGenericParticleArray input)
        {
            if (input == null)
                return null;

            ComposedByteStream stream = ComposedByteStream.FetchStream(8);
            stream.AddStream(input.colors);
            stream.AddStream(input.positions);
            //stream.AddStream(input.rotations);
            stream.AddStream(input.sizes);
            stream.AddStream(input.velocities);
            return stream.Compose();
        }

        public static SGenericParticleArray Deserialize(byte[] input)
        {
            if (input == null || input.Length == 0)
                return null;

            ComposedByteStream stream = ComposedByteStream.FromByteArray(input);
            SGenericParticleArray result = new SGenericParticleArray();

            result.colors = stream.ReadNextStream<byte>();
            result.positions = stream.ReadNextStream<float>();
            //result.rotations = stream.ReadNextStream<float>();
            result.sizes = stream.ReadNextStream<float>();
            result.velocities = stream.ReadNextStream<float>();

            stream.Dispose();
            return result;
        }

    }
    
    
    [Serializable]
    public class SGenericParticleSystem
    {
        public int instanceID;
        public int goInstanceID;
        public bool isLegacy;
        public bool isWorldSpace;
        public bool emit;
        public bool enabled;
        public Vector3 position;
        public string name;
        public SParticleRenderer renderer;
        public SGenericParticleArray particles;
        
        public SGenericParticleSystem() //Default constructor.
        { }

        public SGenericParticleSystem(ParticleEmitter emitter, ParticleRenderer renderer, Particle[] particles)
        {
            if (emitter == null || renderer == null)
                return;

            instanceID = emitter.GetInstanceID();
            goInstanceID = emitter.gameObject.GetInstanceID();
            isLegacy = true;
            isWorldSpace = emitter.useWorldSpace;
            emit = emitter.emit;
            enabled = emitter.enabled;
            position = emitter.transform.position;
            name = emitter.gameObject.name;
            SetParticleRenderer(renderer);

            this.particles = new SGenericParticleArray(particles);
        }


        public SGenericParticleSystem(ParticleSystem system, ParticleSystemRenderer renderer, ParticleSystem.Particle[] particles, int particleCount)
        {
            if (system == null || renderer == null)
                return;

            instanceID = system.GetInstanceID();
            goInstanceID = system.gameObject.GetInstanceID();
            isLegacy = false;
            isWorldSpace = system.simulationSpace == ParticleSystemSimulationSpace.World;
            emit = system.enableEmission;
            enabled = system.GetComponent<Renderer>() != null ? system.GetComponent<Renderer>().enabled : false;
            position = system.transform.position;
            name = system.gameObject.name;
            SetParticleRenderer(renderer);

            this.particles = new SGenericParticleArray(particles, particleCount);
        }


        private void SetParticleRenderer(ParticleRenderer renderer)
        {
            if (renderer == null)
                return;

            this.renderer = new SParticleRenderer();
            SetMainCamPos();

            switch (renderer.particleRenderMode)
            {
                case ParticleRenderMode.Billboard: this.renderer.renderMode = SParticleRenderMode.Billboard; break;
                case ParticleRenderMode.HorizontalBillboard: this.renderer.renderMode = SParticleRenderMode.HorizontalBillboard; break;
                case ParticleRenderMode.SortedBillboard: this.renderer.renderMode = SParticleRenderMode.SortedBillboard; break;
                case ParticleRenderMode.Stretch: this.renderer.renderMode = SParticleRenderMode.Stretch; break;
                case ParticleRenderMode.VerticalBillboard: this.renderer.renderMode = SParticleRenderMode.VerticalBillboard; break;
            }
            
            this.renderer.cameraVelocityScale = renderer.cameraVelocityScale;
            this.renderer.lengthScale = renderer.lengthScale;
            this.renderer.velocityScale = renderer.velocityScale;
        }


        private void SetParticleRenderer(ParticleSystemRenderer renderer)
        {
            if (renderer == null)
                return;

            this.renderer = new SParticleRenderer();
            SetMainCamPos();

            switch(renderer.renderMode)
            {
                case ParticleSystemRenderMode.Billboard: this.renderer.renderMode = SParticleRenderMode.Billboard; break;
                case ParticleSystemRenderMode.HorizontalBillboard: this.renderer.renderMode = SParticleRenderMode.HorizontalBillboard; break;
                case ParticleSystemRenderMode.Mesh: this.renderer.renderMode = SParticleRenderMode.Mesh; break;
                case ParticleSystemRenderMode.Stretch: this.renderer.renderMode = SParticleRenderMode.Stretch; break;
                case ParticleSystemRenderMode.VerticalBillboard: this.renderer.renderMode = SParticleRenderMode.VerticalBillboard; break; 
            }

            this.renderer.cameraVelocityScale = renderer.cameraVelocityScale;
            this.renderer.lengthScale = renderer.lengthScale;
            this.renderer.velocityScale = renderer.velocityScale;
        }

        private void SetMainCamPos()
        {
            if (Camera.main != null)
            {
                this.renderer.camPos = Camera.main.transform.position;
                this.renderer.camUp = Camera.main.transform.up;
            }
            else if (Camera.current != null)
            {
                this.renderer.camPos = Camera.current.transform.position;
                this.renderer.camUp = Camera.current.transform.up;
            }
        }


        public static byte[] Serialize(SGenericParticleSystem input){
            if (input == null)
                return null;

            ComposedPrimitives prims = new ComposedPrimitives();
            prims.AddValue(input.instanceID);
            prims.AddValue(input.goInstanceID);
            prims.AddValue(input.isLegacy);
            prims.AddValue(input.isWorldSpace);
            prims.AddValue(input.emit);
            prims.AddValue(input.enabled);
            prims.AddValue(input.position.x);
            prims.AddValue(input.position.y);
            prims.AddValue(input.position.z);

            ComposedByteStream stream = ComposedByteStream.FetchStream(4);
            stream.AddStream(prims.Compose());
            stream.AddStream(input.name);
            stream.AddStream(SParticleRenderer.Serialize(input.renderer));
            stream.AddStream(SGenericParticleArray.Serialize(input.particles));
            return stream.Compose();
        }


        public static SGenericParticleSystem Deserialize(byte[] input){
            if (input == null || input.Length == 0)
                return null;

            SGenericParticleSystem result = new SGenericParticleSystem();
            ComposedByteStream stream = ComposedByteStream.FromByteArray(input);
            ComposedPrimitives prims = ComposedPrimitives.FromByteArray(stream.ReadNextStream<byte>());
            result.name = stream.ReadNextStream();
            result.renderer = SParticleRenderer.Deserialize(stream.ReadNextStream<byte>());
            result.particles = SGenericParticleArray.Deserialize(stream.ReadNextStream<byte>());

            result.instanceID = prims.ReadNextValue<int>();
            result.goInstanceID = prims.ReadNextValue<int>();
            result.isLegacy = prims.ReadNextValue<bool>();
            result.isWorldSpace = prims.ReadNextValue<bool>();
            result.emit = prims.ReadNextValue<bool>();
            result.enabled = prims.ReadNextValue<bool>();
            result.position = new Vector3(
                prims.ReadNextValue<float>(),
                prims.ReadNextValue<float>(),
                prims.ReadNextValue<float>());

            stream.Dispose();
            return result;
        }

        internal void Dispose()
        {
            if (particles != null)
                particles.Dispose();

            renderer = null;
            particles = null;
        }
    }
} 