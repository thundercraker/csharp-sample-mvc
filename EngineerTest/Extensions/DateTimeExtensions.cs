using System;

namespace EngineerTest.Extensions
{
    public static class DateTimeExtensions
    {
        public static int ToUnixTimeStamp(
            this DateTime dateTime)
        {
            return (int)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
        }
    }
}