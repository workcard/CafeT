using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SmartTracking.Controllers
{
    public class LeaderController : Controller
    {
        public ActionResult GeneralDashboard()
        {
            return View();
        }

        public ActionResult TimesDashboard()
        {
            return View();
        }
    }
}