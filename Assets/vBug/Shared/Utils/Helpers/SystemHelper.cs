using System;

namespace Frankfort.VBug.Internal
{
    public class SystemHelper
    {
        public static Enum[] GetIndividualEnums(Enum input)
        {
            Enum[] values = (Enum[])Enum.GetValues(input.GetType());
            Enum[] result = new Enum[values.Length];
            int iInput = Convert.ToInt32(input);
                
            int c = 0;
            int i = values.Length;
            while (--i > -1)
            {
                int iValue = Convert.ToInt32(values[i]);
                if ((iInput & iValue) == iValue)
                    result[c++] = values[i];
            }
            if (result.Length > c)
                Array.Resize(ref result, c);

            return result;
        }
    }
}