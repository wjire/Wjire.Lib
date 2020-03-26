using System;

namespace Wjire.Common
{
    /// <summary>
    /// DateTimeHelper
    /// </summary>
    public static class DateTimeHelper
    {

        /// <summary>
        /// 日期转换为时间戳（时间戳单位秒）
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static long ToTimeStamp(this DateTime time)
        {
            DateTime dt = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return (long)(time.AddHours(-8) - dt).TotalMilliseconds;
        }



        /// <summary>
        /// 时间戳转换为日期（时间戳单位秒）
        /// </summary>
        /// <param name="timeStamp"></param>
        /// <returns></returns>
        public static DateTime ConvertToDateTime(long timeStamp)
        {
            DateTime start = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return start.AddMilliseconds(timeStamp).AddHours(8);
        }


        /// <summary>
        /// 将秒数转换成天数,如:1天23小时4分钟7秒.
        /// </summary>
        /// <param name="seconds"></param>
        /// <returns></returns>
        public static string ConvertSecondsToString(int seconds)
        {
            if (seconds < 60)
            {
                return seconds + "秒";
            }
            if (seconds < 3600)
            {
                int min = seconds % 60;
                return seconds / 60 + "分" + (min == 0 ? null : ConvertSecondsToString(min));
            }
            if (seconds < 86400)
            {
                int hour = seconds % 3600;
                return seconds / 3600 + "小时" + (hour == 0 ? null : ConvertSecondsToString(hour));
            }
            int day = seconds % 86400;
            return seconds / 86400 + "天" + (day == 0 ? null : ConvertSecondsToString(day));
        }
    }
}