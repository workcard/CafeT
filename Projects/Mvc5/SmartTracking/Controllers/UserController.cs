using CafeT.Frameworks.Identity.Models;
using SmartTracking.Mappers;
using SmartTracking.Models;
using SmartTracking.Repositories;
using SmartTracking.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SmartTracking.Controllers
{
    public class UserController : Controller
    {
        private ApplicationDbContext _db = new ApplicationDbContext();

        //
        // GET: /Account/Index
        [AllowAnonymous]
        public ActionResult Index()
        {
            var _usersList = _db.Users.ToList();
            List<UserViewModel> _userListView = new List<UserViewModel>();
            _userListView = UserMappers.UserToViewModels(_usersList);

            return View(_userListView);
        }

        private void GetListProject(string userId)
        {
            List<ProjectUser> _projectChosesed = _db.ProjectUsers.Where(m => m.UserId == userId).ToList();
            var allProjects = _db.Projects;
            var viewModel = new List<ProjectUserViewModel>();
            foreach (var project in allProjects)
            {
                viewModel.Add(new ProjectUserViewModel
                {
                    Id = project.Id,
                    Name = project.Name,
                    Assigned = _projectChosesed.Where(m => m.ProjectId == project.Id).ToList().Count >= 1 ? true : false
                });
            }
            ViewBag.Projects = viewModel;
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Edit(string id)
        {
            var _user = _db.Users.Find(id);
            UserViewModel _userView = new UserViewModel();
            _userView = UserMappers.UserToViewModel(_user);

            GetListProject(id);
            return View(_userView);
        }


        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit([Bind(Include = "UserId,BugNetUserId,UserName,FirstName,LastName,DisplayName,Email")] UserViewModel model, int[] selectedProjects)
        {
            if (ModelState.IsValid)
            {
                var _user = _db.Users.Find(model.UserId);
                _user.BugNetUserId = model.BugNetUserId;
                _user.FirstName = model.FirstName;
                _user.LastName = model.LastName;
                _user.DisplayName = model.DisplayName;
                _user.Email = model.Email;
                _db.Entry(_user).State = EntityState.Modified;
                _db.SaveChanges();

                //string _userId = model.UserId;
                //if (selectedProjects != null)
                //{
                //    foreach (var project in selectedProjects)
                //    {
                //        ProjectUser _projectUser = new ProjectUser();
                //        _projectUser.ProjectId = project;
                //        _projectUser.UserId = model.UserId;
                //        _db.ProjectUsers.Add(_projectUser);
                //        _db.SaveChanges();
                //    }
                //}
                return RedirectToAction("Index");
            }
            GetListProject(model.UserId);
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult CheckUserFromBugNet([Bind(Include = "UserId,BugNetUserId,UserName,FirstName,LastName,DisplayName,Email")] UserViewModel model)
        {
            List<ProfileUserViewModel> _userProfiles = UserProfileRepositories.GetUserProfileByUserName(model.UserName);
            UserViewModel _userView = new UserViewModel();
            _userView = UserMappers.UserBugNetToViewModel(_userProfiles.FirstOrDefault(), model);

            return View("Edit", _userView);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Delete(string id)
        {
            var _user = _db.Users.Find(id);
            UserViewModel _userView = new UserViewModel();
            _userView = UserMappers.UserToViewModel(_user);

            return View(_userView);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(string id)
        {
            var user = _db.Users.First(r => r.Id == id);
            _db.DeleteUser(_db, user.Id);
            return RedirectToAction("Index");
        }
    }
}