using CafeT.Text;
using Repository.Pattern.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using Web.Managers;
using Web.Models;
using Web.ModelViews;

namespace Web.Controllers
{
    public class ProjectsController : BaseController
    {
        public ProjectsController(IUnitOfWorkAsync unitOfWorkAsync) : base(unitOfWorkAsync)
        {
        }
        [HttpGet]
        [Authorize]
        public ActionResult LoadSummary(Guid id)
        {
            var project = ProjectManager.GetById(id);
            var issues = ProjectManager.GetIssues(id);
            ProjectSummary summary = new ProjectSummary(project,issues);

            if (Request.IsAjaxRequest())
            {
                return PartialView("Projects/_Summary", summary);
            }
            return RedirectToAction("Index");
        }
        [HttpGet]
        [Authorize]
        public ActionResult LoadProjects()
        {
            var projects = ProjectManager.GetAllOf(User.Identity.Name);
            
            if (Request.IsAjaxRequest())
            {
                return PartialView("_Projects", projects);
            }
            return RedirectToAction("Index");
        }
        //[HttpPost]
        //public JsonResult AutoCompleted(string projectId, string Prefix)
        //{
        //    //string projectId = string.Empty;
        //    Guid _projectId = Guid.NewGuid();
        //    if (!projectId.IsNullOrEmptyOrWhiteSpace())
        //        _projectId = Guid.Parse(projectId);

        //    //Note : you can bind same list from database   
        //    Dictionary<string, string> _dict = new Dictionary<string, string>();
        //    var _contacts = ProjectManager.GetContacts(_projectId);
        //    foreach(var _contact in _contacts)
        //    {
        //        if(!_contact.FirstName.IsNullOrEmptyOrWhiteSpace())
        //            _dict.Add(_contact.FirstName, _contact.Email);
        //        if (!_contact.LastName.IsNullOrEmptyOrWhiteSpace())
        //            _dict.Add(_contact.LastName, _contact.Email);
        //    }
        //    char _lastChar = Prefix.ToCharArray().LastOrDefault();
        //    if(_lastChar == '@')
        //    {
        //        var _emails = _contacts.Select(t => t.Email).Distinct();
        //        foreach (var _email in _emails)
        //        {
        //            _dict.Add(_email, _email);
        //        }
        //    }

        //    //Process Prefix
        //    string _text = Prefix;
        //    string _lastWord = Prefix.ToWords().LastOrDefault();
        //    if (_text.EndsWith(" "))
        //    {
        //        _text = _text.DeleteEndTo(" ");
        //    }

        //    if(!_lastWord.IsNullOrEmptyOrWhiteSpace())
        //        Prefix = _lastWord;

        //    var _keyWord = _dict.Where(t => t.Key.Contains(Prefix) || t.Value.Contains(Prefix)).Select(t => t.Value);
        //    return Json(_keyWord, JsonRequestBehavior.AllowGet);
        //}

        private ApplicationDbContext db = new ApplicationDbContext();

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [ValidateInput(false)]
        public async Task<ActionResult> AddContact(Guid projectId, FormCollection collection)
        {
            List<Contact> _contacts = new List<Contact>();
            var _project = ProjectManager.GetById(projectId);
            var _emails = collection["Content"].ToString().GetEmails();

            if(_contacts != null)
            {
                foreach(string _email in _emails)
                {
                    Contact _Model = new Contact(_email);
                    _Model.ProjectId = projectId;
                    _Model.CreatedBy = User.Identity.Name;
                    _contacts.Add(_Model);
                }
            }
            if (Request.IsAjaxRequest())
            {
                return PartialView("Issues/_WorkTime", "Added contacts");
            }
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> Index()
        {
            var _objects = _unitOfWorkAsync.Repository<Project>()
                                .Query().Select().Where(m => !string.IsNullOrWhiteSpace(m.Title))
                                .OrderByDescending(t => t.CreatedDate).AsEnumerable();
            if(User.Identity.IsAuthenticated)
            {
                _objects = _objects.Where(t => t.CreatedBy == User.Identity.Name).AsEnumerable();
            }
            foreach(var Model in _objects)
            {
                Model.Issues = ProjectManager.GetIssues(Model.Id);
                Model.Questions = ProjectManager.GetQuestions(Model.Id);
            }
            return View("Index", _objects);
        }

        public async Task<ActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = await db.Projects.FindAsync(id);
            project.Issues = ProjectManager.GetIssues(project.Id);
            project.Contacts = ProjectManager.GetContacts(project.Id).ToList();

            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [ValidateInput(false)]
        public async Task<ActionResult> Create(Project project)
        {
            if (ModelState.IsValid)
            {
                project.Id = Guid.NewGuid();
                project.CreatedBy = User.Identity.Name;
                db.Projects.Add(project);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(project);
        }

        // GET: Projects/Edit/5
        [Authorize]
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = await db.Projects.FindAsync(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [ValidateInput(false)]
        public async Task<ActionResult> Edit(Project project)
        {
            if (ModelState.IsValid)
            {
                project.UpdatedDate = DateTime.Now;
                project.UpdatedBy = User.Identity.Name;
                db.Entry(project).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(project);
        }

        
        [Authorize]
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = await db.Projects.FindAsync(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // POST: Projects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            Project project = await db.Projects.FindAsync(id);
            db.Projects.Remove(project);
            await db.SaveChangesAsync();
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
