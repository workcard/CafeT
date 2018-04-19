using CafeT.Enumerable;
using Mvc5.CafeT.vn.Models;
using Mvc5.CafeT.vn.ModelViews;
using Repository.Pattern.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Mvc5.CafeT.vn.Controllers
{
    public class DashboardsController : BaseController
    {
        public DashboardsController(IUnitOfWorkAsync unitOfWorkAsync) : base(unitOfWorkAsync)
        {
        }

        [Authorize]
        public ActionResult GetDashboard()
        {
            var _unPublished = _articleManager.GetAllUnPublished();
            ViewBag.Articles = _unPublished;
            ViewBag.Count = _unPublished.Count();

            if(Request.IsAjaxRequest())
            {
                return PartialView("_UserDashboard");
            }
            else
            {
                return View("Dashboard");
            }
        }
    }
}
