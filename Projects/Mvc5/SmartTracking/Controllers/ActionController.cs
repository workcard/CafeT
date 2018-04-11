using SmartTracking.Models;
using SmartTracking.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SmartTracking.Controllers
{
    public class ActionController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Daily()
        {
            List<NewIssue> issues = new List<NewIssue>();
            issues = db.NewIssues.ToList();
            string _result = IssueRepositories.CreateIssues(issues);

            ViewBag.Result = _result;
            return View();
        }

        public ActionResult Weekly()
        {
            return View();
        }

        public ActionResult Monthly()
        {
            return View();
        }
    }
}