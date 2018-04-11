using CafeT.Text;
using System;
using System.ComponentModel.DataAnnotations;


namespace Web.Models
{
    public class BaseObject : CafeT.EF6.Objects.BaseObject
    {
        [Key]
        public Guid Id { get; set; }
        [ScaffoldColumn(false)]
        public DateTime? CreatedDate { set; get; }
        [ScaffoldColumn(false)]
        public DateTime? UpdatedDate { set; get; }
        [ScaffoldColumn(false)]
        public string UpdatedBy { set; get; }
        [ScaffoldColumn(false)]
        public string CreatedBy { set; get; }

        public BaseObject()
        {
            Id = Guid.NewGuid();
            CreatedDate = DateTime.Now;
        }

        public virtual bool IsOf(string userName)
        {
            if (!CreatedBy.IsNullOrEmptyOrWhiteSpace() && (CreatedBy.ToLower() == userName)) return true;
            return false;
        }
    }
}