using CafeT.Objects.Enums;
using CafeT.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using Web.ModelViews;

namespace Web.Helpers
{
    public static class IssuesHelper
    {
        #region IssueMining
        public static IEnumerable<IssueView> HasEmail(this IEnumerable<IssueView> issues)
        {
            return issues.Where(t => t.Content.HasEmail() || t.Description.HasEmail()).ToList();
        }
        #endregion
        #region HelperForFilter
        public static IEnumerable<IssueView> GetNoTime(this IEnumerable<IssueView> issues)
        {
            return issues.Where(t => t.IsNoTime()).ToList();
        }
        public static IEnumerable<IssueView> GetDone(this IEnumerable<IssueView> issues)
        {
            return issues.Where(t => t.Status == IssueStatus.Done).ToList();
        }
        public static IEnumerable<IssueView> GetExpired(this IEnumerable<IssueView> issues)
        {
            return issues.Where(t => t.IsExpired()).ToList();
        }
        public static IEnumerable<IssueView> GetNotDone(this IEnumerable<IssueView> issues)
        {
            return issues.Where(t => t.Status != IssueStatus.Done).ToList();
        }
        public static IEnumerable<IssueView> GetFromNow(this IEnumerable<IssueView> issues)
        {
            return issues.Where(t => t.End.HasValue && t.End.Value >= DateTime.Now).ToList();
        }
        public static IEnumerable<IssueView> GetToday(this IEnumerable<IssueView> issues)
        {
            return issues.Where(t => t.IsToday());
        }
        public static IEnumerable<IssueView> GetInDay(this IEnumerable<IssueView> issues, DateTime date)
        {
            return issues.Where(t => t.IsInDay(date));
        }
        //public static IEnumerable<IssueView> GetInDays(this IEnumerable<IssueView> issues, DateTime start, DateTime end)
        //{
        //    List<IssueView> _results = new List<IssueView>();
        //    //for(var year = start.Year; year <= end.Year; year++)
        //    //{
        //    //    for(var month = start.Month; month <= end.Month; month ++)
        //    //    {
        //    //        for(var day = start.da)
        //    //    }
        //    //}

        //}
        public static IEnumerable<IssueView> GetTomorrow(this IEnumerable<IssueView> issues)
        {
            return issues.Where(t => t.IsTomorrow()).ToList();
        }
        public static IEnumerable<IssueView> GetNext(this IEnumerable<IssueView> issues, int nextDay)
        {
            return issues.Where(t => t.IsNextDay(nextDay)).ToList();
        }
        public static IEnumerable<IssueView> GetPrev(this IEnumerable<IssueView> issues, int nextDay)
        {
            return issues.Where(t => t.IsPrevDay(nextDay)).ToList();
        }
        #endregion
        public static decimal TotalMinutesTimeTodo(this IEnumerable<IssueView> issues)
        {
            return issues.Sum(t => t.IssueEstimation);
        }
        public static decimal TotalHoursTimeTodo(this IEnumerable<IssueView> issues)
        {
            return issues.Sum(t => t.IssueEstimation)/60;
        }

        public static decimal TotalDaysTimeTodo(this IEnumerable<IssueView> issues)
        {
            return issues.Sum(t => t.IssueEstimation) / (60*8);
        }
    }
}