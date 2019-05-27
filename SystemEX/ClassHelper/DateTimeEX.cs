using System;
using System.ComponentModel;
namespace NK
{

    /// <summary>
    /// 日期帮助类
    /// </summary>
    public  static partial class DateTimeEX
    {
        #region 内部处理
        private static long Fix(double Number)
        {
            if (Number >= 0)
            {
                return (long)Math.Floor(Number);
            }
            return (long)Math.Ceiling(Number);
        }

        #endregion

        /// <summary>
        /// 日期类型
        /// </summary>
        [Description("校验类型")]
        public enum DateInterval : byte
        {
            /// <summary>
            /// 年
            /// </summary>
            [Description("年")]
            Year,
            /// <summary>
            /// 月
            /// </summary>
            [Description("月")]
            Month,
            /// <summary>
            /// 星期
            /// </summary>
            [Description("星期")]
            Weekday,
            /// <summary>
            /// 日
            /// </summary>
            [Description("日")]
            Day,
            /// <summary>
            /// 时
            /// </summary>
            [Description("时")]
            Hour,
            /// <summary>
            /// 分
            /// </summary>
            [Description("分")]
            Minute,
            /// <summary>
            /// 秒
            /// </summary>
            [Description("秒")]
            Second
        }
        /// <summary>
        /// 时间比较
        /// </summary>
        /// <param name="date1"></param>
        /// <param name="interval"></param>
        /// <param name="date2"></param>
        /// <returns></returns>
        public static long DateDiff(this DateTime date1, DateInterval interval, DateTime date2)
        {
            TimeSpan ts = date2 - date1;
            switch (interval)
            {
                case DateInterval.Year:
                    return date2.Year - date1.Year;
                case DateInterval.Month:
                    return (date2.Month - date1.Month) + (12 * (date2.Year - date1.Year));
                case DateInterval.Weekday:
                    return Fix(ts.TotalDays) / 7;
                case DateInterval.Day:
                    return Fix(ts.TotalDays);
                case DateInterval.Hour:
                    return Fix(ts.TotalHours);
                case DateInterval.Minute:
                    return Fix(ts.TotalMinutes);
                default:
                    return Fix(ts.TotalSeconds);
            }
        }

        /// <summary>
        /// 添加日期
        /// </summary>
        /// <param name="date"></param>
        /// <param name="interval"></param>
        /// <param name="Time"></param>
        /// <returns></returns>
        public static DateTime DateAdd(this DateTime date, DateInterval interval,int Time)
        {
            switch (interval)
            {
                case DateInterval.Year:
                    return date.AddYears(Time);
                case DateInterval.Month:
                    return date.AddMonths(Time);
                case DateInterval.Weekday:
                    return date.AddDays(Time*7);
                case DateInterval.Day:
                    return date.AddDays(Time);
                case DateInterval.Hour:
                    return date.AddHours(Time);
                case DateInterval.Minute:
                    return date.AddMinutes(Time);
                default:
                    return date.AddSeconds(Time);
            }
        }

    }
}
