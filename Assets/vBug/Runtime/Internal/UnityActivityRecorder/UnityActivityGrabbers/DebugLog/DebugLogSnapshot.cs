using System;
using UnityEngine;

namespace Frankfort.VBug.Internal
{
    [Serializable]
    public class DebugLogCall
    {
        public LogType type;
        public double unixTimeStamp;
        public string logString;
        public string stackTrace;


        public DebugLogCall() //Default constructor.
        { }

        public DebugLogCall(LogType type, string logString, string stackTrace)
        {
            this.unixTimeStamp = vBugEnvironment.GetUnixTimestamp();
            this.type = type;
            this.logString = logString;
            this.stackTrace = stackTrace;
        }
        
        public static void Serialize(ref DebugLogCall input, ref ComposedByteStream storeTo)
        {
            byte[] header = new byte[9];

            header[0] = (byte)input.type;
            Buffer.BlockCopy(BitConverter.GetBytes(input.unixTimeStamp), 0, header, 1, 8);
            
            storeTo.AddStream(header);
            storeTo.AddStream(input.logString);
            storeTo.AddStream(input.stackTrace);
        }


        public static DebugLogCall Deserialize(ref ComposedByteStream stream)
        {
            DebugLogCall result = new DebugLogCall();
            byte[] header = stream.ReadNextStream<byte>();
            result.type = (LogType)header[0];
            result.unixTimeStamp = BitConverter.ToDouble(header, 1);
            result.logString = stream.ReadNextStream();
            result.stackTrace = stream.ReadNextStream();
            return result;
        }

        internal void Dispose() {
            logString = null;
            stackTrace = null;
        }
    }


    [Serializable]
    public class DebugLogSnapshot
    {
        public DebugLogCall[] calls;

        public DebugLogSnapshot() //Default constructor.
        { }

        public DebugLogSnapshot(DebugLogCall[] logs)
        {
            this.calls = logs;
        }

        public static byte[] Serialize(DebugLogSnapshot input)
        {
            ComposedByteStream stream = ComposedByteStream.FetchStream();
            for (int i = 0; i < input.calls.Length; i++)
                DebugLogCall.Serialize(ref input.calls[i], ref stream);

            return stream.Compose();
        }

        public static DebugLogSnapshot Deserialize(byte[] input)
        {
            if (input == null || input.Length == 0)
                return null;

            ComposedByteStream stream = ComposedByteStream.FromByteArray(input);
            if (stream == null)
                return null;

            int iMax = stream.streamCount / 3;
            DebugLogSnapshot result = new DebugLogSnapshot();
            result.calls = new DebugLogCall[iMax];

            for (int i = 0; i < iMax; i++)
                 result.calls[i] = DebugLogCall.Deserialize(ref stream);

            stream.Dispose();
            return result;
        }


        public void Dispose()
        {
            if (calls != null) {
                foreach (DebugLogCall call in calls) {
                    if (call != null)
                        call.Dispose();
                }
            }
            calls = null;
        }
    }
}