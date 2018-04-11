using CafeT.Frameworks.Identity.Models;
using SmartTracking.Models;
using SmartTracking.Models.Objects;
using SmartTracking.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SmartTracking.Helpers
{
    public static class GraphHelper
    {
        public static decimal GetValueOfColumn(decimal estimation)
        {
            return (estimation / 8) * 100;
        }
    }

    public class PerformanceHelpers:IPerformance
    {
        private static ApplicationDbContext db = new ApplicationDbContext();

        //public static decimal GetPerformanceByEstimation(decimal estimation)
        //{
        //    return (estimation / 8) * 100;
        //}

        public static decimal GetPerformanceByIssues(List<Issue> issues)
        {
            decimal _performance = 0;
            foreach(var issue in issues)
            {
                _performance += GraphHelper.GetValueOfColumn(issue.IssueEstimation);
            }
            return _performance;
        }

        public static decimal GetPerformanceByIssuesInMonth(List<Issue> issues)
        {
            decimal _performance = 0;
            foreach (var issue in issues)
            {
                _performance += GraphHelper.GetValueOfColumn(issue.IssueEstimation);
            }
            return _performance;
        }

        public static string GetAverageNote(decimal performance)
        {
            string note = "";
            if (performance < 60)
                note = "Not good";
            else if (performance >= 60 && performance <= 65)
                note = "Good";
            else
                note = "Excellent";
            return note;
        }

        public static GraphItem GetPerformanceAverageByIssues(List<Issue> issues, int member)
        {
            GraphItem _item = new GraphItem();
            _item.y = GetPerformanceByIssues(issues) / (member);
            _item.Note = GetAverageNote(_item.y);

            return _item;
        }

        public static GraphItem GetPerformanceAverageByIssuesInWeek(List<Issue> issues, int member)
        {
            GraphItem _item = new GraphItem();
            _item.y = GetPerformanceByIssues(issues) / (member * 5);
            _item.Note = GetAverageNote(_item.y);

            return _item;
        }

        public static GraphItem GetPerformanceAverageByIssuesInDate(List<Issue> issues, DateTime date, int member)
        {
            issues = issues.Where(m => DateHelpers.IsEquals(m.IssueDueDate, date) == true).ToList();
            GraphItem _performanceAverage = new GraphItem();
            _performanceAverage.y = GetPerformanceByIssues(issues) / member;
            _performanceAverage.Note = GetAverageNote(_performanceAverage.y);

            return _performanceAverage;
        }



        public static List<GraphItem> GetPerformanceUserByIssuesDate(List<Issue> issues)
        {
            List<ApplicationUser> _users = db.Users.Where(m => m.BugNetUserId != null).ToList();
            List<GraphItem> _performanceUsers = new List<GraphItem>();
            foreach(var user in _users)
            {
                GraphItem _performanceUser = new GraphItem();
                _performanceUser.x = user.UserName;
                _performanceUser.y = GetPerformanceByIssues(issues.Where(m => m.AssignedUserName == user.UserName).ToList());
                _performanceUser.Note = GetAverageNote(_performanceUser.y);

                _performanceUsers.Add(_performanceUser);
            }

            return _performanceUsers;
        }

        public static GraphItem GetPerformanceUserByIssuesDate(string userName, List<Issue> issues)
        {
            GraphItem _performanceUser = new GraphItem();
           _performanceUser.x = userName;
           _performanceUser.y = GetPerformanceByIssues(issues.Where(m => m.AssignedUserName == userName).ToList());
           _performanceUser.Note = GetAverageNote(_performanceUser.y);
           return _performanceUser;
        }

        public static GraphItem GetPerformanceUserByIssues(List<Issue> issues)
        {
            GraphItem _performanceUser = new GraphItem();
            _performanceUser.y = GetPerformanceByIssues(issues);
            _performanceUser.Note = GetAverageNote(_performanceUser.y);

            return _performanceUser;
        }

        public decimal PerformanceOnDay()
        {
            throw new NotImplementedException();
        }

        public decimal PerformanceOfStaff(string userName, DateTime date)
        {
            throw new NotImplementedException();
        }

        public decimal PerformanceOfStaff(string userName, DateTime fromDate, DateTime endDate)
        {
            throw new NotImplementedException();
        }
    }
}