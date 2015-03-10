using System;
using System.Collections.Generic;
using UnityEngine;

namespace Frankfort.VBug.Internal
{
    public static class TexturePool
    {
        private const int maxByteBuffers = 1024;
        private const int maxTextures2D = 64;
        private const int maxRenderTextures = 4;

        private static List<Texture2D> texture2DBuffers = new List<Texture2D>();
        private static List<RenderTexture> renderTextureBuffers = new List<RenderTexture>();

        //--------------- Dispose --------------------
        public static void PruneCache()
        {
            if (Application.isPlaying)
            {
                foreach (Texture2D texture in texture2DBuffers)
                    UnityEngine.Object.Destroy(texture);

                foreach (RenderTexture texture in renderTextureBuffers)
                    UnityEngine.Object.Destroy(texture);
            } else { 
                foreach (Texture2D texture in texture2DBuffers)
                    UnityEngine.Object.DestroyImmediate(texture);

                foreach (RenderTexture texture in renderTextureBuffers)
                    UnityEngine.Object.DestroyImmediate(texture);
            }

            texture2DBuffers.Clear();
            renderTextureBuffers.Clear();
        }
        //--------------- Dispose --------------------
			




        //--------------- TEXTURE 2D --------------------
        public static Texture2D GetTexture2D(int width, int height)
        {
            Texture2D result = null;
            int i = texture2DBuffers.Count;
            while (--i > -1){
                if (texture2DBuffers[i] == null){
                    texture2DBuffers.RemoveAt(i);
                }
            }

            int idx = texture2DBuffers.FindIndex(item => item.width == width && item.height == height);
            if (idx != -1)
            {
                result = texture2DBuffers[idx];
                texture2DBuffers.RemoveAt(idx);
            }

            if (result == null)
            {
                result = new Texture2D(width, height, TextureFormat.ARGB32, false, false);
                result.filterMode = FilterMode.Point;
                result.anisoLevel = 0;
            }

            return result;
        }



        public static void StoreTexture2D(Texture2D texture)
        {
            if (texture == null || texture2DBuffers.Contains(texture))
                return;

            if (texture2DBuffers.Count > maxTextures2D)
            {
                if (vBug.settings.general.debugMode)
                    Debug.Log("Max Texture2D's reached!");
                if (Application.isPlaying){
                    UnityEngine.Object.Destroy(texture2DBuffers[0]);
                }else{
                    UnityEngine.Object.DestroyImmediate(texture2DBuffers[0]);
                }
                texture2DBuffers.RemoveAt(0);
            }

            texture2DBuffers.Add(texture);
        }
        //--------------- TEXTURE 2D --------------------
        
        
        
        //--------------- RENDERTEXTURES --------------------
        public static RenderTexture GetRenderTexture(int width, int height, RenderTextureFormat format)
        {
            int i = renderTextureBuffers.Count;
            while (--i > -1){
                if (renderTextureBuffers[i] == null){
                    renderTextureBuffers.RemoveAt(i);
                }
            }

            RenderTexture result = null;
            int idx = renderTextureBuffers.FindIndex(item => item.width == width && item.height == height && item.format == format);
            if (idx != -1)
            {
                result = renderTextureBuffers[idx];
                renderTextureBuffers.RemoveAt(idx);
            }

            if (result == null)
            {
                result = new RenderTexture(width, height, 16, format, RenderTextureReadWrite.Default);
                result.filterMode = FilterMode.Point;
                result.anisoLevel = 0;
            }

            return result;
        }

        public static void StoreRenderTexture(RenderTexture texture)
        {
            if (texture == null || renderTextureBuffers.Contains(texture))
                return;

            if (renderTextureBuffers.Count > maxRenderTextures)
            {
                if (vBug.settings.general.debugMode)
                    Debug.Log("Max RenderTextures's reached!");

                if (Application.isPlaying) {
                    UnityEngine.Object.Destroy(renderTextureBuffers[0]);
                } else {
                    UnityEngine.Object.DestroyImmediate(renderTextureBuffers[0]);
                }
                renderTextureBuffers.RemoveAt(0);
            }

            renderTextureBuffers.Add(texture);
        }
        //--------------- RENDERTEXTURES --------------------
        
    }
}
