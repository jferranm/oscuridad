using UnityEngine;
using System;
using System.Collections.Generic;


namespace Frankfort.VBug.Internal
{

    //--------------------------------------- STRUCTS --------------------------------------
    //--------------------------------------- STRUCTS --------------------------------------
    #region STRUCTS
    
    
    [Serializable]
    public struct SVector2
    {
        public float x;
        public float y;

        public SVector2(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        public static SVector2 zero
        {
            get { return new SVector2(); }
        }

        public static implicit operator SVector2(Vector2 value)
        {
            return new SVector2(value.x, value.y);
        }
        public static implicit operator Vector2(SVector2 value)
        {
            return new Vector2(value.x, value.y);
        }

        public override string ToString()
        {
            return x + "-" + y;
        }
    }



    [Serializable]
    public struct SVector3
    {
        public float x;
        public float y;
        public float z;

        public SVector3(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public static implicit operator SVector3(Vector3 value)
        {
            return new SVector3(value.x, value.y, value.z);
        }
        public static implicit operator Vector3(SVector3 value)
        {
            return new Vector3(value.x, value.y, value.z);
        }

        public override string ToString()
        {
            return x + "-" + y + "-" + z;
        }
    }


    [Serializable]
    public struct SVector4
    {
        public float x;
        public float y;
        public float z;
        public float w;

        public SVector4(float x, float y, float z, float w)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }

        public static implicit operator SVector4(Vector4 value)
        {
            return new SVector4(value.x, value.y, value.z, value.w);
        }
        public static implicit operator Vector4(SVector4 value)
        {
            return new Vector4(value.x, value.y, value.z, value.w);
        }

        public override string ToString()
        {
            return x + "-" + y + "-" + z + "-" + w;
        }
    }


    [Serializable]
    public struct SColor
    {
        public float r;
        public float g;
        public float b;
        public float a;

        public SColor(float r, float g, float b, float a)
        {
            this.r = r;
            this.g = g;
            this.b = b;
            this.a = a;
        }

        public static implicit operator SColor(Color value)
        {
            return new SColor(value.r, value.g, value.b, value.a);
        }
        public static implicit operator Color(SColor value)
        {
            return new Color(value.r, value.g, value.b, value.a);
        }

        public override string ToString()
        {
            return r + "-" + g + "-" + b + "-" + a;
        }
    }

    [Serializable]
    public struct SQuaternion
    {
        public float x;
        public float y;
        public float z;
        public float w;

        public SQuaternion(float x, float y, float z, float w)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }

        public static implicit operator SQuaternion(Quaternion value)
        {
            return new SQuaternion(value.x, value.y, value.z, value.w);
        }
        public static implicit operator Quaternion(SQuaternion value)
        {
            return new Quaternion(value.x, value.y, value.z, value.w);
        }

        public override string ToString()
        {
            return x + "-" + y + "-" + z + "-" + w;
        }
    }


    [Serializable]
    public struct SBounds
    {
        public SVector3 center;
        public SVector3 extents;

        public SBounds(SVector3 center, SVector3 extents)
        {
            this.center = center;
            this.extents = extents;
        }

        public static implicit operator SBounds(Bounds value)
        {
            return new SBounds(value.center, value.extents);
        }
        public static implicit operator Bounds(SBounds value)
        {
            return new Bounds(value.center, (Vector3)value.extents * 2);
        }

        public override string ToString()
        {
            return "center: " + center + ", extents: " + extents;
        }
    }


    [Serializable]
    public struct SMatrix4x4
    {
        public float m00;
        public float m01;
        public float m02;
        public float m03;
        public float m10;
        public float m11;
        public float m12;
        public float m13;
        public float m20;
        public float m21;
        public float m22;
        public float m23;
        public float m30;
        public float m31;
        public float m32;
        public float m33;


        public SMatrix4x4(float[] data){
            m00 = data[0]; m01 = data[1]; m02 = data[2]; m03 = data[3];
            m10 = data[4]; m11 = data[5]; m12 = data[6]; m13 = data[7];
            m20 = data[8]; m21 = data[9]; m22 = data[10]; m23 = data[11];
            m30 = data[12]; m31 = data[13]; m32 = data[14]; m33 = data[15];
        }

        public SMatrix4x4(Matrix4x4 matrix){
            m00 = matrix.m00; m01 = matrix.m01; m02 = matrix.m02; m03 = matrix.m03;
            m10 = matrix.m10; m11 = matrix.m11; m12 = matrix.m12; m13 = matrix.m13;
            m20 = matrix.m20; m21 = matrix.m21; m22 = matrix.m22; m23 = matrix.m23;
            m30 = matrix.m30; m31 = matrix.m31; m32 = matrix.m32; m33 = matrix.m33;
        }

        public static implicit operator SMatrix4x4(Matrix4x4 value){
            return new SMatrix4x4(value);
        }

        public static implicit operator Matrix4x4(SMatrix4x4 value){
            Matrix4x4 result = new Matrix4x4();
            result.m00 = value.m00; result.m01 = value.m01; result.m02 = value.m02; result.m03 = value.m03;
            result.m10 = value.m10; result.m11 = value.m11; result.m12 = value.m12; result.m13 = value.m13;
            result.m20 = value.m20; result.m21 = value.m21; result.m22 = value.m22; result.m23 = value.m23;
            result.m30 = value.m30; result.m31 = value.m31; result.m32 = value.m32; result.m33 = value.m33;
            return result;
        }

        public Vector3 position {
            get {
                return new Vector3(m03, m13, m23);
            }
        }

        internal float[] GetRawData() {
            return new float[]{
                m00, m01, m02, m03,
                m10, m11, m12, m13,
                m20, m21, m22, m23,
                m30, m31, m32, m33
            };
        }
    }



    #endregion
    //--------------------------------------- STRUCTS --------------------------------------
    //--------------------------------------- STRUCTS --------------------------------------













    //--------------------------------------- CLASSES --------------------------------------
    //--------------------------------------- CLASSES --------------------------------------
    #region CLASSES


    public class SMaterial
    {
        public int instanceID;
        public string materialName;
        public string shaderName;
        public byte[] rawData;

        public static byte[] Serialize(SMaterial input)
        {
            if (input == null)
                return null;

            ComposedByteStream stream = ComposedByteStream.FetchStream();
            stream.AddStream(BitConverter.GetBytes(input.instanceID));
            stream.AddStream(input.materialName);
            stream.AddStream(input.shaderName);
            stream.AddStream(input.rawData);
            return stream.Compose();
        }

        public static SMaterial Deserialize(byte[] input)
        { 
            if(input == null || input.Length == 0)
                return null;

            ComposedByteStream stream = ComposedByteStream.FromByteArray(input);
            SMaterial result = new SMaterial();

            result.instanceID = BitConverter.ToInt32(stream.ReadNextStream<byte>(), 0);
            result.materialName = stream.ReadNextStream();
            result.shaderName = stream.ReadNextStream();
            result.rawData = stream.ReadNextStream<byte>();
            stream.Dispose();

            return result;
        }

        internal void Dispose() {
            materialName = null;
            shaderName = null;
            rawData = null;
        }
    }

    #endregion
    //--------------------------------------- CLASSES --------------------------------------
    //--------------------------------------- CLASSES --------------------------------------













    //--------------------------------------- ARRAYS --------------------------------------
    //--------------------------------------- ARRAYS --------------------------------------
    #region ARRAYS

    [Serializable]
    public class SMatrix4x4Array
    {
        public int count;
        public float[] matrixData;

        public SMatrix4x4Array() //Default constructor
        { }

        public SMatrix4x4Array(Matrix4x4[] input)
        {
            count = input.Length;
            matrixData = new float[count * 16];
            int i = 0;
            int j = 0;

            while (i < count)
            {
                Matrix4x4 matrix = input[i];
                matrixData[j++] = matrix.m00; matrixData[j++] = matrix.m01; matrixData[j++] = matrix.m02; matrixData[j++] = matrix.m03;
                matrixData[j++] = matrix.m10; matrixData[j++] = matrix.m11; matrixData[j++] = matrix.m12; matrixData[j++] = matrix.m13;
                matrixData[j++] = matrix.m20; matrixData[j++] = matrix.m21; matrixData[j++] = matrix.m22; matrixData[j++] = matrix.m23;
                matrixData[j++] = matrix.m30; matrixData[j++] = matrix.m31; matrixData[j++] = matrix.m32; matrixData[j++] = matrix.m33;
                i++;
            }
        }
        public SMatrix4x4Array(float[] reconstruct)
        {
            this.count = reconstruct.Length / 16;
            this.matrixData = reconstruct;
        }

        public Matrix4x4 GetMatrix(int origionalIndex)
        {
            int idx = origionalIndex * 16;

            Matrix4x4 result = new Matrix4x4();
            result.m00 = matrixData[idx++]; result.m01 = matrixData[idx++]; result.m02 = matrixData[idx++]; result.m03 = matrixData[idx++];
            result.m10 = matrixData[idx++]; result.m11 = matrixData[idx++]; result.m12 = matrixData[idx++]; result.m13 = matrixData[idx++];
            result.m20 = matrixData[idx++]; result.m21 = matrixData[idx++]; result.m22 = matrixData[idx++]; result.m23 = matrixData[idx++];
            result.m30 = matrixData[idx++]; result.m31 = matrixData[idx++]; result.m32 = matrixData[idx++]; result.m33 = matrixData[idx++];
            return result;
        }

        internal void Dispose() {
            matrixData = null;
        }
    }




    [Serializable]
    public class SVector3Array
    {
        public int count;
        public float[] vectorData;

        public SVector3Array() //Default constructor.
        { }
        public SVector3Array(int count)
        {
            this.count = count;
            this.vectorData = new float[count * 3];
        }
        public SVector3Array(Vector3[] input)
        {
            count = input.Length;
            vectorData = new float[count * 3];
            int i = 0;
            int j = 0;
            while (i < count)
            {
                Vector3 vector = input[i];
                vectorData[j++] = vector.x;
                vectorData[j++] = vector.y;
                vectorData[j++] = vector.z;
                i++;
            }
        }
        public SVector3Array(float[] reconstruct)
        {
            this.count = reconstruct.Length / 3;
            this.vectorData = reconstruct;
        }

        public Vector3 GetVertex(int origionalIndex)
        {
            int idx = origionalIndex * 3;
            return new Vector3(vectorData[idx], vectorData[idx + 1], vectorData[idx + 2]);
        }

        internal void Dispose() {
            vectorData = null;
        }
    }

    [Serializable]
    public class SVector2Array
    {
        public int count;
        public float[] vectorData;

        public SVector2Array() //Default constructor.
        { }
        public SVector2Array(int count)
        {
            this.count = count;
            this.vectorData = new float[count * 2];
        }
        public SVector2Array(Vector2[] input)
        {
            count = input.Length;
            vectorData = new float[count * 2];
            int i = 0;
            int j = 0;
            while (i < count)
            {
                Vector2 vector = input[i];
                vectorData[j++] = vector.x;
                vectorData[j++] = vector.y;
                i++;
            }
        }
        public SVector2Array(float[] reconstruct)
        {
            this.count = reconstruct.Length / 2;
            this.vectorData = reconstruct;
        }

        public Vector2 GetVertex(int origionalIndex)
        {
            int idx = origionalIndex * 2;
            return new Vector2(vectorData[idx], vectorData[idx + 1]);
        }

        public Vector2[] GetVertexArray()
        {
            Vector2[] result = new Vector2[count];
            int i = count;
            while (--i > -1)
                result[i] = GetVertex(i);
            
            return result;
        }

        internal void Dispose() {
            vectorData = null;
        }
    }


    #endregion
    //--------------------------------------- ARRAYS --------------------------------------
    //--------------------------------------- ARRAYS --------------------------------------









}