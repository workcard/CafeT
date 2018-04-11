using CafeT.Frameworks.Identity.Models;
using SmartTracking.Models;
using SmartTracking.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartTracking.Mappers
{
    //public class UserMappers
    //{
    //    public static RegisterViewModel RegisterToViewModel(ProfileUserViewModel model)
    //    {
    //        RegisterViewModel view = new RegisterViewModel();
    //        view.BugNetUserId = model.BugNetUserId;
    //        view.UserName = model.UserName;
    //        view.FirstName = model.FirstName;
    //        view.LastName = model.LastName;
    //        view.DisplayName = model.DisplayName;
    //        view.Email = model.Email;

    //        return view;
    //    }

    //    public static List<RegisterViewModel> UserProfileToViewModels(List<ProfileUserViewModel> models)
    //    {
    //        List<RegisterViewModel> views = new List<RegisterViewModel>();
    //        foreach (var model in models)
    //        {
    //            views.Add(RegisterToViewModel(model));
    //        }
    //        return views;
    //    }

    //    public static UserViewModel UserToViewModel(ApplicationUser model)
    //    {
    //        UserViewModel view = new UserViewModel();
    //        view.UserId = model.Id;
    //        view.BugNetUserId = model.BugNetUserId;
    //        view.UserName = model.UserName;
    //        view.FirstName = model.FirstName;
    //        view.LastName = model.LastName;
    //        view.DisplayName = model.DisplayName;
    //        view.Email = model.Email;

    //        return view;
    //    }

    //    public static List<UserViewModel> UserToViewModels(List<ApplicationUser> models)
    //    {
    //        List<UserViewModel> views = new List<UserViewModel>();
    //        foreach (var model in models)
    //        {
    //            views.Add(UserToViewModel(model));
    //        }
    //        return views;
    //    }

    //    public static ProfileUserViewModel ProfileUserToViewModel(ApplicationUser model)
    //    {
    //        ProfileUserViewModel view = new ProfileUserViewModel();
    //        view.BugNetUserId = model.BugNetUserId;
    //        view.BugNetUserId = model.BugNetUserId;
    //        view.UserName = model.UserName;
    //        view.FirstName = model.FirstName;
    //        view.LastName = model.LastName;
    //        view.DisplayName = model.DisplayName;
    //        view.Email = model.Email;

    //        return view;
    //    }

    //    public static List<ProfileUserViewModel> ProfileUserToViewModels(List<ApplicationUser> models)
    //    {
    //        List<ProfileUserViewModel> views = new List<ProfileUserViewModel>();
    //        foreach (var model in models)
    //        {
    //            views.Add(ProfileUserToViewModel(model));
    //        }
    //        return views;
    //    }

    //    public static UserViewModel UserBugNetToViewModel(ProfileUserViewModel userBugNet, UserViewModel user)
    //    {
    //        UserViewModel view = new UserViewModel();
    //        view.UserId = user.UserId;
    //        view.BugNetUserId = userBugNet.BugNetUserId;
    //        view.UserName = userBugNet.UserName;
    //        view.FirstName = userBugNet.FirstName;
    //        view.LastName = userBugNet.LastName;
    //        view.DisplayName = userBugNet.DisplayName;
    //        view.Email = userBugNet.Email;

    //        return view;
    //    }

    //    public static List<UserViewModel> UserBugNetToViewModels(List<ProfileUserViewModel> usersBugNet, List<UserViewModel> users)
    //    {
    //        List<UserViewModel> views = new List<UserViewModel>();
    //        foreach (var user in users)
    //        {
    //            ProfileUserViewModel _userProfile = new ProfileUserViewModel();
    //            _userProfile = usersBugNet.FirstOrDefault(m => m.UserName == user.UserName);

    //            views.Add(UserBugNetToViewModel(_userProfile, user));
    //        }
    //        return views;
    //    }
    //}
}