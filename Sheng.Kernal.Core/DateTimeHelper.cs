using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Linq;


namespace Sheng.Kernal.Core
{
    public class DateTimeHelper
    {
        /// <summary>
        /// 获取指定日期，在为一年中为第几周
        /// </summary>
        /// <param name="dt">指定时间</param>
        /// <reutrn>返回第几周</reutrn>
        public static int GetWeekOfYear(DateTime dt)
        {
            GregorianCalendar gc = new GregorianCalendar();
            int weekOfYear = gc.GetWeekOfYear(dt, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
            return weekOfYear;
        }

        /// <summary>
        /// 确认指定的周次是否在周列表的范围内
        /// </summary>
        /// <param name="strWeek"></param>
        /// <param name="weekList"></param>
        /// <returns></returns>
        public static int GetWeekOfYear(string strWeek, List<Week> weekList)
        {
            int currentWeeekOfYear = DateTimeHelper.GetWeekOfYear(DateTime.Now);

            int weekOfYear = -1;
            if (String.IsNullOrEmpty(strWeek) == false)
                int.TryParse(strWeek, out weekOfYear);

            if (weekOfYear < weekList[0].WeekOfYear ||
                weekOfYear > weekList[weekList.Count - 1].WeekOfYear)
            {
                var currentWeekList =
                    (from c in weekList where c.WeekOfYear == currentWeeekOfYear select c).ToList();

                if (currentWeekList.Count > 0)
                {
                    weekOfYear = currentWeeekOfYear;
                }
                else
                {
                    weekOfYear = weekList[0].WeekOfYear;
                }
            }

            return weekOfYear;
        }

        /// <summary>
        /// 获取指定日期为当月的第几周
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="weekStart"></param>
        /// <returns></returns>
        public static int GetWeekOfMonth(DateTime dt)
        {
            int weekStart = 1;

            //WeekStart
            //1表示 周一至周日 为一周
            //2表示 周日至周六 为一周
            DateTime FirstofMonth;
            FirstofMonth = Convert.ToDateTime(dt.Date.Year + "-" + dt.Date.Month + "-" + 1);

            int i = (int)FirstofMonth.Date.DayOfWeek;
            if (i == 0)
            {
                i = 7;
            }

            if (weekStart == 1)
            {
                return (dt.Date.Day + i - 2) / 7 + 1;
            }
            if (weekStart == 2)
            {
                return (dt.Date.Day + i - 1) / 7;

            }
            return 0;
            //错误返回值0
        }

        /// <summary>
        /// 计算本周起始日期（礼拜一的日期）
        /// </summary>
        /// <param name="someDate">该周中任意一天</param>
        /// <returns>返回礼拜一日期，后面的具体时、分、秒和传入值相等</returns>
        public static DateTime CalculateFirstDateOfWeek(DateTime someDate)
        {
            int i = someDate.DayOfWeek - DayOfWeek.Monday;
            if (i == -1) i = 6;// i值 > = 0 ，因为枚举原因，Sunday排在最前，此时Sunday-Monday=-1，必须+7=6。
            TimeSpan ts = new TimeSpan(i, 0, 0, 0);
            return someDate.Subtract(ts);
        }

        /// <summary>
        /// 计算本周结束日期（礼拜日的日期）
        /// </summary>
        /// <param name="someDate">该周中任意一天</param>
        /// <returns>返回礼拜日日期，后面的具体时、分、秒和传入值相等</returns>
        public static DateTime CalculateLastDateOfWeek(DateTime someDate)
        {
            int i = someDate.DayOfWeek - DayOfWeek.Sunday;
            if (i != 0) i = 7 - i;// 因为枚举原因，Sunday排在最前，相减间隔要被7减。
            TimeSpan ts = new TimeSpan(i, 0, 0, 0);
            return someDate.Add(ts);
        }

        /// 得到一年中的某周的起始日和截止日
        /// 年 nYear
        /// 周数 nNumWeek
        /// 周始 out dtWeekStart
        /// 周终 out dtWeekeEnd
        /// </summary>
        /// <param name="nYear"></param>
        /// <param name="nNumWeek"></param>
        /// <param name="dtWeekStart"></param>
        /// <param name="dtWeekeEnd"></param>
        public static void GetWeek(int nYear, int nNumWeek, out DateTime dtWeekStart, out DateTime dtWeekeEnd)
        {
            DateTime dt = new DateTime(nYear, 1, 1);
            dt = dt + new TimeSpan((nNumWeek - 1) * 7, 0, 0, 0);
            //dtWeekStart = dt.AddDays(-(int)dt.DayOfWeek + (int)DayOfWeek.Monday);
            //dtWeekeEnd = dt.AddDays((int)DayOfWeek.Saturday - (int)dt.DayOfWeek + 1);

            //默认的sunday=0,monday=1
            //但是此处一周的起启日期要用星期一
            //否则BUG：
            //2017/1/1 是星期天，上面的算法取2017年第1周的时间，取的是1/2到1/8
            //但是 GetWeekOfYear(DateTime dt)中，1月2日它返回的周次是第2周
            //这就造成周次对不上
            int dayOfWeek = (int)dt.DayOfWeek;
            if (dayOfWeek == 0)
                dayOfWeek = 7;
            dtWeekStart = dt.AddDays(-dayOfWeek + (int)DayOfWeek.Monday);
            dtWeekeEnd = dt.AddDays((int)DayOfWeek.Saturday - dayOfWeek + 1);

        }

        /// <summary>
        /// 判断选择的日期是否是本周（根据系统当前时间决定的‘本周’比较而言）
        /// </summary>
        /// <param name="someDate"></param>
        /// <returns></returns>
        public static bool IsThisWeek(DateTime someDate)
        {
            //得到someDate对应的周一
            DateTime someMon = CalculateFirstDateOfWeek(someDate);
            //得到本周一
            DateTime nowMon = CalculateFirstDateOfWeek(DateTime.Now);

            TimeSpan ts = someMon - nowMon;
            if (ts.Days < 0)
                ts = -ts;//取正
            if (ts.Days >= 7)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// 求某年有多少周
        /// 返回 int
        /// </summary>
        /// <param name="strYear"></param>
        /// <returns>int</returns>
        public static int GetYearWeekCount(int strYear)
        {
            System.DateTime fDt = DateTime.Parse(strYear.ToString() + "-01-01");
            int k = Convert.ToInt32(fDt.DayOfWeek);//得到该年的第一天是周几 
            if (k == 1)
            {
                int countDay = fDt.AddYears(1).AddDays(-1).DayOfYear;
                int countWeek = countDay / 7 + 1;
                return countWeek;

            }
            else
            {
                int countDay = fDt.AddYears(1).AddDays(-1).DayOfYear;
                int countWeek = countDay / 7 + 2;
                return countWeek;
            }

        }

        ///// 求某月有多少周
        ///// 返回 int
        ///// </summary>
        ///// <param name="strYear"></param>
        ///// <returns>int</returns>
        //public static int GetMonthWeekCount(int year, int month)
        //{
        //    DateTime startDate = new DateTime(year, month, 1);

        //    GregorianCalendar gc = new GregorianCalendar();
        //    int daysCount = gc.GetDaysInMonth(year, month);
        //    DateTime endDate = new DateTime(year, month, daysCount);

        //    if (startDate.DayOfWeek != DayOfWeek.Monday || endDate.DayOfWeek != DayOfWeek.Sunday)
        //    {
        //        return 5;
        //    }
        //    else
        //    {
        //        return 4;
        //    }
        //}

        /// <summary>
        /// 获取指定月份的周列表
        /// </summary>
        /// <param name="month"></param>
        /// <returns></returns>
        public static List<Week> GetWeekListOfMonth(int year, int month)
        {
            DateTime dtNow = DateTime.Now;

            if (year == 0)
                year = dtNow.Year;

            if (month == 0)
                month = dtNow.Month;

            List<Week> list = new List<Week>();

            GregorianCalendar gc = new GregorianCalendar();
            int daysCount = gc.GetDaysInMonth(year, month);


            DateTime dt = new DateTime(year, month, 1);

            int weekOfYear = GetWeekOfYear(dt);

            int currentWeekOfYear = GetWeekOfYear(DateTime.Now);

            while (true)
            {
                DateTime dtWeekStart;
                DateTime dtWeekEnd;

                GetWeek(year, weekOfYear, out dtWeekStart, out dtWeekEnd);

                Week week = new Week();
                week.WeekOfYear = weekOfYear;
                if (dtWeekStart.Month == month)
                    week.WeekOfMonth = GetWeekOfMonth(dtWeekStart);
                else
                    week.WeekOfMonth = GetWeekOfMonth(dtWeekEnd);
                week.Monday = dtWeekStart;
                week.Sunday = dtWeekEnd;

                week.CurrentWeek = weekOfYear == currentWeekOfYear;

                list.Add(week);

                weekOfYear++;

                if (dtWeekEnd.Month != month || dtWeekEnd.Day == daysCount)
                    break;
            }

            return list;
        }

        public static List<Week> GetRecentlyWeekList(int weekOfYear)
        {
            List<Week> list = new List<Week>();

            DateTime dtNow = DateTime.Now;

            int oldweekOfYear = weekOfYear;

            if (weekOfYear < 0)
            {
                weekOfYear = GetWeekOfYear(dtNow);
            }

            //int weekOfYear = GetWeekOfYear(dtNow);
            weekOfYear = weekOfYear - 1;

            for (int i = 0; i <= 2; i++)
            {
                DateTime dtWeekStart;
                DateTime dtWeekEnd;

                GetWeek(dtNow.Year, weekOfYear, out dtWeekStart, out dtWeekEnd);

                Week week = new Week();
                week.WeekOfYear = weekOfYear;
                if (dtWeekStart.Month == dtNow.Month)
                    week.WeekOfMonth = GetWeekOfMonth(dtWeekStart);
                else
                    week.WeekOfMonth = GetWeekOfMonth(dtWeekEnd);
                week.Monday = dtWeekStart;
                week.Sunday = dtWeekEnd;

                if (oldweekOfYear == weekOfYear)
                {
                    week.CurrentWeek = true;
                }

                list.Add(week);

                weekOfYear++;
            }

            return list;
        }


        public class Week
        {
            public int WeekOfYear
            {
                get;
                set;
            }

            public int WeekOfMonth
            {
                get;
                set;
            }

            public DateTime Monday
            {
                get;
                set;
            }

            public DateTime Sunday
            {
                get;
                set;
            }

            private bool _currentWeek = false;
            public bool CurrentWeek
            {
                get { return _currentWeek; }
                set { _currentWeek = value; }
            }
        }
    }
}
