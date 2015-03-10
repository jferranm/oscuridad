using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Frankfort.VBug.Internal
{

    [Serializable]
    public class ComposedByteStream
    {
        public static bool UseBufferPool = false; //weird ass threading issues when set to TRUE;
        public static int GlobalMaxStreams = 8096;
        public static int GlobalMaxStreamSize = 16777216; //2048 * 2048 * 4 
        public static int GlobalMaxPoolBuffers = 4096;

        private static Dictionary<int, Queue<byte[]>> disposedBuffers = new Dictionary<int, Queue<byte[]>>();
        private static Dictionary<int, Queue<byte[]>> poolBuffers = new Dictionary<int, Queue<byte[]>>();
        private static Queue<ComposedByteStream> poolStreams = new Queue<ComposedByteStream>();

        private static long fetchedArrays = 0;
        private static int poolBufferCount = 0;
        private static int reusedBufferCount = 0;



        //--------------- Fetching --------------------
        public static ComposedByteStream FetchStream()
        {
            ComposedByteStream result = null;
            if (UseBufferPool && poolStreams.Count > 0)
                result = poolStreams.Dequeue(); //Is null sometimes -multithreading sizzle

            if (result == null) {
                result = new ComposedByteStream();
                result.isFetched = true;
            }
            return result;
        }

        public static ComposedByteStream FetchStream(int capacity)
        {
            ComposedByteStream result = null;
            if (UseBufferPool && poolStreams.Count > 0)
            {
                result = poolStreams.Dequeue();
                if (result == null)
                    result = new ComposedByteStream(capacity);

                result.references = new List<Array>(capacity);
            }
            else
            {
                result = new ComposedByteStream(capacity);
            }

            result.isFetched = true;
            return result;
        }

        public static void ReleaseStream(ComposedByteStream stream)
        {
            if (UseBufferPool && stream.isFetched)
                poolStreams.Enqueue(stream);
        }

        //--------------- Fetching --------------------
			




        //--------------- blit byte buffers --------------------
        public static void BlitBuffers() {
            if (!UseBufferPool)
                return;

            fetchedArrays = 0;
            poolBufferCount = 0;
            reusedBufferCount = 0;

            if (poolBuffers.Count > 0)
                poolBuffers.Clear();
            
            Dictionary<int, Queue<byte[]>> tempBuffer = poolBuffers;
            poolBuffers = disposedBuffers;
            disposedBuffers = tempBuffer;
        }


        private static byte[] FetchByteArray(int byteCount)
        {
            if (!UseBufferPool)
                return new byte[byteCount];

            fetchedArrays++;
            byte[] result = null;
            lock (poolBuffers) {
                if (poolBuffers.ContainsKey(byteCount)) {
                    Queue<byte[]> queue = poolBuffers[byteCount];
                    if (queue.Count > 0) {
                        reusedBufferCount++;
                        result = queue.Dequeue();
                    }
                }
            }
            return result != null ? result : new byte[byteCount];
        }

        private static void ReleaseByteArray(byte[] input)
        {
            if (!UseBufferPool)
                return;

            if(input == null|| input.Length == 0)
                return;

            int byteCount = input.Length;
            if (!disposedBuffers.ContainsKey(byteCount))
            {
                Queue<byte[]> queue = new Queue<byte[]>();
                queue.Enqueue(input);
                disposedBuffers.Add(byteCount, queue);
                poolBufferCount++;
            }
            else
            {
                if (poolBufferCount < GlobalMaxPoolBuffers)
                {
                    disposedBuffers[byteCount].Enqueue(input);
                    poolBufferCount++;
                }
            }
        }
        //--------------- blit byte buffers --------------------






        //--------------- Rebuilding --------------------
        public static ComposedByteStream FromByteArray(byte[] input)
        {
            if (input == null || input.Length == 0)
            {
                //Debug.LogError("ComposedByteStream.FromByteArray Error: input null or 0: " + (input == null ? "null" : input.Length.ToString()));
                return null;
            }

            if (input.Length <= 4)
            {
                //Debug.LogError("ComposedByteStream.FromByteArray Error: input to small: " + (input == null ? "null" : input.Length.ToString()));
                return null;
            }

            int maxStreams = BitConverter.ToInt32(input, 0);
            if (maxStreams < 0 || maxStreams > GlobalMaxStreams)
            {
                if (vBug.settings.general.debugMode)
                    Debug.LogError("ComposedByteStream.FromByteArray Error: maxStreams over the top: " + maxStreams + ", max: " + GlobalMaxStreams);
                return null;
            }

            ComposedByteStream result = ComposedByteStream.FetchStream();
            result.streamCount = maxStreams;
            result.composedData = input;
            
            result.sIdx = 0;
            result.lIdx = 4;
            result.bIdx = 4 + (result.streamCount * 4);

            return result;
        }
        //--------------- Rebuilding --------------------





        
        public int streamCount;
        protected bool isFetched;
        protected int sIdx;
        protected int lIdx;
        protected int bIdx;
        private byte[] composedData;
        private List<Array> references;


        //--------------- Constructors --------------------
        public ComposedByteStream() //Default constructor
        {
            references = new List<Array>();
        }
        public ComposedByteStream(int capacity)
        {
            references = new List<Array>(capacity);
        }
        //--------------- Constructor --------------------
		
	
        //--------------- Reset & Dispose --------------------
        public void Dispose()
        {
            if (composedData != null && isFetched)
                ReleaseByteArray(composedData);

            if (references != null)
                references.Clear();

            sIdx = 0;
            lIdx = 0;
            bIdx = 0;
            streamCount = 0;

            composedData = null;
            ReleaseStream(this);
        }
        //--------------- Reset & Dispose --------------------
			

        //--------------- Add streams --------------------
        public void AddStream(string input)
        {
            if(input == null || input.Length == 0) {
                AddEmptyStream();
            } else {
                AddStream(System.Text.Encoding.UTF8.GetBytes(input));
            }
        }

        public void AddStream(Array input)
        {
            if (input != null && input.Length == 0) {
                references.Add(null); //add empty stream instead.
            } else {
                references.Add(input); //add if null or useful value;
            }
        }

        public void AddEmptyStream()
        {
            references.Add(null);
        }
        public void AddEmptyStreams(int emptyStreamsCount)
        {
            while (--emptyStreamsCount > -1)
                references.Add(null);
        }
        //--------------- Add streams --------------------



        //--------------- JIT Build the ByteArray --------------------
        public byte[] Compose(bool dispose = true) {
            streamCount = references.Count;
            int byteCount = 0;

            //--------------- Pre calculate the size of the final byte-array --------------------
            int[] blockSizes = new int[streamCount];
            for (int i = 0; i < streamCount; i++)
            {
                Array reference = references[i];
                if (reference == null) {
                    blockSizes[i] = 0;
                } else {
                    int primSize = CalcPrimSize(reference.GetType().GetElementType());
                    
                    if (primSize > 0)
                    {
                        int blockSize = reference.Length * primSize;
                        byteCount += blockSize;
                        blockSizes[i] = blockSize;
                    }
                    else
                    {
                        blockSizes[i] = 0;
                    }
                }
            }
            //--------------- Pre calculate the size of the final byte-array --------------------


            //--------------- Set indexes to right positions --------------------
            sIdx = 0;
            lIdx = 4;
            bIdx = 4 + (streamCount * 4);
            byteCount += bIdx;
            //--------------- Set indexes to right positions --------------------
			
            
            //--------------- Copy all data --------------------
            composedData = FetchByteArray(byteCount);
            Buffer.BlockCopy(BitConverter.GetBytes(streamCount), 0, composedData, 0, 4); //copy length to header
                
            for (int i = 0; i < references.Count; i++) {
                Array reference = references[i];
                int blockSize = 0;

                if (reference != null) {
                    blockSize = blockSizes[i];
                    if (blockSize > 0) {
                        Buffer.BlockCopy(reference, 0, composedData, bIdx, blockSize);
                        bIdx += blockSize;
                    }
                }

                Buffer.BlockCopy(BitConverter.GetBytes(blockSize), 0, composedData, lIdx, 4); //copy length to header
                lIdx += 4;
            }
            //--------------- Copy all data --------------------

            byte[] resultBuffer = composedData;
            if (dispose)
                Dispose();

            return resultBuffer;
        }

        //--------------- JIT Build the ByteArray --------------------




        //--------------- Calc primitive size --------------------
        private int CalcPrimSize(Type type)
        {
            if (type == null)
                return 0;
            if (type == typeof(Byte))
                return 1;
            if (type == typeof(Int16) || type == typeof(UInt16))
                return 2;
            if (type == typeof(Boolean) || type == typeof(Single) || type == typeof(Int32) || type == typeof(UInt32))
                return 4;
            if (type == typeof(Double) || type == typeof(Int64) || type == typeof(UInt64))
                return 8;

            return 0;
        }
        //--------------- Calc primitive size --------------------



        //--------------- Get Streams --------------------
        public string ReadNextStream()
        {
            if (composedData == null)
                return null;

            byte[] data = ReadNextStream<byte>();
            return data != null ? System.Text.Encoding.UTF8.GetString(data) : null;
        }

        public T[] ReadNextStream<T>()where T: IConvertible
        {
            if (composedData == null)
            {
                throw new Exception("ComposedByteStream.FromByteArray Error:: composedData == null!");
            }

            if (streamCount == 0 || lIdx + 4 > composedData.Length) //its okay, happens when WriteEmptyStream was used.
            {
                lIdx += 4;
                return default(T[]);
            }

            int primSize = CalcPrimSize(typeof(T));
            if (primSize == 0)
            {
                throw new Exception("ComposedByteStream.FromByteArray Error:: Primitive size == 0!"); 
            }

            int streamLength = BitConverter.ToInt32(composedData, lIdx);
            lIdx += 4;
            if (streamLength < 0 || streamLength > GlobalMaxStreamSize)
            {
                throw new Exception("ComposedByteStream.ReadNextStream Error:: streamLength going wild:: streamLength: " + streamLength + ", max: " + GlobalMaxStreamSize);
            }

            if (bIdx + streamLength > composedData.Length)
            {
                throw new Exception("ComposedByteStream.ReadNextStream Error:: End of stream reached:: buffer Length: " + composedData.Length + ", bIdx: " + bIdx + ", streamLength: " + streamLength);
            }

            T[] result = new T[streamLength / primSize];
            Buffer.BlockCopy(composedData, bIdx, result, 0, streamLength);
            bIdx += streamLength;
            return result;
        }
        //--------------- Get Streams --------------------

    }
}
