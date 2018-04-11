using SmartTracking.Mappers;
using SmartTracking.Models;
using SmartTracking.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Repository.Pattern.UnitOfWork;
using SmartTracking.Repositories;

namespace SmartTracking.Controllers
{
    public class ProjectController : BaseController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ProjectController(IUnitOfWorkAsync unitOfWorkAsync) : base(unitOfWorkAsync)
        {
        }

        public ActionResult Index()
        {
            List<Project> _projects = _unitOfWorkAsync.RepositoryAsync<Project>().Query().Select().ToList();


            return View(ProjectMappers.ProjectToViewModels(_projects));
        }

        //
        // GET: /Project/Create
        //[AllowAnonymous]
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Project/Create
        [HttpPost]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Create([Bind(Include = "Id,Name,Code,Description,Disabled,ManagerUserName,BugNetDateCreated")] ProjectViewModel projectView)
        {
            if (ModelState.IsValid)
            {
                Project _project = ProjectMappers.ProjectViewToModel(projectView);
                _project.Id = projectView.Id;
                _project.CreateDate = DateTime.Now;
                _project.CreateBy = User.Identity.Name;
                db.Projects.Add(_project);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(projectView);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult CheckProjectId(int id)
        {
            List<Project> _projects = ProjectRepositories.GetProjectById(id);
            ProjectViewModel _projectViewModel = new ProjectViewModel();

            if (_projects.Count > 0)
                _projectViewModel = ProjectMappers.ProjectToViewModel(_projects.FirstOrDefault());

            return View("Create", _projectViewModel);
        }
    }
}