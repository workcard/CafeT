using System;
using System.ComponentModel.DataAnnotations;

namespace SmartTracking.ViewModels
{
    public class ProjectViewModel
    {
        [Display(Name = "Id")]
        public int Id { get; set; }
        [Display(Name = "Name")]
        public string Name { get; set; }
        [Display(Name = "Code")]
        public string Code { get; set; }
        [Display(Name = "Description")]
        public string Description { get; set; }
        [Display(Name = "Disabled")]
        public bool Disabled { get; set; }
        [Display(Name = "Manager UserName")]
        public string ManagerUserName { get; set; }
        [Display(Name = "BugNet Date Created")]
        public DateTime BugNetDateCreated { get; set; }
    }

    public class ProjectUserViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Assigned { get; set; }
    }
}
