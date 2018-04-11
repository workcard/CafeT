using System;
using System.ComponentModel.DataAnnotations;

namespace SmartTracking.ViewModels
{
    public class UserViewModel
    {
        [Display(Name = "UserId")]
        public string UserId { get; set; }

        [Display(Name = "BugNet UserId")]
        public Guid? BugNetUserId { get; set; }

        [Display(Name = "UserName")]
        public string UserName { get; set; }

        [Display(Name = "FirstName")]
        public string FirstName { get; set; }

        [Display(Name = "LastName")]
        public string LastName { get; set; }

        [Display(Name = "Display Name")]
        public string DisplayName { get; set; }

        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class ProfileUserViewModel
    {
        public Guid? BugNetUserId { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DisplayName { get; set; }
        public string Email { get; set; }
        public DateTime? LastUpdate { get; set; }
    }
}
