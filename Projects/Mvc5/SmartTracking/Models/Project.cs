using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartTracking.Models
{
    public class Project:BaseObject
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public bool Disabled { get; set; }
        public string ManagerUserName { get; set; }
        public DateTime BugNetDateCreated { get; set; }

        public DateTime CreateDate { set; get; }
        public string CreateBy { set; get; }
        public DateTime? ModifyDate { set; get; }
        public string ModifyBy { set; get; }

        public Project()
        {
            CreateDate = DateTime.Now;
        }
    }
}
