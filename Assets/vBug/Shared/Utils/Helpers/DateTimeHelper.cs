using System;

namespace Frankfort.VBug.Internal
{
    public static class DateTimeHelper
    {
        private static DateTime identityTimeStamp = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);


        internal static double ToSeconds()
        {
            return DateTime.UtcNow.Subtract(identityTimeStamp).TotalSeconds;
        }


        internal static DateTime FromSeconds(double seconds)
        {
            return identityTimeStamp.Add(TimeSpan.FromSeconds(seconds));
        }

    }
}