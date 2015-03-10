using System;
using UnityEngine;


namespace Frankfort.VBug.Internal
{
    public static class TransformExtension
    {
        public static Vector3 GetScale(this Transform transform)
        {
            Vector3 result = transform.localScale;
            Transform parent = transform.parent;

            while (parent != null)
            {
                result = Vector3.Scale(result, parent.localScale);
                parent = parent.parent;
            }

            return result;
        }
    }
}