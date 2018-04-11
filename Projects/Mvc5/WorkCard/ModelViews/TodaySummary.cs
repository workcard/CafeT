using CafeT.Time;
using System;
using System.Collections.Generic;
using System.Linq;
using Web.Models;

namespace Web.ModelViews
{
    public class DaySummary
    {
        protected const int TIME_TO_DO = 480; //minutes
        public enum Status
        {
            Effected,
            NotEnough
        }
        public double TotalTimes { set; get; }
        public DateTime Date { set; get; }
        public IEnumerable<WorkIssue> Issues { set; get; }
        public IEnumerable<WorkIssue> CompletedIssues { set; get; }

        public DaySummary(IEnumerable<WorkIssue> issues)
        {
            Issues = issues;
            CompletedIssues = Issues.Where(t => t.IsCompleted());
            TotalTimes = Issues
                .Sum(t => t.IssueEstimation);
        }

        public Status GetWorkingStatus()
        {
            if (TotalTimes >= TIME_TO_DO) return Status.Effected;
            else
            {
                return Status.NotEnough;
            }
        }
    }

    public class MonthSummary
    {
        public int Month { set; get; }
        public int DaysOfMonth { set; get; }
        public IEnumerable<WorkIssue> Issues { set; get; }
        public IEnumerable<WorkIssue> CompletedIssues { set; get; }
        public List<DaySummary> days = new List<DaySummary>();

        public MonthSummary(IEnumerable<WorkIssue> issues)
        {
            Issues = issues;
            CompletedIssues = Issues.Where(t => t.IsCompleted());
            DaysOfMonth = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);


            for (int day = 1; day <= DaysOfMonth; day++)
            {
                var _issues = GetIssuesByDay(day);
                if (_issues != null && _issues.Count() > 0)
                {
                    DaySummary daySummary = new DaySummary(_issues);
                    days.Add(daySummary);
                }
            }
        }
        public IEnumerable<WorkIssue> GetIssuesByDay(int day)
        {
            var date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, day);
            return Issues.Where(t=>t.End.HasValue && t.End.Value.IsDay(date))
                .AsEnumerable();
        }
    }
}