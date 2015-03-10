using System;
using System.Collections.Generic;

namespace Frankfort.VBug.Internal
{
    public class ComposedPrimitives
    {
        private List<IComparable> input = new List<IComparable>();
        protected byte[] output;
        private int outputIdx;

        private enum PrimType
        {
            _enum,
            _byte,
            _bool,
            _Int16,
            _Int32,
            _Int64,
            _UInt16,
            _UInt32,
            _UInt64,
            _Single,
            _Double
        }

        public static ComposedPrimitives FromByteArray(byte[] output)
        {
            if (output == null)
                return null;

            ComposedPrimitives result = new ComposedPrimitives();
            result.output = output;
            return result;
        }


        public void AddValue(IComparable value)
        {
            input.Add(value);
        }

        public byte[] Compose()
        {
            int iMax = input.Count;
            PrimType[] primTypes = new PrimType[iMax];

            //--------------- Get total bytearray size --------------------
            int totalSize = 0;

            int i = iMax;
            while (--i > -1)
            {
                Type type = input[i].GetType();
                
                if (type == typeof(Byte)){
                    totalSize += 1; primTypes[i] = PrimType._byte;
                } else if (type == typeof(Boolean)) {
                    totalSize += 1; primTypes[i] = PrimType._bool;
                } else if (type.IsEnum) {
                    totalSize += 4; primTypes[i] = PrimType._enum;
                } else if (type == typeof(Int16)) {
                    totalSize += 2; primTypes[i] = PrimType._Int16;
                } else if (type == typeof(Int32)){
                    totalSize += 4; primTypes[i] = PrimType._Int32;
                } else if (type == typeof(Int64)){
                    totalSize += 8; primTypes[i] = PrimType._Int64;
                } else if (type == typeof(UInt16)){
                    totalSize += 2; primTypes[i] = PrimType._UInt16;
                } else if (type == typeof(UInt32)){
                    totalSize += 4; primTypes[i] = PrimType._UInt32;
                } else if (type == typeof(UInt64)){
                    totalSize += 8; primTypes[i] = PrimType._UInt64;
                } else if (type == typeof(Single)){
                    totalSize += 4; primTypes[i] = PrimType._Single;
                } else if (type == typeof(Double)){
                    totalSize += 8; primTypes[i] = PrimType._Double;
                }
            }
            //--------------- Get total bytearray size --------------------


            //--------------- Copy values to new bytearray --------------------
            output = new byte[totalSize];

            int idx = 0;
            for(i = 0; i < iMax; i++)
            {
                PrimType type = primTypes[i];
                switch(type)
                {
                    case PrimType._byte:    output[idx] = (byte)input[i]; idx += 1; break;
                    case PrimType._bool:    output[idx] = (bool)input[i] == true ? (byte)1 : (byte)0;                       idx += 1; break;
                    case PrimType._enum:    Buffer.BlockCopy(BitConverter.GetBytes((Int32)input[i]), 0, output, idx, 4);    idx += 4; break;
                    case PrimType._Int16:   Buffer.BlockCopy(BitConverter.GetBytes((Int16)input[i]), 0, output, idx, 2);    idx += 2; break;
                    case PrimType._Int32:   Buffer.BlockCopy(BitConverter.GetBytes((Int32)input[i]), 0, output, idx, 4);    idx += 4; break;
                    case PrimType._Int64:   Buffer.BlockCopy(BitConverter.GetBytes((Int64)input[i]), 0, output, idx, 8);    idx += 8; break;
                    case PrimType._UInt16:  Buffer.BlockCopy(BitConverter.GetBytes((UInt16)input[i]), 0, output, idx, 2);   idx += 2; break;
                    case PrimType._UInt32:  Buffer.BlockCopy(BitConverter.GetBytes((UInt32)input[i]), 0, output, idx, 4);   idx += 4; break;
                    case PrimType._UInt64:  Buffer.BlockCopy(BitConverter.GetBytes((UInt64)input[i]), 0, output, idx, 8);   idx += 8; break;
                    case PrimType._Single:  Buffer.BlockCopy(BitConverter.GetBytes((Single)input[i]), 0, output, idx, 4);   idx += 4; break;
                    case PrimType._Double:  Buffer.BlockCopy(BitConverter.GetBytes((Double)input[i]), 0, output, idx, 8);   idx += 8; break;
                }
            }
            //--------------- Copy values to new bytearray --------------------

            byte[] result = output;
            Dispose();

            return result;
        }

        public T ReadNextValue<T>() where T : IConvertible
        {
            if (output == null)
                return default(T);

            Type type = typeof(T);
            IConvertible value = 0;
            if (type == typeof(byte))   {value  = output[outputIdx];                            outputIdx += 1; return (T)value; }
            if (type == typeof(bool))   {value  = output[outputIdx] == (byte)1 ? true : false;  outputIdx += 1; return (T)value; }
            if (type.IsEnum)            {value  = BitConverter.ToInt32(output, outputIdx);      outputIdx += 4; return (T)value; }
            if (type == typeof(Int16))  {value  = BitConverter.ToInt16(output, outputIdx);      outputIdx += 2; return (T)value; }
            if (type == typeof(Int32))  {value  = BitConverter.ToInt32(output, outputIdx);      outputIdx += 4; return (T)value; }
            if (type == typeof(Int64))  {value  = BitConverter.ToInt64(output, outputIdx);      outputIdx += 5; return (T)value; }
            if (type == typeof(UInt16)) {value  = BitConverter.ToUInt16(output, outputIdx);     outputIdx += 2; return (T)value; }
            if (type == typeof(UInt32)) {value  = BitConverter.ToUInt32(output, outputIdx);     outputIdx += 4; return (T)value; }
            if (type == typeof(UInt64)) {value  = BitConverter.ToUInt64(output, outputIdx);     outputIdx += 8; return (T)value; }
            if (type == typeof(Single)) {value  = BitConverter.ToSingle(output, outputIdx);     outputIdx += 4; return (T)value; }
            if (type == typeof(Double)) {value  = BitConverter.ToDouble(output, outputIdx);     outputIdx += 8; return (T)value; }

            return default(T);
        }



        //--------------- Reset & Dispose -------------------- 
        public void Reset()
        {
            outputIdx = 0;
        }

        //TODO: Implement everywhere!
        public void Dispose()
        {
            if (input != null)
                input.Clear();

            input = null;
            output = null;
        } 
        //--------------- Reset & Dispose --------------------
			
    }
}
