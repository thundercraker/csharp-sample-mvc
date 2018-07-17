using System;

namespace EngineerTest.Extensions
{
    public static class DateTimeExtensions
    {
        public static int ToUnixTimeStamp(
            this DateTime dateTime)
        {
            return (int)(dateTime.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
        }
        
        public static DateTime FromUnixTimeStamp(
            this long timeStamp)
        {
            return (new DateTime(1970, 1, 1)).AddSeconds(timeStamp);
        }
    }
}