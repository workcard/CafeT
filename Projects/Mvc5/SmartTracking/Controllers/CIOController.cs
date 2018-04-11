using SmartTracking.Helpers;
using SmartTracking.Models;
using SmartTracking.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SmartTracking.Controllers
{
    public class CIOController : Controller
    {
        public static DateTime _today = DateHelpers.GetToDay();
        public static DateTime _firstDayOfWeek = DateHelpers.FirstDayOfWeek(_today);
        public static DateTime _lastDayOfWeek = DateHelpers.LastDayOfWeek(_today);
        private static ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult GeneralDashboard()
        {
            return View(CIOHelpers.GetGeneralCIOViewModel());
        }

        public ActionResult TimesDashboard()
        {
            return View(CIOHelpers.GetCIOTimesViewModel(_firstDayOfWeek, _lastDayOfWeek));
        }

        [HttpPost]
        public ActionResult TimesDashboard(CIOTimesViewModel model)
        {
            return View(CIOHelpers.GetCIOTimesViewModel(model.FromDate, model.ToDate));
        }

        public ActionResult IssuesOnTimes()
        {
            return View(CIOHelpers.GetListIssuesForDateViewModel(_firstDayOfWeek, _lastDayOfWeek));
        }

        [HttpPost]
        public ActionResult IssuesOnTimes(CIOListIssuesForDateViewModel model)
        {
            return View(CIOHelpers.GetListIssuesForDateViewModel(model.FromDate, model.ToDate));
        }

        public ActionResult StaffDashboard(string userName)
        {
            if ((userName == "" || userName == null) && @Request.IsAuthenticated)
                userName = User.Identity.Name;

            StaffTimesViewModel _staffTimes = StaffHelpers.GetPerformanceInDateViewModel(_firstDayOfWeek, _lastDayOfWeek, userName);
            ViewBag.BarChart = DataChartHelpers.GetStaffLineBarChart(_staffTimes.Performances);
            ViewBag.Users = db.Users.ToList();

            return View(_staffTimes);
        }

        [HttpPost]
        public ActionResult StaffDashboard(StaffTimesViewModel model)
        {
            StaffTimesViewModel _staffTimes = StaffHelpers.GetPerformanceInDateViewModel(model.FromDate, model.ToDate, model.UserName);
            ViewBag.BarChart = DataChartHelpers.GetStaffLineBarChart(_staffTimes.Performances);
            ViewBag.Users = db.Users.ToList();

            return View(_staffTimes);
        }

        public ActionResult OrderTimes()
        {
            StaffTimesViewModel _staffTimes = StaffHelpers.GetPerformanceInDateViewModel(_firstDayOfWeek, _lastDayOfWeek);
            ViewBag.BarChart = DataChartHelpers.GetStaffLineBarChart(_staffTimes.Performances);
            ViewBag.Users = db.Users.ToList();

            return View(_staffTimes);
        }

        [HttpPost]
        public ActionResult OrderTimes(StaffTimesViewModel model)
        {
            StaffTimesViewModel _staffTimes = StaffHelpers.GetPerformanceInDateViewModel(model.FromDate, model.ToDate, model.UserName);
            ViewBag.BarChart = DataChartHelpers.GetStaffLineBarChart(_staffTimes.Performances);
            ViewBag.Users = db.Users.ToList();

            return View(_staffTimes);
        }
    }
}