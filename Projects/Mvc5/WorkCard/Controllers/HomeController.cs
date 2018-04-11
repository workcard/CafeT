using CafeT.Time;
using Repository.Pattern.UnitOfWork;
using System;
using System.Linq;
using System.Web.Mvc;
using Web.Mappers;
using Web.Models;
using Web.ModelViews;

namespace Web.Controllers
{
    public class HomeController : BaseController
    {
        public HomeController(IUnitOfWorkAsync unitOfWorkAsync) : base(unitOfWorkAsync)
        {
        }

        public ActionResult Index()
        {
            if(User.Identity.IsAuthenticated)
            {
                var _objects = IssueManager.SeeList(User.Identity.Name);
                var _views = IssueMappers.IssuesToViews(_objects.ToList());
                return View("Index", _views);
            }
            return View("Index");
        }
        public ActionResult Dashboards()
        {
            return View();
        }
        [HttpGet]
        [Authorize]
        public ActionResult LoadQuickIssue()
        {
            if (Request.IsAjaxRequest())
            {
                return PartialView("Issues/_QuickCreateIssue", new WorkIssue());
            }
            return View();
        }

        [HttpGet]
        [Authorize]
        public ActionResult GetTodaySummary()
        {
            var _objects =  _unitOfWorkAsync.RepositoryAsync<WorkIssue>()
                                .Query().Select()
                                .Where(t=>t.IsValid())
                                .Where(t=>t.End.HasValue && t.End.Value.IsToday())
                                .AsEnumerable();
            
            if (User.Identity.IsAuthenticated)
            {
                _objects = _objects.Where(t => t.IsOf(User.Identity.Name));
                DaySummary summary = new DaySummary(_objects);

                if (Request.IsAjaxRequest())
                {
                    return PartialView("_TodaySummary", summary);
                }
            }
          
            return View();
        }

        [HttpGet]
        [Authorize]
        public ActionResult GetMonthSummary()
        {
            var _objects = IssueManager.GetAllOf(User.Identity.Name)
                .Where(t => t.IsInMonth(DateTime.Now.Month));

            MonthSummary summary = new MonthSummary(_objects);

            if (Request.IsAjaxRequest())
            {
                return PartialView("Dashboards/_MonthDashboard", summary);
            }
            return View("Dashboards/_MonthDashboard", summary);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}