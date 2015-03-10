using System;
using UnityEngine;


namespace Frankfort.VBug.Internal
{
    public class MaterialDataSnapshot
    {
        public SMaterial[] materials;


        public static byte[] Serialize(MaterialDataSnapshot input)
        {
            if (input == null)
                return null;

            ComposedByteStream stream = ComposedByteStream.FetchStream();
            foreach (SMaterial material in input.materials)
                stream.AddStream(SMaterial.Serialize(material));

            return stream.Compose();
        }

        public static MaterialDataSnapshot Deserialize(byte[] input)
        {
            if (input == null || input.Length == 0)
                return null;

            MaterialDataSnapshot result = new MaterialDataSnapshot();
            ComposedByteStream stream = ComposedByteStream.FromByteArray(input);

            int iMax = stream.streamCount;
            result.materials = new SMaterial[iMax];
            for (int i = 0; i < iMax; i++)
                result.materials[i] = SMaterial.Deserialize(stream.ReadNextStream<byte>());

            stream.Dispose();
            return result;
        }

        internal void Dispose() {
            if (materials != null) {
                foreach (SMaterial mat in materials) {
                    if (mat != null)
                        mat.Dispose();
                }
            }
            materials = null;
        }
    }
}
