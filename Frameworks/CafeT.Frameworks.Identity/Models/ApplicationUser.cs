using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace CafeT.Frameworks.Identity.Models
{
    public class ApplicationUser : IdentityUser<string, ApplicationUserLogin,
        ApplicationUserRole, ApplicationUserClaim>
    {
        [Required]
        [Display(Name ="Tên")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Họ & chữ lót")]
        public string LastName { get; set; }

        [Display(Name = "Tên để hiển thị")]
        public string DisplayName { get; set; }

        [Display(Name = "Ngày sinh")]
        public DateTime? Birthday { set; get; }

        public string AvatarPath { set; get; }

        public string UploadFolder { set; get; }

        public int? Points { set; get; } = 0;

        public int? CountViews { set; get; } = 0;

        [Display(Name = "Giới thiệu")]
        public string About { set; get; }

        public Guid? BugNetUserId { get; set; }

        public ICollection<ApplicationUserRole> UserRoles { get; set; }

        public ApplicationUser()
        {
            this.Id = Guid.NewGuid().ToString();
            //Points = 0;
            //CountViews = 0;
        }

        public string GetFullName()
        {
            string fullName = "";
            if (!string.IsNullOrEmpty(LastName))
                fullName += LastName;
            if (!string.IsNullOrEmpty(FirstName))
            {
                if(!string.IsNullOrEmpty(fullName))
                {
                    fullName += " ";
                }
                fullName += FirstName;
            }
            return fullName;
        }

        //public async Task<ClaimsIdentity> GenerateUserIdentityAsync(ApplicationUserManager manager)
        //{
        //    var userIdentity = await manager
        //        .CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
        //    return userIdentity;
        //}        
    }
}