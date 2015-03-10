using System;
using Frankfort.VBug.Internal;
using UnityEngine;

namespace Frankfort.VBug.Editor
{
    public class MemoryInfoGraph : BaseGraphContainer
    {
        
        public MemoryInfoGraph() : base( 
            new Color32[]
                {
                    new Color(1f, 0.5f, 0f),
                    new Color(0.5f, 1f, 0f),
                    new Color(0f, 0.5f, 1f),
                    new Color(0f, 1f, 0.5f),
                    new Color(1f, 0f, 1f),
                    new Color(1f, 0f, 0.5f)
                },
                new string[] { 
                    "Texture2D",
                    "RenderTexture",
                    "AudioClips",
                    "AnimationClips",
                    "Meshes",
                    "Materials"
                })
        {
            this.requiresMemoryCapture = true;
            this.notAvailableMessage = "Memory can only be measured when the profiler is active.\nYou can enable the Unity-profiler yourself, or toggle 'Auto enable profiler' in 'System Info Recording' vBug-settings.";
        }


        protected override float[] GetSliceGraphData(VerticalActivitySlice slice) {
            return new float[]
            {
                slice.systemInfo.activeTextureMemory,
                slice.systemInfo.activeRenderTextureMemory,
                slice.systemInfo.activeAudioClipMemory,
                slice.systemInfo.activeAnimationMemory,
                slice.systemInfo.activeMeshMemory,
                slice.systemInfo.activeMaterialsMemory
            };
        }

        protected override string CollectHoverData(VerticalActivitySlice slice)
        {
            float div = 1048576;
            string result = "Textures 2D/3D:  " +   (slice.systemInfo.activeTextures != -1 ?        (slice.systemInfo.activeTextures        + " [" + (slice.systemInfo.activeTextureMemory / div) + " mb]\n")       : "n/a\n");
            result += "Render textures:  " +        (slice.systemInfo.activeRenderTextures != -1 ?  (slice.systemInfo.activeRenderTextures  + " [" + (slice.systemInfo.activeRenderTextureMemory / div) + " mb]\n") : "n/a\n");
            result += "Audio clips:  " +            (slice.systemInfo.activeAudioClips != -1 ?      (slice.systemInfo.activeAudioClips      + " [" + (slice.systemInfo.activeAudioClipMemory / div) + " mb]\n")     : "n/a\n");
            result += "Animation clips:  " +        (slice.systemInfo.activeAnimations != -1 ?      (slice.systemInfo.activeAnimations      + " [" + (slice.systemInfo.activeAnimationMemory/ div) + " mb]\n")      : "n/a\n");
            result += "Mesh data:  " +              (slice.systemInfo.activeMeshes != -1 ?          (slice.systemInfo.activeMeshes          + " [" + (slice.systemInfo.activeMeshMemory / div) + " mb]\n")          : "n/a\n");
            result += "Materials:  " +              (slice.systemInfo.activeMaterials != -1 ?       (slice.systemInfo.activeMaterials       + " [" + (slice.systemInfo.activeMaterialsMemory / div) + " mb]\n")     : "n/a\n");
            return result;
        }

    }
}
