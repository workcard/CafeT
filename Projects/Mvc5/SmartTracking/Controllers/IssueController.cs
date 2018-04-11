using SmartTracking.Helpers;
using SmartTracking.Mappers;
using SmartTracking.Models;
using SmartTracking.Models.Objects;
using SmartTracking.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace SmartTracking.Controllers
{
    public class IssueController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private string UploadFolder = "~/Content/Uploads/";

        public ActionResult IssuesManager()
        {
            return View();
        }

        public ActionResult Index()
        {
            return View(db.NewIssues.ToList());
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,IssueTitle,IssueDescription,ProjectId,IssueCategoryId,IssueStatusId,IssuePriorityId,IssueMilestoneId,IssueAffectedMilestoneId,IssueTypeId,IssueAssignedUserName,IssueCreatorUserName,IssueOwnerUserName,IssueDueDate,IssueVisibility,IssueEstimation,IssueProgress,IssueType")] NewIssue newIssue)
        {
            if (ModelState.IsValid)
            {
                db.NewIssues.Add(newIssue);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(newIssue);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult CheckIssueId(int id)
        {
            Issue _issues = IssueRepositories.GetIssueDetails(id);
            NewIssue _issueNew = IssueMappers.NewIssueToViewModel(_issues);

            return View("Create", _issueNew);
        }

        public ActionResult IsueDetails(int id)
        {
            return View(IssueRepositories.GetIssueDetails(id));
        }

        [HttpPost]
        public ActionResult Close(int id)
        {
            return View(IssueRepositories.GetIssueDetails(id));
        }

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

        public string UpdateFile(HttpPostedFileBase FileImport)
        {
            if (FileImport != null)
            {
                try
                {
                    string FileEXT = Path.GetExtension(FileImport.FileName);
                    string FileName = Path.GetFileNameWithoutExtension(FileImport.FileName);
                    string FileNameEXTNew = FileName + "_" + String.Format("{0:yyyyMMddHHmmss}", DateTime.Now) + FileEXT;
                    string saveFileName = Path.Combine(Server.MapPath(UploadFolder), FileNameEXTNew);
                    string extensionName = Path.GetExtension(FileImport.FileName);
                    Session["FileNameEXTNew"] = FileNameEXTNew;
                    FileImport.SaveAs(saveFileName);
                    ViewBag.Message = "File uploaded successfully";
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }

                return Server.MapPath(UploadFolder) + Session["FileNameEXTNew"];
            }
            else
            {
                return "";
            }
        }

        [HttpPost]
        public ActionResult IssuesManager(HttpPostedFileBase FileImport)
        {
            IssueHelpers fileImport = new IssueHelpers();
            List<SmartIssue> data = null;

            try
            {
                data = fileImport.GetIssuesFromFile(UpdateFile(FileImport)).ToList();
                Session["DataImport"] = data;
            }
            catch
            {
                ViewBag.File = "File error!";
            }

            return View(data);
        }

        [HttpPost]
        public ActionResult AddIssues(List<SmartIssue> fileImport)
        {
            try
            {
                Session["Result"] = IssueRepositories.CreateNewIssue((List<SmartIssue>)Session["DataImport"]);
            }
            catch
            {
                Session["Result"] = "File error!";
            }

            return RedirectToAction("IssuesManager", fileImport);
        }
    }
}