/*
 * Improved version to C# LibLZF Port:
 * Copyright (c) 2010 Roman Atachiants <kelindar@gmail.com>
 * 
 * Original CLZF Port:
 * Copyright (c) 2005 Oren J. Maurice <oymaurice@hazorea.org.il>
 * 
 * Original LibLZF Library & Algorithm:
 * Copyright (c) 2000-2008 Marc Alexander Lehmann <schmorp@schmorp.de>
 * 
 * Redistribution and use in source and binary forms, with or without modifica-
 * tion, are permitted provided that the following conditions are met:
 * 
 *   1.  Redistributions of source code must retain the above copyright notice,
 *       this list of conditions and the following disclaimer.
 * 
 *   2.  Redistributions in binary form must reproduce the above copyright
 *       notice, this list of conditions and the following disclaimer in the
 *       documentation and/or other materials provided with the distribution.
 * 
 *   3.  The name of the author may not be used to endorse or promote products
 *       derived from this software without specific prior written permission.
 * 
 * THIS SOFTWARE IS PROVIDED BY THE AUTHOR ``AS IS'' AND ANY EXPRESS OR IMPLIED
 * WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MER-
 * CHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED.  IN NO
 * EVENT SHALL THE AUTHOR BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPE-
 * CIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO,
 * PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS;
 * OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY,
 * WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTH-
 * ERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED
 * OF THE POSSIBILITY OF SUCH DAMAGE.
 *
 * Alternatively, the contents of this file may be used under the terms of
 * the GNU General Public License version 2 (the "GPL"), in which case the
 * provisions of the GPL are applicable instead of the above. If you wish to
 * allow the use of your version of this file only under the terms of the
 * GPL and not to allow others to use your version of this file under the
 * BSD license, indicate your decision by deleting the provisions above and
 * replace them with the notice and other provisions required by the GPL. If
 * you do not delete the provisions above, a recipient may use your version
 * of this file under either the BSD or the GPL.
 */
using System;
using System.Text;
using UnityEngine;
using System.Collections.Generic;

/* Benchmark with Alice29 Canterbury Corpus
        ---------------------------------------
        (Compression) Original CLZF C#
        Raw = 152089, Compressed = 101092
         8292,4743 ms.
        ---------------------------------------
        (Compression) My LZF C#
        Raw = 152089, Compressed = 101092
         33,0019 ms.
        ---------------------------------------
        (Compression) Zlib using SharpZipLib
        Raw = 152089, Compressed = 54388
         8389,4799 ms.
        ---------------------------------------
        (Compression) QuickLZ C#
        Raw = 152089, Compressed = 83494
         80,0046 ms.
        ---------------------------------------
        (Decompression) Original CLZF C#
        Decompressed = 152089
         16,0009 ms.
        ---------------------------------------
        (Decompression) My LZF C#
        Decompressed = 152089
         15,0009 ms.
        ---------------------------------------
        (Decompression) Zlib using SharpZipLib
        Decompressed = 152089
         3577,2046 ms.
        ---------------------------------------
        (Decompression) QuickLZ C#
        Decompressed = 152089
         21,0012 ms.
    */






/// <summary>
/// Improved C# LZF Compressor, a very small data compression library. The compression algorithm is extremely fast. 
namespace Frankfort.VBug.Internal
{
    public class CLZF
    {

        //--------------- Michiel Frankfort added --------------------
        private const int startRato = 1;
        private const int incrementRato = 2;


        private static int compressByteCountGuess = -1;
        private static int decompressByteCountGuess = -1;
        private static Queue<CLZF> queue = new Queue<CLZF>();

        private static CLZF Fetch()
        {
            CLZF result;
            lock (queue)
            {
                if (queue.Count > 0)
                {
                    result = queue.Dequeue();
                }
                else
                {
                    result = new CLZF();
                }
            }
            return result;
        }

        private static void Store(CLZF instance)
        {
            lock (queue)
            {
                if (!queue.Contains(instance))
                    queue.Enqueue(instance);
            }
        }

        public static byte[] Compress(byte[] input, uint startIdx = 0)
        {
            if(input == null)
            {
                if (vBug.settings.general.debugMode)
                    Debug.LogError("Compress error: input == null!");
                return null;
            }
            if (input.Length <= Math.Max(0, startIdx))
            {
                if (vBug.settings.general.debugMode)
                    Debug.LogError("Compress error: input.length <= startIdx! startIdx: " + startIdx + ", length: " + input.Length);
                return null;
            }
            
            CLZF instance = CLZF.Fetch();
            byte[] result = instance.CompressData(input, startIdx);
            CLZF.Store(instance);
            return result;
        }

        public static byte[] Decompress(byte[] input, uint startIdx = 0)
        {
            if (input == null)
            {
                if (vBug.settings.general.debugMode)
                    Debug.LogError("Decompress error: input == null!");
                return null;
            }
            if (input.Length <= Math.Max(0, startIdx))
            {
                if (vBug.settings.general.debugMode)
                    Debug.LogError("Decompress error: input.length < startIdx! \nstartIdx: " + startIdx + ", length: " + input.Length);
                return null;
            }
            CLZF instance = CLZF.Fetch();
            byte[] result = instance.DecompressData(input, startIdx);
            CLZF.Store(instance);
            return result;
        }

        private byte[] compressByteBuffer;
        private byte[] decompressByteBuffer;
        //--------------- Michiel Frankfort added --------------------




        private const uint HLOG = 14;
        private const uint HSIZE = (1 << 14);
        private const uint MAX_LIT = (1 << 5);
        private const uint MAX_OFF = (1 << 13);
        private const uint MAX_REF = ((1 << 8) + (1 << 3));

        /// <summary>
        /// Hashtable, that can be allocated only once
        /// </summary>
        private readonly long[] HashTable = new long[HSIZE];

        // Compresses inputBytes
        private byte[] CompressData(byte[] inputBytes, uint startIdx)
        {
            // Starting guess, increase it later if needed
            if (compressByteCountGuess == -1)
                compressByteCountGuess = inputBytes.Length * startRato;

            if (compressByteBuffer == null || compressByteBuffer.Length != compressByteCountGuess)
                compressByteBuffer = new byte[compressByteCountGuess];

            int byteCount = lzf_compress(inputBytes, ref compressByteBuffer, startIdx);

            // If byteCount is 0, then increase buffer and try again
            while (byteCount == 0)
            {
                compressByteCountGuess *= incrementRato;
                compressByteBuffer = new byte[compressByteCountGuess];
                byteCount = lzf_compress(inputBytes, ref compressByteBuffer, startIdx);

                if (vBug.settings.general.debugMode)
                    Debug.Log("Compress: Increased compressByteCountGuess: " + compressByteCountGuess + " bytes, queue size: " + queue.Count);
            }

            byte[] outputBytes = new byte[byteCount];
            Buffer.BlockCopy(compressByteBuffer, 0, outputBytes, 0, byteCount);
            return outputBytes;
        }

        // Decompress outputBytes
        private byte[] DecompressData(byte[] inputBytes, uint startIdx)
        {
            // Starting guess, increase it later if needed
            if (decompressByteCountGuess == -1)
                decompressByteCountGuess = inputBytes.Length * startRato;

            if (decompressByteBuffer == null || decompressByteBuffer.Length != decompressByteCountGuess)
                decompressByteBuffer = new byte[decompressByteCountGuess];

            int byteCount = lzf_decompress(inputBytes, ref decompressByteBuffer, startIdx);

            // If byteCount is 0, then increase buffer and try again
            while (byteCount == 0)
            {
                decompressByteCountGuess *= incrementRato;
                if (decompressByteCountGuess > 0)
                {
                    if (vBug.settings.general.debugMode)
                        Debug.Log("Decompress: Increased decompressByteCountGuess: " + decompressByteCountGuess + " bytes, queue size: " + queue.Count);
                    decompressByteBuffer = new byte[decompressByteCountGuess];
                    byteCount = lzf_decompress(inputBytes, ref decompressByteBuffer, startIdx);
                }
            }

            byte[] outputBytes = new byte[byteCount];
            Buffer.BlockCopy(decompressByteBuffer, 0, outputBytes, 0, byteCount);
            return outputBytes;
        }


        /// <summary>
        /// Compresses the data using LibLZF algorithm
        /// </summary>
        /// <param name="input">Reference to the data to compress</param>
        /// <param name="output">Reference to a buffer which will contain the compressed data</param>
        /// <returns>The size of the compressed archive in the output buffer</returns>
        public int lzf_compress(byte[] input, ref byte[] output, uint startIdx)
        {
            int inputLength = input.Length;
            int outputLength = output.Length;

            Array.Clear(HashTable, 0, (int)HSIZE);

            if(startIdx != 0)
                Buffer.BlockCopy(input, 0, output, 0, (int)startIdx);

            long hslot;
            uint iidx = startIdx;
            uint oidx = startIdx;
            long reference;

            uint hval = (uint)(((input[iidx]) << 8) | input[iidx + 1]); // FRST(in_data, iidx);
            long off;
            int lit = 0;

            for (; ; )
            {
                if (iidx < inputLength - 2)
                {
                    hval = (hval << 8) | input[iidx + 2];
                    hslot = ((hval ^ (hval << 5)) >> (int)(((3 * 8 - HLOG)) - hval * 5) & (HSIZE - 1));
                    reference = HashTable[hslot];
                    HashTable[hslot] = (long)iidx;


                    if ((off = iidx - reference - 1) < MAX_OFF
                        && iidx + 4 < inputLength
                        && reference > 0
                        && input[reference + 0] == input[iidx + 0]
                        && input[reference + 1] == input[iidx + 1]
                        && input[reference + 2] == input[iidx + 2]
                        )
                    {
                        /* match found at *reference++ */
                        uint len = 2;
                        uint maxlen = (uint)inputLength - iidx - len;
                        maxlen = maxlen > MAX_REF ? MAX_REF : maxlen;

                        if (oidx + lit + 1 + 3 >= outputLength)
                            return 0;

                        do
                            len++;
                        while (len < maxlen && input[reference + len] == input[iidx + len]);

                        if (lit != 0)
                        {
                            output[oidx++] = (byte)(lit - 1);
                            lit = -lit;
                            do
                                output[oidx++] = input[iidx + lit];
                            while ((++lit) != 0);
                        }

                        len -= 2;
                        iidx++;

                        if (len < 7)
                        {
                            output[oidx++] = (byte)((off >> 8) + (len << 5));
                        }
                        else
                        {
                            output[oidx++] = (byte)((off >> 8) + (7 << 5));
                            output[oidx++] = (byte)(len - 7);
                        }

                        output[oidx++] = (byte)off;

                        iidx += len - 1;
                        hval = (uint)(((input[iidx]) << 8) | input[iidx + 1]);

                        hval = (hval << 8) | input[iidx + 2];
                        HashTable[((hval ^ (hval << 5)) >> (int)(((3 * 8 - HLOG)) - hval * 5) & (HSIZE - 1))] = iidx;
                        iidx++;

                        hval = (hval << 8) | input[iidx + 2];
                        HashTable[((hval ^ (hval << 5)) >> (int)(((3 * 8 - HLOG)) - hval * 5) & (HSIZE - 1))] = iidx;
                        iidx++;
                        continue;
                    }
                }
                else if (iidx == inputLength)
                    break;

                /* one more literal byte we must copy */
                lit++;
                iidx++;

                if (lit == MAX_LIT)
                {
                    if (oidx + 1 + MAX_LIT >= outputLength)
                        return 0;

                    output[oidx++] = (byte)(MAX_LIT - 1);
                    lit = -lit;
                    do
                        output[oidx++] = input[iidx + lit];
                    while ((++lit) != 0);
                }
            }

            if (lit != 0)
            {
                if (oidx + lit + 1 >= outputLength)
                    return 0;

                output[oidx++] = (byte)(lit - 1);
                lit = -lit;
                do
                    output[oidx++] = input[iidx + lit];
                while ((++lit) != 0);
            }

            return (int)oidx;
        }


        /// <summary>
        /// Decompresses the data using LibLZF algorithm
        /// </summary>
        /// <param name="input">Reference to the data to decompress</param>
        /// <param name="output">Reference to a buffer which will contain the decompressed data</param>
        /// <returns>Returns decompressed size</returns>
        public int lzf_decompress(byte[] input, ref byte[] output, uint startIdx)
        {
            int inputLength = input.Length;
            int outputLength = output.Length;

            if (startIdx != 0)
                Buffer.BlockCopy(input, 0, output, 0, (int)startIdx);

            uint iidx = startIdx;
            uint oidx = startIdx;

            do
            {
                uint ctrl = input[iidx++];

                if (ctrl < (1 << 5)) /* literal run */
                {
                    ctrl++;

                    if (oidx + ctrl > outputLength)
                    {
                        //SET_ERRNO (E2BIG);
                        return 0;
                    }

                    do
                        output[oidx++] = input[iidx++];
                    while ((--ctrl) != 0);
                }
                else /* back reference */
                {
                    uint len = ctrl >> 5;

                    int reference = (int)(oidx - ((ctrl & 0x1f) << 8) - 1);

                    if (len == 7)
                        len += input[iidx++];

                    reference -= input[iidx++];

                    if (oidx + len + 2 > outputLength)
                    {
                        //SET_ERRNO (E2BIG);
                        return 0;
                    }

                    if (reference < 0)
                    {
                        //SET_ERRNO (EINVAL);
                        return 0;
                    }

                    output[oidx++] = output[reference++];
                    output[oidx++] = output[reference++];

                    do
                        output[oidx++] = output[reference++];
                    while ((--len) != 0);
                }
            }
            while (iidx < inputLength);

            return (int)oidx;
        }
    }
}