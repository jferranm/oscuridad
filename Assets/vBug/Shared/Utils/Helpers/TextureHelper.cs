using UnityEngine;
using System;
using System.Collections.Generic;
using Frankfort.VBug.Editor;

namespace Frankfort.VBug.Internal
{

    public static class TextureHelper
    {

        /// <summary>
        /// 1st int16 [0, 1] == width
        /// 2nd int16 [2, 3] == height
        /// 2nd int16 [4] == GetBytesMode enum
        /// byte[5] to N == Color32 data.
        /// </summary>
        /// <param name="texture"></param>
        /// <returns></returns>
        public static byte[] GetBytes(Color32[] pixels, int textureWidth, int textureHeight, int scaleDown = 1, ScreenCaptureQuality quality = ScreenCaptureQuality.rawGray8)
        {
            scaleDown = Mathf.Max(1, scaleDown);
            Int16 targetWidth = (Int16)Mathf.Ceil((float)textureWidth / (float)scaleDown);
            Int16 targetHeight = (Int16)Mathf.Ceil((float)textureHeight / (float)scaleDown);
            int pMax = targetWidth * targetHeight;

            switch (quality)
            {
                case ScreenCaptureQuality.rawRGBA32:
                case ScreenCaptureQuality.lzfRGBA32:
                    pMax *= 4; break;
                case ScreenCaptureQuality.rawRGB24:
                case ScreenCaptureQuality.lzfRGB24:
                    pMax *= 3; break;
                case ScreenCaptureQuality.rawRGBA16:
                case ScreenCaptureQuality.lzfRGBA16:
                    pMax *= 2; break;
            }

            int iMax = pMax + 5;
            byte[] result = new byte[iMax];

            result[0] = (byte)targetWidth;
            result[1] = (byte)(targetWidth >> 8);
            result[2] = (byte)targetHeight;
            result[3] = (byte)(targetHeight >> 8);
            result[4] = (byte)quality;

            int i = 5;
            int p = 0;
            int currentX = 0;
            int rowStart = 0;
            //byte alpha = 255;


            //--------------- Pinning --------------------
            //GCHandle resultPin = GCHandle.Alloc(result, GCHandleType.Pinned);
            //GCHandle pixelsPin = GCHandle.Alloc(pixels, GCHandleType.Pinned);
            //--------------- Pinning --------------------
			

            switch (quality)
            {
                case ScreenCaptureQuality.rawRGBA32:
                case ScreenCaptureQuality.lzfRGBA32:
                    if (scaleDown == 1){
                        while (i < iMax){
                            Color32 c = pixels[p++];
                            result[i++] = c.r; result[i++] = c.g; result[i++] = c.b; result[i++] = c.a;//(byte)(Mathf.Min(255, (int)color.a * 2));
                        }
                    } else {
                        while (i < iMax){
                            Color32 c = pixels[p]; IncrementP_Color32(ref p, ref currentX, ref rowStart, ref scaleDown, ref textureWidth);
                            result[i++] = c.r; result[i++] = c.g; result[i++] = c.b; result[i++] = c.a;//(byte)(Mathf.Min(255, (int)color.a * 2));
                        }
                    }
                    break;

                case ScreenCaptureQuality.rawRGB24:
                case ScreenCaptureQuality.lzfRGB24:
                    if (scaleDown == 1) {
                        while (i < iMax) {
                            Color32 color = pixels[p++];
                            result[i++] = color.r; result[i++] = color.g; result[i++] = color.b;
                        }
                    } else {
                        while (i < iMax) {
                            Color32 color = pixels[p]; IncrementP_Color32(ref p, ref currentX, ref rowStart, ref scaleDown, ref textureWidth);
                            result[i++] = color.r; result[i++] = color.g; result[i++] = color.b;
                        }
                    }
                    break;

                case ScreenCaptureQuality.rawRGBA16:
                case ScreenCaptureQuality.lzfRGBA16:
                    if (scaleDown == 1) {
                        while (i < iMax) {
                            Color32 c = pixels[p++];
                            result[i++] = (byte)((c.r >> 4) | (c.g & 240)); result[i++] = (byte)((c.b >> 4) | (c.a & 240));    
                        }
                    } else {
                        while (i < iMax) {
                            Color32 c = pixels[p]; IncrementP_Color32(ref p, ref currentX, ref rowStart, ref scaleDown, ref textureWidth);
                            result[i++] = (byte)((c.r >> 4) | (c.g & 240)); result[i++] = (byte)((c.b >> 4) | (c.a & 240));
                        }
                    }
                    break;

                case ScreenCaptureQuality.rawGray8:
                case ScreenCaptureQuality.lzfGray8:
                    if(scaleDown == 1) {
                        while (i < iMax)
                            result[i++] = pixels[p++].r;
                    } else {
                        while (i < iMax) {
                            result[i++] = pixels[p].r;
                            IncrementP_Color32(ref p, ref currentX, ref rowStart, ref scaleDown, ref textureWidth);
                        }
                    }

                    break;
            }

            //pixelsPin.Free();
            if (quality.ToString().Contains("lzf"))
                result = CLZF.Compress(result, 5);

            //resultPin.Free();
            return result;
        }

        //Everything ref saves performance
        private static void IncrementP_Color32(ref int outP, ref int x, ref int rowStart, ref int scaleDown, ref int textureWidth){
            x += scaleDown;
            if (x >= textureWidth){
                x = 0;
                rowStart += textureWidth * scaleDown;
            }
            outP = rowStart + x;
        }










        //TODO: ColorHelper static call eruit, scheelt erg veel overhead!
        public static Texture2D SetBytes(byte[] input, bool hasAlpha)
        {

            if (input == null || input.Length <= 5){
                if (vBugEditorSettings.DebugMode)
                    Debug.LogError("Texture2D.SetBytes error! bytes null or empty");
                return null;
            }
            int width = input[0] | (input[1] << 8);
            int height = input[2] | (input[3] << 8);
            ScreenCaptureQuality quality = (ScreenCaptureQuality)input[4];

            if (width < 32 || width > 4096 || height < 32 || height > 4096){
                if (vBugEditorSettings.DebugMode)
                    Debug.LogError("Texture2D.SetBytes error! width: " + width + ", height: " + height);
                return null;
            }

            if (quality.ToString().Contains("lzf"))
                input = CLZF.Decompress(input, 5);

            int p = 5;
            int i = 0;
            int iMax = width * height;
            int bytesBufferSize = input.Length - 5;
            Color32[] colors = new Color32[iMax];

            byte bMax = 255;
            switch (quality)
            {
                case ScreenCaptureQuality.rawGray8:
                case ScreenCaptureQuality.lzfGray8:
                    while (i < iMax) {
                        byte singleChannel = input[p++];
                        colors[i++] = new Color32(singleChannel, singleChannel, singleChannel, bMax);
                    }
                    break;

                case ScreenCaptureQuality.rawRGBA16:
                case ScreenCaptureQuality.lzfRGBA16:
                    while (i < iMax) {
                        byte[] color = ColorHelper.RGBA16_to_RGBA32(input[p++], input[p++]);
                        colors[i++] = new Color32(color[0], color[1], color[2], hasAlpha ? color[3] : bMax);
                    }
                    break;

                case ScreenCaptureQuality.rawRGB24:
                case ScreenCaptureQuality.lzfRGB24:
                    while (i < iMax)
                        colors[i++] = new Color32(input[p++], input[p++], input[p++], bMax);
                    break;

                case ScreenCaptureQuality.rawRGBA32:
                case ScreenCaptureQuality.lzfRGBA32:
                    while (i < iMax) {
                        if (hasAlpha) {
                            colors[i++] = new Color32(input[p++], input[p++], input[p++], input[p++]);
                        } else {
                            colors[i++] = new Color32(input[p++], input[p++], input[p++], bMax);
                            p++;
                        }
                    }
                    break;

                default:
                    if (vBugEditorSettings.DebugMode)
                        Debug.LogError("Texture2D.SetBytes error! width: " + width + ", height: " + height + ", bytes available: " + bytesBufferSize);
                    return null;
            }

            Texture2D result = TexturePool.GetTexture2D(width, height);
            result.name = "vBugTexture";
            result.hideFlags = HideFlags.HideAndDontSave;
            result.SetPixels32(colors);
            result.Apply(false, false);
            result.filterMode = FilterMode.Trilinear;

            colors = null;
            input = null;
            return result;
        }
    }

}