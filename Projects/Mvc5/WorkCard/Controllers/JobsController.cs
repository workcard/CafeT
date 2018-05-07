using CafeT.Enumerable;
using Repository.Pattern.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using Web.Models;

namespace Web.Controllers
{
    public class JobsController : BaseController
    {
        private ApplicationDbContext dbContext = new ApplicationDbContext();
        public JobsController(IUnitOfWorkAsync unitOfWorkAsync) : base(unitOfWorkAsync)
        {
        }

        [HttpGet]
        [Authorize]
        public ActionResult Apply(Guid id)
        {
            Job model = JobManager.GetById(id);
            model.AddApplier(User.Identity.Name);
            bool _result = JobManager.Update(model);
            if(_result)
            {
                if (Request.IsAjaxRequest())
                {
                    return PartialView("Jobs/_Job", model);
                }
                return RedirectToAction("Index");
            }
            else
            {
                return PartialView("Messages/_Error", "Can not Apply");
            }
        }

        public async Task<ActionResult> Index()
        {
            ViewBag.HotJobs = JobManager.GetHotJobs(Const_DefaultGetItems);
            var _views = JobManager.GetAllAsync(null);
            return View(_views);
        }

        [HttpGet]
        [Authorize]
        public ActionResult AddQuestion(Guid jobId)
        {
            Question _question = new Question() { JobId = jobId };
            _question.CreatedBy = User.Identity.Name;

            if (Request.IsAjaxRequest())
            {
                return PartialView("Questions/_AddQuestionAjax", _question);
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize]
        public ActionResult AddIssue(Guid jobId)
        {
            WorkIssue issue = new WorkIssue() { JobId = jobId };
            if (Request.IsAjaxRequest())
            {
                return PartialView("Issues/_QuickCreateIssue", issue);
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult GetLastestJobs(int? n)
        {
            var _objects = _unitOfWorkAsync.RepositoryAsync<Job>()
                .Query().Select()
                .OrderByDescending(t => t.CreatedDate)
                .ThenByDescending(t => t.UpdatedDate)
                .TakeMax(n)
                .ToList();

            if (Request.IsAjaxRequest())
            {
                return PartialView("Jobs/_Jobs", _objects);
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult GetIssues(Guid id, int? n)
        {
            var _objects = _unitOfWorkAsync.RepositoryAsync<WorkIssue>()
                .Query().Select()
                .Where(t=>t.JobId.HasValue && t.JobId.Value == id)
                .OrderByDescending(t => t.CreatedDate)
                .ThenByDescending(t => t.UpdatedDate)
                .TakeMax(n)
                .ToList();
            var views = Mappers.IssueMappers.IssuesToViews(_objects);
            if (Request.IsAjaxRequest())
            {
                return PartialView("Issues/_Issues", views);
            }
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult GetLastestQuestions(Guid id, int? n)
        {
            var _objects = _unitOfWorkAsync.RepositoryAsync<Question>()
                .Query().Select()
                .Where(t=>t.JobId.HasValue && t.JobId.Value == id)
                .OrderByDescending(t => t.CreatedDate)
                .ThenByDescending(t => t.UpdatedDate)
                .TakeMax(n)
                .ToList();

            if (Request.IsAjaxRequest())
            {
                return PartialView("Questions/_Questions", _objects);
            }
            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<ActionResult> GetLastestAppliers(Guid id, int? n)
        {
            var _object = dbContext.Jobs.Find(id);

            var _appliers = _unitOfWorkAsync.RepositoryAsync<JobApplier>()
                .Query().Select()
                .Where(t => t.JobId.HasValue && t.JobId.Value == id)
                .OrderByDescending(t=>t.CreatedDate);

            if (Request.IsAjaxRequest())
            {
                return PartialView("Jobs/_Appliers", _appliers);
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult GetComments(Guid id, int?n)
        {
            var _objects = _unitOfWorkAsync.RepositoryAsync<Comment>()
                .Query().Select()
                .Where(t=>t.JobId.HasValue && t.JobId.Value == id)
                .OrderByDescending(t => t.CreatedDate)
                .ThenByDescending(t => t.UpdatedDate)
                .TakeMax(n)
                .ToList();

            if (Request.IsAjaxRequest())
            {
                return PartialView("Comments/_Comments", _objects);
            }
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Job job = await dbContext.Jobs.FindAsync(id);
            if (job == null)
            {
                return HttpNotFound();
            }
            if (Session["EndDate"] == null)
            {
                Session["EndDate"] = DateTime.Now.AddMinutes(10).ToString("dd-MM-yyyy h:mm:ss tt");
            }
            ViewBag.EndDate = Session["EndDate"];
            ViewBag.Comments = JobManager.GetCommentsOf(id.Value);
            return View(job);
        }

        // GET: Jobs/Create
        public ActionResult Create()
        {
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public async Task<ActionResult> Create(Job job)
        {
            if (ModelState.IsValid)
            {
                job.Id = Guid.NewGuid();
                dbContext.Jobs.Add(job);
                await dbContext.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(job);
        }

        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Job job = await dbContext.Jobs.FindAsync(id);
            if (job == null)
            {
                return HttpNotFound();
            }
            return View(job);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public async Task<ActionResult> Edit(Job job)
        {
            if (ModelState.IsValid)
            {
                dbContext.Entry(job).State = EntityState.Modified;
                await dbContext.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(job);
        }

        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Job job = await dbContext.Jobs.FindAsync(id);
            if (job == null)
            {
                return HttpNotFound();
            }
            return View(job);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            Job job = await dbContext.Jobs.FindAsync(id);
            dbContext.Jobs.Remove(job);
            await dbContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                dbContext.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
