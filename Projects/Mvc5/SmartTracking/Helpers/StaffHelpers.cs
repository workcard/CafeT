using SmartTracking.Mappers;
using SmartTracking.Models;
using SmartTracking.Models.Objects;
using SmartTracking.Repositories;
using SmartTracking.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace SmartTracking.Helpers
{
    public class StaffHelpers
    {
        private static ApplicationDbContext db = new ApplicationDbContext();

        public static DateTime _today = DateHelpers.GetToDay();
        public static DateTime _lastWorkingDay = DateHelpers.GetLastWorkingDate(_today);
        public static DateTime _nextWorkingDay = DateHelpers.GetNextWorkingDate(_today);
        public static DateTime _firstDayOfWeek = DateHelpers.FirstDayOfWeek(_today);
        public static DateTime _lastDayOfWeek = DateHelpers.LastDayOfWeek(_today);

        public static GeneralStaffViewModel GetGeneralStaffViewModel( string userName)
        {
            List<Project> _projects = db.Projects.ToList();
            List<Issue> _issuesForUserAllProject = IssueRepositories.GetIssuesByUser(userName);

            List<Issue> _issuesForUser = new List<Issue>();
            foreach (var project in _projects)
            {
                _issuesForUser.AddRange(_issuesForUserAllProject.Where(m => m.ProjectId == project.Id).ToList());
            }

            GeneralStaffViewModel _generalStaff = new GeneralStaffViewModel();
            _generalStaff.PerfomanceLastWorkingDay = PerformanceHelpers.GetPerformanceUserByIssues(_issuesForUser.Where(m => m.IssueDueDate == _lastWorkingDay).ToList());
            _generalStaff.PerfomanceToday = PerformanceHelpers.GetPerformanceUserByIssues(_issuesForUser.Where(m => m.IssueDueDate == _today).ToList());
            _generalStaff.PerfomanceNextWorkingDay = PerformanceHelpers.GetPerformanceUserByIssues(_issuesForUser.Where(m => m.IssueDueDate == _nextWorkingDay).ToList());

            _generalStaff.IssuesNotFinished = IssueMappers.IssueToViewModels(_issuesForUser.Where(m => m.IsClosed == false).ToList());
            _generalStaff.IssuesFuture = IssueMappers.IssueToViewModels(_issuesForUser.Where(m => m.IssueDueDate >= _today).ToList());

            return _generalStaff;
        }

        public static StaffTimesViewModel GetPerformanceInDateViewModel(DateTime fromDate, DateTime toDate)
        {
            string user = "quy.hv";
            List<Issue> _issuesInTimes = IssueRepositories.GetIssuesProjectIdFromDateToDate(fromDate, toDate);
            List<Project> _projects = db.Projects.ToList();
            List<Issue> _issuesNotFinishedTemp = IssueRepositories.GetIssuesNotFinishedByDate(_today);
            List<Issue> _issuesFutureTemp = IssueRepositories.GetIssuesFutureByDate(_today);

            List<Issue> _issuesNotFinished = new List<Issue>();
            List<Issue> _issuesFuture = new List<Issue>();
            List<Issue> issuesInTimes = new List<Issue>();

            foreach (var project in _projects)
            {
                issuesInTimes.AddRange(_issuesInTimes.Where(m => m.ProjectId == project.Id).ToList());
                _issuesNotFinished.AddRange(_issuesNotFinishedTemp.Where(m => m.ProjectId == project.Id).ToList());
                _issuesFuture.AddRange(_issuesFutureTemp.Where(m => m.ProjectId == project.Id).ToList());
            }

            StaffTimesViewModel staffTimes = new StaffTimesViewModel();
            List<GraphItem> issues = new List<GraphItem>();
            staffTimes.FromDate = fromDate;
            staffTimes.ToDate = toDate;

            DateTime _dateTemp = staffTimes.FromDate;
            while (_dateTemp <= staffTimes.ToDate)
            {
                GraphItem _issue = new GraphItem();
                _issue.x = _dateTemp.ToShortDateString();
                _issue.y = PerformanceHelpers.GetPerformanceByIssues(issuesInTimes.Where(m => DateHelpers.IsEquals(m.IssueDueDate, _dateTemp) && m.AssignedUserName == user).ToList());

                _dateTemp = _dateTemp.AddDays(1);

                issues.Add(_issue);
            }
            staffTimes.Performances = issues;
            staffTimes.UserName = user;
            staffTimes.IssuesNotFinished = IssueMappers.IssueToViewModels(_issuesNotFinished.Where(m => m.AssignedUserName == user).ToList());
            staffTimes.IssuesFuture = IssueMappers.IssueToViewModels(_issuesFuture.Where(m => m.AssignedUserName == user).ToList());
            staffTimes.IssuesAll = IssueMappers.IssueToViewModels(issuesInTimes.Where(m => m.AssignedUserName == user).ToList());

            return staffTimes;
        }

        public static StaffTimesViewModel GetPerformanceInDateViewModel(DateTime fromDate, DateTime toDate, string user)
        {
            List<Issue> _issuesInTimes = IssueRepositories.GetIssuesProjectIdFromDateToDate(fromDate, toDate);
            List<Project> _projects = db.Projects.ToList();
            List<Issue> _issuesNotFinishedTemp = IssueRepositories.GetIssuesNotFinishedByDate(_today);
            List<Issue> _issuesFutureTemp = IssueRepositories.GetIssuesFutureByDate(_today);

            List<Issue> _issuesNotFinished = new List<Issue>();
            List<Issue> _issuesFuture = new List<Issue>();
            List<Issue> issuesInTimes = new List<Issue>();

            foreach (var project in _projects)
            {
                issuesInTimes.AddRange(_issuesInTimes.Where(m => m.ProjectId == project.Id).ToList());
                _issuesNotFinished.AddRange(_issuesNotFinishedTemp.Where(m => m.ProjectId == project.Id).ToList());
                _issuesFuture.AddRange(_issuesFutureTemp.Where(m => m.ProjectId == project.Id).ToList());
            }

            StaffTimesViewModel staffTimes = new StaffTimesViewModel();
            List<GraphItem> issues = new List<GraphItem>();
            staffTimes.FromDate = fromDate;
            staffTimes.ToDate = toDate;

            DateTime _dateTemp = staffTimes.FromDate;
            while (_dateTemp <= staffTimes.ToDate)
            {
                GraphItem _issue = new GraphItem();
                _issue.x = _dateTemp.ToShortDateString();
                _issue.y = PerformanceHelpers.GetPerformanceByIssues(issuesInTimes.Where(m => DateHelpers.IsEquals(m.IssueDueDate, _dateTemp) && m.AssignedUserName == user).ToList());

                _dateTemp = _dateTemp.AddDays(1);

                issues.Add(_issue);
            }
            staffTimes.Performances = issues;
            staffTimes.UserName = user;
            staffTimes.IssuesNotFinished = IssueMappers.IssueToViewModels(_issuesNotFinished.Where(m => m.AssignedUserName == user).ToList());
            staffTimes.IssuesFuture = IssueMappers.IssueToViewModels(_issuesFuture.Where(m => m.AssignedUserName == user).ToList());
            staffTimes.IssuesAll = IssueMappers.IssueToViewModels(issuesInTimes.Where(m => m.AssignedUserName == user).ToList());

            return staffTimes;
        }
    }
}