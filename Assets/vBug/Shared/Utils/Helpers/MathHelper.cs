using System;

namespace Frankfort.VBug.Internal
{
    public static class MathHelper
    {

        public static bool IsPowerOfTwo(int x) {
            return x > 0 && (x & (x - 1)) == 0;
        }


        public static int MakePowerOfTwo(int x, bool roundToCeiling = false) {
            if (IsPowerOfTwo(x))
                return x;

            if (x <= 1)
                return x;

            int y = 1;
            int yOld = 1;
            while (x > y)
            {
                yOld = y;
                y *= 2;
            }

            return roundToCeiling ? y : yOld;
        }



        public static long CombineInts(int a, int b) {
            long aa = (long)a << 32;
            long bb = (long)b;
            return aa | bb;
        }

    }
}
