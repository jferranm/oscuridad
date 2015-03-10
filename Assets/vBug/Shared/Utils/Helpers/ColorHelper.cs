using System;
using UnityEngine;


namespace Frankfort.VBug.Internal
{
    public static class ColorHelper
    {

        //
        public static int RGBA32_to_RGBA16(byte r, byte g, byte b, byte a)
        {
            return (r >> 4) | ((g >> 4) << 4) | ((b >> 4) << 8) | ((a >> 4) << 12);
        }
        public static void RGBA32_to_RGBA16(byte[] outputBuffer, ref int outputIdx, ref byte r, ref byte g, ref byte b, ref byte a)
        {
            outputBuffer[outputIdx++] = (byte)((r >> 4) | (g & 240)); // R & G
            outputBuffer[outputIdx++] = (byte)((b >> 4) | (a & 240)); // B & A
        }



        public static byte[] RGBA16_to_RGBA32(ref int rgba8bits)
        {
            return RGBA16_to_RGBA32((byte)(rgba8bits >> 4), (byte)((rgba8bits & 255) >> 8));
        }
        private static byte[] buffer4 = new byte[4];
        public static byte[] RGBA16_to_RGBA32(byte x, byte y)
        {
            //240 == 255 - 15 = 11110000
            buffer4[0] = (byte)(x << 4); // r
            buffer4[1] = (byte)(x & 240); // g
            buffer4[2] = (byte)(y << 4); // b
            buffer4[3] = (byte)(y & 240); // a
            return buffer4;
        }
    }
}