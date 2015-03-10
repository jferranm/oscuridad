using System;
using UnityEngine;

namespace Frankfort.VBug.Internal
{
    public static class Matrix4x4Extention
    {
        public static Vector3 GetPosition(this Matrix4x4 matrix)
        {
            return matrix.GetColumn(3);
        }
        public static Vector3 GetScale(this Matrix4x4 matrix)
        {
            return new Vector3(
                matrix.GetColumn(0).magnitude,
                matrix.GetColumn(1).magnitude,
                matrix.GetColumn(2).magnitude);
        }
        public static Quaternion GetRotation(this Matrix4x4 matrix)
        {
            Vector3 c1 = matrix.GetColumn(1);
            Vector3 c2 = matrix.GetColumn(2);
            if (c1 == Vector3.zero || c2 == Vector3.zero)
                return Quaternion.identity;

            return Quaternion.LookRotation( matrix.GetColumn(2), matrix.GetColumn(1));
        }
    }
}
