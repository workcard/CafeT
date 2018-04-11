using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SmartTracking.Models;

namespace SmartTracking.Controllers
{
    public class NewIssuesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: NewIssues
        public ActionResult Index()
        {
            return View(db.NewIssues.ToList());
        }

        // GET: NewIssues/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NewIssue newIssue = db.NewIssues.Find(id);
            if (newIssue == null)
            {
                return HttpNotFound();
            }
            return View(newIssue);
        }

        // GET: NewIssues/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: NewIssues/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,IssueTitle,IssueDescription,ProjectId,IssueCategoryId,IssueStatusId,IssuePriorityId,IssueMilestoneId,IssueAffectedMilestoneId,IssueTypeId,IssueResolutionId,IssueAssignedUserName,IssueCreatorUserName,IssueOwnerUserName,IssueDueDate,IssueVisibility,IssueEstimation,IssueProgress,IssueType")] NewIssue newIssue)
        {
            if (ModelState.IsValid)
            {
                db.NewIssues.Add(newIssue);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(newIssue);
        }

        // GET: NewIssues/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NewIssue newIssue = db.NewIssues.Find(id);
            if (newIssue == null)
            {
                return HttpNotFound();
            }
            return View(newIssue);
        }

        // POST: NewIssues/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,IssueTitle,IssueDescription,ProjectId,IssueCategoryId,IssueStatusId,IssuePriorityId,IssueMilestoneId,IssueAffectedMilestoneId,IssueTypeId,IssueResolutionId,IssueAssignedUserName,IssueCreatorUserName,IssueOwnerUserName,IssueDueDate,IssueVisibility,IssueEstimation,IssueProgress,IssueType")] NewIssue newIssue)
        {
            if (ModelState.IsValid)
            {
                db.Entry(newIssue).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(newIssue);
        }

        // GET: NewIssues/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NewIssue newIssue = db.NewIssues.Find(id);
            if (newIssue == null)
            {
                return HttpNotFound();
            }
            return View(newIssue);
        }

        // POST: NewIssues/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            NewIssue newIssue = db.NewIssues.Find(id);
            db.NewIssues.Remove(newIssue);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
