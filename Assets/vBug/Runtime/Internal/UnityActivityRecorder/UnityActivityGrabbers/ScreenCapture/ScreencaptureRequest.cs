using UnityEngine;
using System;
using System.Collections;

namespace Frankfort.VBug.Internal
{
    public class ScreenCaptureRequest : IAbortableWorkObject
    {
        public class RenderLayer
        {
            public RenderTexture sourceTexture;
            public Texture2D destTexture;
            public Color32[] colorBuffer;
            public byte[] byteBuffer;
            public Rect pixelRect;

            public void Dispose()
            {
                if(sourceTexture != null)
                    TexturePool.StoreRenderTexture(sourceTexture);

                if (destTexture != null)
                    TexturePool.StoreTexture2D(destTexture);

                byteBuffer = null;
                colorBuffer = null;
                sourceTexture = null;
                destTexture = null;
            }
        }

        public enum State
        {
            idle,
            readPixels,
            getBytes,
            getColors,
            complete
        }

        public int resultWidth;
        public int resultHeight;

        public Camera[] renderCams;
        public RenderLayer[] renderLayers;
        
        public ScreenCaptureQuality quality;
        public int scaleDown = 1;
        
        public int cycleID;
        public State state;
        public int frameNumber;

        public bool isAborted { get; set; }

        public ScreenCaptureRequest(Camera[] renderCams, int resultWidth, int resultHeight, ScreenCaptureQuality quality)
        {
            this.renderCams = renderCams;
            this.resultWidth = resultWidth;
            this.resultHeight = resultHeight;
            this.quality = quality;

            this.renderLayers = new RenderLayer[renderCams.Length];
            for (int i = 0; i < renderCams.Length; i++)
                renderLayers[i] = new RenderLayer();
        }

        public void Dispose()
        {
            isAborted = true;
            
            if (renderLayers != null)
            {
                foreach (RenderLayer layer in renderLayers)
                    layer.Dispose();
            }

            renderCams = null;
            renderLayers = null;
        }
    }
}