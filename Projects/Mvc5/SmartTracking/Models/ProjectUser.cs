using CafeT.Frameworks.Identity.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartTracking.Models
{
    public class ProjectUser
    {
        [Key, Column(Order = 0)]
        public int ProjectId { get; set; }
        [Key, Column(Order = 1)]
        public string UserId { get; set; }

        public ICollection<ApplicationUser> Users { get; set; }
        public ICollection<Project> Projects { get; set; }
    }
}
