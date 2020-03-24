using System;

namespace ZDY.DMS.Tools
{
    /// <summary>
    /// 时间类
    /// </summary>
    public static class TimeHelper
    {
        /// <summary>
        /// 把秒转换成分钟
        /// </summary>
        /// <returns></returns>
        public static int SecondToMinute(int second)
        {
            decimal mm = second / 60;
            return Convert.ToInt32(Math.Ceiling(mm));
        }

        /// <summary>
        /// 得到随机日期
        /// </summary>
        /// <param name="time1">起始日期</param>
        /// <param name="time2">结束日期</param>
        /// <returns>间隔日期之间的 随机日期</returns>
        public static DateTime GetRandomTime(DateTime time1, DateTime time2)
        {
            Random random = new Random();
            DateTime minTime = new DateTime();
            DateTime maxTime = new DateTime();

            System.TimeSpan ts = new System.TimeSpan(time1.Ticks - time2.Ticks);

            // 获取两个时间相隔的秒数
            double dTotalSecontds = ts.TotalSeconds;
            int iTotalSecontds = 0;

            if (dTotalSecontds > System.Int32.MaxValue)
            {
                iTotalSecontds = System.Int32.MaxValue;
            }
            else if (dTotalSecontds < System.Int32.MinValue)
            {
                iTotalSecontds = System.Int32.MinValue;
            }
            else
            {
                iTotalSecontds = (int)dTotalSecontds;
            }


            if (iTotalSecontds > 0)
            {
                minTime = time2;
                maxTime = time1;
            }
            else if (iTotalSecontds < 0)
            {
                minTime = time1;
                maxTime = time2;
            }
            else
            {
                return time1;
            }

            int maxValue = iTotalSecontds;

            if (iTotalSecontds <= System.Int32.MinValue)
                maxValue = System.Int32.MinValue + 1;

            int i = random.Next(System.Math.Abs(maxValue));

            return minTime.AddSeconds(i);
        }

        /// <summary>
        /// 将时间格式化成 yyyy{0}MM{1}dd
        /// </summary>
        /// <param name="dateTime">时间</param>
        /// <param name="Separator">分隔符</param>
        /// <returns></returns>
        public static string FormatDate(this DateTime dateTime, char Separator)
        {
            if (dateTime != null && !dateTime.Equals(DBNull.Value))
            {
                string tem = string.Format("yyyy{0}MM{1}dd", Separator, Separator);
                return dateTime.ToString(tem);
            }
            else
            {
                return FormatTime(DateTime.Now, Separator);
            }
        }

        /// <summary>
        /// 将时间格式化成 hh{0}mm{1}ss
        /// </summary>
        /// <param name="dateTime"></param>
        /// <param name="Separator"></param>
        /// <returns></returns>
        public static string FormatTime(this DateTime dateTime, char Separator)
        {
            if (dateTime != null && !dateTime.Equals(DBNull.Value))
            {
                string tem = string.Format("hh{0}mm{1}ss", Separator, Separator);
                return dateTime.ToString(tem);
            }
            else
            {
                return FormatTime(DateTime.Now, Separator);
            }
        }

        /// <summary>
        /// 格式化日期时间
        /// </summary>
        /// <param name="dateTime">日期时间</param>
        /// <param name="mode">显示模式</param>
        /// <returns>0-9种模式的日期</returns>
        public static string FormatDate(this DateTime dateTime, int mode)
        {
            switch (mode)
            {
                case 0:
                    return dateTime.ToString("yyyy-MM-dd");
                case 1:
                    return dateTime.ToString("yyyy-MM-dd HH:mm:ss");
                case 2:
                    return dateTime.ToString("yyyy/MM/dd");
                case 3:
                    return dateTime.ToString("yyyy年MM月dd日");
                case 4:
                    return dateTime.ToString("MM-dd");
                case 5:
                    return dateTime.ToString("MM/dd");
                case 6:
                    return dateTime.ToString("MM月dd日");
                case 7:
                    return dateTime.ToString("yyyy-MM");
                case 8:
                    return dateTime.ToString("yyyy/MM");
                case 9:
                    return dateTime.ToString("yyyy年MM月");
                default:
                    return dateTime.ToString();
            }
        }

        /// <summary>
        /// 取得某月的第一天
        /// </summary>
        /// <param name="dateTime">要取得月份第一天的时间</param>
        /// <returns></returns>
        public static DateTime GetFirstDayInMonth(this DateTime dateTime)
        {
            return dateTime.AddDays(1 - dateTime.Day);
        }

        /// <summary>
        /// 取得某月的最后一天
        /// </summary>
        /// <param name="dateTime">要取得月份最后一天的时间</param>
        /// <returns></returns>
        public static DateTime GetLastDayInMonth(this DateTime dateTime)
        {
            return dateTime.AddDays(1 - dateTime.Day).AddMonths(1).AddDays(-1);
        }

        /// <summary>
        /// 取得上个月第一天
        /// </summary>
        /// <param name="dateTime">要取得上个月第一天的当前时间</param>
        /// <returns></returns>
        public static DateTime GetFirstDayInPreviousMonth(this DateTime dateTime)
        {
            return dateTime.AddDays(1 - dateTime.Day).AddMonths(-1);
        }

        /// <summary>
        /// 取得上个月的最后一天
        /// </summary>
        /// <param name="dateTime">要取得上个月最后一天的当前时间</param>
        /// <returns></returns>
        public static DateTime GetLastDayInPrdviousMonth(this DateTime dateTime)
        {
            return dateTime.AddDays(1 - dateTime.Day).AddDays(-1);
        }

        /// <summary>
        /// 获取当前月第一天和最后一天
        /// </summary>
        /// <param name="firstDate"></param>
        /// <param name="lastDate"></param>
        public static Tuple<DateTime,DateTime> GetBothEndsInMonth(this DateTime dateTime)
        {
            //本月第一天时间    
            DateTime firstDay = dateTime.AddDays(-(dateTime.Day) + 1);
            //将本月月数+1  
            DateTime dt2 = dateTime.AddMonths(1);
            //本月最后一天时间  
            DateTime lastDay = dt2.AddDays(-(dateTime.Day));

            return new Tuple<DateTime, DateTime>(firstDay, lastDay);
        }

        /// <summary>
        /// 获取当前周第一天和最后一天
        /// </summary>
        /// <param name="firstDate"></param>
        /// <param name="lastDate"></param>
        public static Tuple<DateTime, DateTime> GetBothEndsInWeek(this DateTime dateTime)
        {
            int weeknow = Convert.ToInt32(dateTime.DayOfWeek);
            int daydiff = (-1) * weeknow + 1;
            int dayadd = 7 - weeknow;

            //本周第一天
            DateTime firstDay = dateTime.AddDays(daydiff);

            //本周最后一天
            DateTime lastDay = dateTime.AddDays(dayadd);

            return new Tuple<DateTime, DateTime>(firstDay, lastDay);
        }

        /// <summary>
        /// 返回某年某月最后一天
        /// </summary>
        /// <param name="year">年份</param>
        /// <param name="month">月份</param>
        /// <returns>日</returns>
        public static DateTime GetLastDayByYearAndMonth(int year, int month)
        {
            DateTime lastDay = new DateTime(year, month, new System.Globalization.GregorianCalendar().GetDaysInMonth(year, month));
            return lastDay;
        }

        /// <summary>
        /// 获得两个日期的间隔
        /// </summary>
        /// <param name="dateTime1">日期一</param>
        /// <param name="dateTime2">日期二</param>
        /// <returns>日期间隔TimeSpan。</returns>
        public static TimeSpan GetDateDiff(DateTime dateTime1, DateTime dateTime2)
        {
            TimeSpan ts1 = new TimeSpan(dateTime1.Ticks);
            TimeSpan ts2 = new TimeSpan(dateTime2.Ticks);
            return ts1.Subtract(ts2).Duration();
        }

        /// <summary>
        /// 获取时间差
        /// </summary>
        /// <param name="dateTime1"></param>
        /// <param name="dateTime2"></param>
        /// <returns></returns>
        public static string GetDateDiffForChinese(DateTime dateTime1, DateTime dateTime2)
        {
            string dateDiff = null;
            try
            {
                //TimeSpan ts1 = new TimeSpan(DateTime1.Ticks);
                //TimeSpan ts2 = new TimeSpan(DateTime2.Ticks);
                //TimeSpan ts = ts1.Subtract(ts2).Duration();
                TimeSpan ts = dateTime2 - dateTime1;
                if (ts.Days >= 1)
                {
                    dateDiff = dateTime1.Month.ToString() + "月" + dateTime1.Day.ToString() + "日";
                }
                else
                {
                    if (ts.Hours > 1)
                    {
                        dateDiff = ts.Hours.ToString() + "小时前";
                    }
                    else
                    {
                        dateDiff = ts.Minutes.ToString() + "分钟前";
                    }
                }
            }
            catch
            { }
            return dateDiff;
        }

        /// <summary>
        /// 获得两个时间的间隔
        /// </summary>
        /// <param name="dateTime1">日期一</param>
        /// <param name="dateTime2">日期二</param>
        /// <returns>返回间隔实际的年差月差日差</returns>
        public static Tuple<int, int, int> GetDateDiffForYearMonthDay(DateTime dateTime1, DateTime dateTime2)
        {
            dateTime1 = dateTime1.AddDays(-1);
            int year = dateTime2.Year - dateTime1.Year;
            dateTime1 = dateTime1.AddYears(year);
            int month = dateTime2.Month - dateTime1.Month;
            dateTime1 = dateTime1.AddMonths(month);
            int day = (new TimeSpan(dateTime2.Ticks - dateTime1.Ticks)).Days;
            return new Tuple<int, int, int>(year, month, day);
        }
    }
}
