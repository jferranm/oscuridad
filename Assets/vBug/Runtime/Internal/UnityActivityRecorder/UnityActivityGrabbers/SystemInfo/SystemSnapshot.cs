using UnityEngine;
using System;
using System.Collections;


namespace Frankfort.VBug.Internal
{
    [Serializable]
    public class SystemInfoSnapshot
    {
        public long gcTotalSize;
        public long usedHeapSize;

        public bool memoryProfiled = false;
        
        public int runtimeMemory;
        public int activeTextures = -1;
        public int activeTextureMemory = -1;
        public int activeRenderTextures = -1;
        public int activeRenderTextureMemory = -1;
        public int activeMaterials = -1;
        public int activeMaterialsMemory = -1;
        public int activeMeshes = -1;
        public int activeMeshMemory = -1;
        public int activeAnimations = -1;
        public int activeAnimationMemory = -1;
        public int activeAudioClips = -1;
        public int activeAudioClipMemory = -1;

        public float updateFps;
        public float fixedFps;
        public float averageUpdateFps;

        public SystemInfoSnapshot() //Default constructor
        { }

        public static byte[] Serialize(SystemInfoSnapshot ss)
        {
            ComposedByteStream stream = ComposedByteStream.FetchStream();
            stream.AddStream(new long[] { 
                ss.gcTotalSize, 
                ss.usedHeapSize 
            });

            stream.AddStream(BitConverter.GetBytes(ss.memoryProfiled));
            stream.AddStream(new int[] { 
                ss.runtimeMemory,
                ss.activeTextures,
                ss.activeTextureMemory,
                ss.activeRenderTextures,
                ss.activeRenderTextureMemory,
                ss.activeMaterials,
                ss.activeMaterialsMemory,
                ss.activeMeshes,
                ss.activeMeshMemory,
                ss.activeAnimations,
                ss.activeAnimationMemory,
                ss.activeAudioClips,
                ss.activeAudioClipMemory
            });

            stream.AddStream(new float[] {
                ss.updateFps,
                ss.fixedFps,
                ss.averageUpdateFps
            });

            return stream.Compose();
        }

        public static SystemInfoSnapshot Deserialize(byte[] input)
        {
            if (input == null || input.Length == 0)
                return null;

            SystemInfoSnapshot result = new SystemInfoSnapshot();
            ComposedByteStream stream = ComposedByteStream.FromByteArray(input);
            if (stream == null)
                return null;

            long[] lS = stream.ReadNextStream<long>();
                result.gcTotalSize = lS[0]; 
                result.usedHeapSize = lS[1];

            result.memoryProfiled = BitConverter.ToBoolean(stream.ReadNextStream<byte>(), 0);
            int[] iS = stream.ReadNextStream<int>();
                result.runtimeMemory = iS[0];
                result.activeTextures = iS[1];
                result.activeTextureMemory = iS[2];
                result.activeRenderTextures = iS[3];
                result.activeRenderTextureMemory = iS[4];
                result.activeMaterials = iS[5];
                result.activeMaterialsMemory = iS[6];
                result.activeMeshes = iS[7];
                result.activeMeshMemory = iS[8];
                result.activeAnimations = iS[9];
                result.activeAnimationMemory = iS[10];
                result.activeAudioClips = iS[11];
                result.activeAudioClipMemory = iS[12];
    
            float[] fS = stream.ReadNextStream<float>();
                result.updateFps = fS[0];
                result.fixedFps = fS[1];
                result.averageUpdateFps = fS[2];

            stream.Dispose();
            return result;
        }


        public void Dispose()
        {
            //TODO: Build!
        }
    }
}