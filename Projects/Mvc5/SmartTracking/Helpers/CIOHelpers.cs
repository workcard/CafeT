using CafeT.Frameworks.Identity.Models;
using SmartTracking.Mappers;
using SmartTracking.Models;
using SmartTracking.Models.Objects;
using SmartTracking.Repositories;
using SmartTracking.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartTracking.Helpers
{
    public class CIOHelpers
    {
        private static ApplicationDbContext db = new ApplicationDbContext();

        public static DateTime _today = DateHelpers.GetToDay();
        public static DateTime _lastWorkingDay = DateHelpers.GetLastWorkingDate(_today);
        public static DateTime _nextWorkingDay = DateHelpers.GetNextWorkingDate(_today);
        public static DateTime _firstDayOfWeek = DateHelpers.FirstDayOfWeek(_today);
        public static DateTime _lastDayOfWeek = DateHelpers.LastDayOfWeek(_today);

        public static GeneralCIOViewModel GetGeneralCIOViewModel()
        {
            List<ApplicationUser> _users = db.Users.Where(m => m.BugNetUserId != null).ToList();
            List<Project> _projects = db.Projects.ToList();

            List<Issue> _issuesThisWeekAllProject = IssueRepositories.GetIssuesProjectIdFromDateToDate(_firstDayOfWeek, _lastDayOfWeek);

            List<Issue> _issuesLastWorkingDayTemp = IssueRepositories.GetIssuesEQDate(_lastWorkingDay).Where(m => m.IsClosed == true).ToList();
            List<Issue> _issuesTodayTemp = IssueRepositories.GetIssuesEQDate(_today);
            List<Issue> _issuesNextWorkingDayTemp = IssueRepositories.GetIssuesEQDate(_nextWorkingDay);
            List<Issue> _issuesNotFinishedTemp = IssueRepositories.GetIssuesNotFinishedByDate(_today);
            List<Issue> _issuesFutureTemp = IssueRepositories.GetIssuesFutureByDate(_today);

            List<Issue> _issuesThisWeek = new List<Issue>();
            List<Issue> _issuesLastWorkingDay = new List<Issue>();
            List<Issue> _issuesToday = new List<Issue>();
            List<Issue> _issuesNextWorkingDay = new List<Issue>();
            List<Issue> _issuesNotFinished = new List<Issue>();
            List<Issue> _issuesFuture = new List<Issue>();

            foreach(var project in _projects)
            {
                _issuesThisWeek.AddRange(_issuesThisWeekAllProject.Where(m => m.ProjectId == project.Id).ToList());
                _issuesLastWorkingDay.AddRange(_issuesLastWorkingDayTemp.Where(m => m.ProjectId == project.Id).ToList());
                _issuesNextWorkingDay.AddRange(_issuesNextWorkingDayTemp.Where(m => m.ProjectId == project.Id).ToList());
                _issuesToday.AddRange(_issuesTodayTemp.Where(m => m.ProjectId == project.Id).ToList());

                _issuesNotFinished.AddRange(_issuesNotFinishedTemp.Where(m => m.ProjectId == project.Id).ToList());
                _issuesFuture.AddRange(_issuesFutureTemp.Where(m => m.ProjectId == project.Id).ToList());
            }


            GeneralCIOViewModel _generalCIO = new GeneralCIOViewModel();
            _generalCIO.PerfomanceAverageToday = PerformanceHelpers.GetPerformanceAverageByIssues(_issuesToday, _users.Count);
            _generalCIO.PerfomanceAverageLastWorkingDay = PerformanceHelpers.GetPerformanceAverageByIssues(_issuesLastWorkingDay, _users.Count);
            _generalCIO.PerfomanceAverageNextWorkingDay = PerformanceHelpers.GetPerformanceAverageByIssues(_issuesNextWorkingDay, _users.Count);
            _generalCIO.PerfomanceAverageThisWeek = PerformanceHelpers.GetPerformanceAverageByIssuesInWeek(_issuesThisWeek, _users.Count);

            _generalCIO.PerformancesLastWorkingDate = PerformanceHelpers.GetPerformanceUserByIssuesDate(_issuesLastWorkingDay).OrderByDescending(m => m.y).ToList();
            _generalCIO.PerformancesToday = PerformanceHelpers.GetPerformanceUserByIssuesDate(_issuesToday).OrderByDescending(m => m.y).ToList();
            _generalCIO.PerformancesNextWorkingDate = PerformanceHelpers.GetPerformanceUserByIssuesDate(_issuesNextWorkingDay).OrderByDescending(m => m.y).ToList();

            _generalCIO.IssuesNotFinished = IssueMappers.IssueToViewModels(_issuesNotFinished);
            _generalCIO.IssuesFuture = IssueMappers.IssueToViewModels(_issuesFuture);

            return _generalCIO;
        }

        public static CIOTimesViewModel GetCIOTimesViewModel(DateTime fromDate, DateTime toDate)
        {
            List<Issue> _issuesThisWeekAllProject = IssueRepositories.GetIssuesProjectIdFromDateToDate(fromDate, toDate);
            List<ApplicationUser> _users = db.Users.Where(m => m.BugNetUserId != null).ToList();
            List<Project> _projects = db.Projects.ToList();

            List<Issue> _issuesThisWeek = new List<Issue>();

            foreach (var project in _projects)
            {
                _issuesThisWeek.AddRange(_issuesThisWeekAllProject.Where(m => m.ProjectId == project.Id).ToList());
            }

            CIOTimesViewModel cioTimes = new CIOTimesViewModel();
            List<ListPerformanceUsersViewModel> listPer = new List<ListPerformanceUsersViewModel>();
            cioTimes.FromDate = fromDate;
            cioTimes.ToDate = toDate;

            DateTime _dateTemp = cioTimes.FromDate;
            while (_dateTemp <= cioTimes.ToDate)
            {
                ListPerformanceUsersViewModel _per = new ListPerformanceUsersViewModel();
                _per.Date = _dateTemp;
                if(_per.Date < DateTime.Today)
                    _per.PerformancesUsers = PerformanceHelpers.GetPerformanceUserByIssuesDate(_issuesThisWeek.Where(m => DateHelpers.IsEquals(m.IssueDueDate, _dateTemp) && m.IsClosed == true).ToList()).OrderByDescending(m => m.y).ToList();
                else
                    _per.PerformancesUsers = PerformanceHelpers.GetPerformanceUserByIssuesDate(_issuesThisWeek.Where(m => DateHelpers.IsEquals(m.IssueDueDate, _dateTemp)).ToList()).OrderByDescending(m => m.y).ToList();

                _dateTemp = _dateTemp.AddDays(1);

                listPer.Add(_per);
            }
            cioTimes.ListPerformances = listPer;

            return cioTimes;
        }

        public static CIOListIssuesForDateViewModel GetListIssuesForDateViewModel(DateTime fromDate, DateTime toDate)
        {
            List<Issue> _issuesInTimes = IssueRepositories.GetIssuesProjectIdFromDateToDate(fromDate, toDate);
            List<ApplicationUser> _users = db.Users.Where(m => m.BugNetUserId != null).ToList();
            List<Project> _projects = db.Projects.ToList();

            List<Issue> _issuesThisWeek = new List<Issue>();

            foreach (var project in _projects)
            {
                _issuesThisWeek.AddRange(_issuesInTimes.Where(m => m.ProjectId == project.Id).ToList());
            }

            CIOListIssuesForDateViewModel issuesForDate = new CIOListIssuesForDateViewModel();
            List<ListIssuesForDateViewModel> issues = new List<ListIssuesForDateViewModel>();
            issuesForDate.FromDate = fromDate;
            issuesForDate.ToDate = toDate;

            DateTime _dateTemp = issuesForDate.FromDate;
            while (_dateTemp <= issuesForDate.ToDate)
            {
                ListIssuesForDateViewModel _issue = new ListIssuesForDateViewModel();
                _issue.Date = _dateTemp;
                if (_issue.Date < DateTime.Today)
                    _issue.Issues = IssueMappers.IssueToViewModels(_issuesThisWeek.Where(m => DateHelpers.IsEquals(m.IssueDueDate, _dateTemp)).ToList()).OrderByDescending(m => m.IssueDueDate).ToList();
                else
                    _issue.Issues = IssueMappers.IssueToViewModels(_issuesThisWeek.Where(m => DateHelpers.IsEquals(m.IssueDueDate, _dateTemp)).ToList()).OrderByDescending(m => m.IssueDueDate).ToList();

                _dateTemp = _dateTemp.AddDays(1);

                issues.Add(_issue);
            }
            issuesForDate.ListIssuesForDate = issues;

            return issuesForDate;
        }

        //public static OrderTimesViewModel GetOrderTimesViewModel(DateTime fromDate, DateTime toDate)
        //{
        //    List<Issue> _issuesInTimes = IssueRepositories.GetIssuesProjectIdFromDateToDate(fromDate, toDate);
        //    List<ApplicationUser> _users = db.Users.Where(m => m.BugNetUserId != null).ToList();
        //    List<Project> _projects = db.Projects.ToList();

        //    List<Issue> _issuesThisWeek = new List<Issue>();

        //    foreach (var project in _projects)
        //    {
        //        _issuesThisWeek.AddRange(_issuesInTimes.Where(m => m.ProjectId == project.Id).ToList());
        //    }

        //    OrderTimesViewModel _orderTimes = new OrderTimesViewModel();
        //    List<PerformanceUserViewModel> _performanceUsers = new List<PerformanceUserViewModel>();

        //    _orderTimes.PhaseName = "";

        //    DateTime _dateTemp = issuesForDate.FromDate;
        //    while (_dateTemp <= issuesForDate.ToDate)
        //    {
        //        PerformanceUserViewModel _performance = new PerformanceUserViewModel();
        //        _performance.Date = _dateTemp;
        //        if (_performance.Date < DateTime.Today)
        //            _performance.Issues = IssueMappers.IssueToViewModels(_issuesThisWeek.Where(m => DateHelpers.IsEquals(m.IssueDueDate, _dateTemp)).ToList()).OrderByDescending(m => m.IssueDueDate).ToList();
        //        else
        //            _performance.Issues = IssueMappers.IssueToViewModels(_issuesThisWeek.Where(m => DateHelpers.IsEquals(m.IssueDueDate, _dateTemp)).ToList()).OrderByDescending(m => m.IssueDueDate).ToList();

        //        _dateTemp = _dateTemp.AddDays(1);

        //        _performanceUsers.Add(_performance);
        //    }
        //    _orderTimes.PerformancesUsers = _performanceUsers;

        //    return _orderTimes;
        //}
    }
}