using CafeT.Html;
using CafeT.Text;
using SmartTracking.Models;
using SmartTracking.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartTracking.Mappers
{
    public class ProjectMappers
    {
        public static ProjectViewModel ProjectToViewModel(Project model)
        {
            ProjectViewModel view = new ProjectViewModel();
            view.Id = model.Id;
            view.Name = model.Name;
            view.Code = model.Code;
            view.Description = model.Description.HtmlToText();
            view.Disabled = model.Disabled;
            view.ManagerUserName = model.ManagerUserName;
            view.BugNetDateCreated = model.BugNetDateCreated;

            return view;
        }

        public static List<ProjectViewModel> ProjectToViewModels(List<Project> models)
        {
            List<ProjectViewModel> views = new List<ProjectViewModel>();
            foreach (var model in models)
            {
                views.Add(ProjectToViewModel(model));
            }
            return views;
        }

        public static Project ProjectViewToModel(ProjectViewModel view)
        {
            Project model = new Project();
            model.Id = view.Id;
            model.Name = view.Name;
            model.Code = view.Code;
            model.Description = view.Description;
            model.Disabled = view.Disabled;
            model.ManagerUserName = view.ManagerUserName;
            model.BugNetDateCreated = view.BugNetDateCreated;

            return model;
        }

        public static List<Project> ProjectViewToModels(List<ProjectViewModel> views)
        {
            List<Project> models = new List<Project>();
            foreach (var view in views)
            {
                models.Add(ProjectViewToModel(view));
            }
            return models;
        }
    }
}
