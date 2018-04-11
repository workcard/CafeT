
using SmartTracking.Helpers;
using SmartTracking.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SmartTracking.Controllers
{
    public class StaffController : Controller
    {
        public static DateTime _today = DateHelpers.GetToDay();
        public static DateTime _firstDayOfWeek = DateHelpers.FirstDayOfWeek(_today);
        public static DateTime _lastDayOfWeek = DateHelpers.LastDayOfWeek(_today);

        public ActionResult Issue()
        {
            return View(StaffHelpers.GetGeneralStaffViewModel(User.Identity.Name));
        }

        public ActionResult GeneralDashboard()
        {
            return View();
        }

        public ActionResult TimesDashboard()
        {
            string user = User.Identity.Name;
            if (user == null || user == "")
                user = "quy.hv";
            StaffTimesViewModel _staffTimes = StaffHelpers.GetPerformanceInDateViewModel(_firstDayOfWeek, _lastDayOfWeek, user);
            ViewBag.BarChart = DataChartHelpers.GetStaffLineBarChart(_staffTimes.Performances);

            return View(_staffTimes);
        }

        [HttpPost]
        public ActionResult TimesDashboard(StaffTimesViewModel model)
        {
            string user = User.Identity.Name;
            if (user == null || user == "")
                user = "quy.hv";
            StaffTimesViewModel _staffTimes = StaffHelpers.GetPerformanceInDateViewModel(model.FromDate, model.ToDate, user);
            ViewBag.BarChart = DataChartHelpers.GetStaffLineBarChart(_staffTimes.Performances);

            return View(_staffTimes);
        }
    }
}