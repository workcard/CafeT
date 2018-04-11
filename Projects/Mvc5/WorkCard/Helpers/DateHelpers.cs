using System;

namespace Web.Helpers
{
    public class DateHelpers
    {
        public static bool IsEquals(DateTime? date1, DateTime? date2)
        {
            DateTime d1 = (DateTime)date1;
            DateTime d2 = (DateTime)date2;
            if(d1.ToShortDateString() == d2.ToShortDateString())
            return true;
            else
                return false;
        }

        public static int DaysBetweenForNow(DateTime? date)
        {
            DateTime d1 = (DateTime)date;
            DateTime now = DateTime.Now;
            TimeSpan span = now.Subtract(d1);
            return (int)span.TotalDays;
        }

        public static DateTime GetToDay()
        {
            return DateTime.Now;
        }

        public static DateTime GetLastWorkingDate(DateTime date)
        {
            DateTime _lastWorkingDay = date.AddDays(-1);
            if (_lastWorkingDay.DayOfWeek == DayOfWeek.Saturday)
                _lastWorkingDay = _lastWorkingDay.AddDays(-1);
            else
            {
                if (_lastWorkingDay.DayOfWeek == DayOfWeek.Sunday)
                    _lastWorkingDay = _lastWorkingDay.AddDays(-2);
            }
            return _lastWorkingDay;
        }

        public static DateTime GetNextWorkingDate(DateTime date)
        {
            DateTime _nextWorkingDate = date.AddDays(1);
            if (_nextWorkingDate.DayOfWeek == DayOfWeek.Saturday)
                _nextWorkingDate = _nextWorkingDate.AddDays(2);
            else
            {
                if (_nextWorkingDate.DayOfWeek == DayOfWeek.Sunday)
                    _nextWorkingDate = _nextWorkingDate.AddDays(1);
            }
            return _nextWorkingDate;
        }

        public static DateTime FirstDayOfWeek(DateTime date)
        {
            var _candidateDate = date;
            while (_candidateDate.DayOfWeek != DayOfWeek.Monday)
            {
                _candidateDate = _candidateDate.AddDays(-1);
            }
            return _candidateDate;
        }

        public static DateTime LastDayOfWeek(DateTime date)
        {
            var _candidateDate = date;
            while (_candidateDate.DayOfWeek != DayOfWeek.Monday)
            {
                _candidateDate = _candidateDate.AddDays(-1);
            }
            return _candidateDate.AddDays(4);
        }
    }
}
