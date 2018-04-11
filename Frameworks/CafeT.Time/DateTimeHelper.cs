using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeT.Time
{
    public static class CompareTimer
    {
        /// <summary>
        /// Compare (compareTime) with time.
        /// <0 -- time > compareTime
        /// </summary>
        /// <param name="time"></param>
        /// <param name="compareTime"></param>
        /// <returns></returns>
        public static bool IsLaterOrEquals(this DateTime time, DateTime compareTime)
        {
            int result = DateTime.Compare(time, compareTime);
            if (result < 0)
                return false;
            return true;
        }
    }
    public static class DateTimeHelper
    {
        #region Move
        public static DateTime Tommorow(this DateTime date)
        {
            return DateTime.Today.AddDays(1);
        }
        public static DateTime Yesterday(this DateTime date)
        {
            return DateTime.Today.AddDays(-1);
        }
        #endregion
        # region ParseByString
        public static DateTime? Parse(string input)
        {
            if (input.ToLower() == "today") return DateTime.Today;
            if (input.ToLower() == "yesterday") return DateTime.Today;
            return null;
        }
        public static DateTime? GetDateTime(this DateTime date,string input)
        {
            if(input.ToLower() == "today")
            {
                return DateTime.Today;
            }
            if (input.ToLower() == "yesterday")
            {
                return date.Yesterday();
            }
            if (input.ToLower() == "tommorow")
            {
                return date.Tommorow();
            }
            //if (input.ToLower() == "nextweek")
            //{
            //    return date.N();
            //}
            return null;
        }
        public static int CountTimes(this DateTime date, string input)
        {
            if (input.ToLower() == "OneMinutes".ToLower())
            {
                return 1;
            }
            if (input.ToLower() == "FiveMinutes".ToLower())
            {
                return 5;
            }
            if (input.ToLower() == "TenMinutes".ToLower())
            {
                return 10;
            }
            if (input.ToLower() == "FifthteenMinutes".ToLower())
            {
                return 15;
            }
            if (input.ToLower() == "ThirtyMinutes".ToLower())
            {
                return 30;
            }
            if (input.ToLower() == "SixtyMinutes".ToLower())
            {
                return 60;
            }
            if (input.ToLower() == "OneDay".ToLower())
            {
                return 60*24;
            }
            if (input.ToLower() == "HaftDay".ToLower())
            {
                return 60 * 24;
            }
            //if (input.ToLower() == "nextweek")
            //{
            //    return date.N();
            //}
            return -1;
        }
        #endregion

        public static DateTime? LaterNearest(this List<DateTime> list)
        {
            list.SortAscending();
            foreach(var item in list)
            {
                if (DateTime.Compare(item, DateTime.Now) > 0) return item;
            }
            return null;
        }

        public static List<DateTime> SortAscending(this List<DateTime> list)
        {
            list.Sort((a, b) => a.CompareTo(b));
            return list;
        }

        public static List<DateTime> SortDescending(this List<DateTime> list)
        {
            list.Sort((a, b) => b.CompareTo(a));
            return list;
        }

        public static List<DateTime> SortMonthAscending(this List<DateTime> list)
        {
            list.Sort((a, b) => a.Month.CompareTo(b.Month));
            return list;
        }

        public static List<DateTime> SortMonthDescending(this List<DateTime> list)
        {
            list.Sort((a, b) => b.Month.CompareTo(a.Month));
            return list;
        }
    }
}
