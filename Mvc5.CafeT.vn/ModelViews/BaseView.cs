using System;
using System.ComponentModel.DataAnnotations;

namespace Mvc5.CafeT.vn.ModelViews
{
    public abstract class BaseView
    {
        [Key]
        public Guid Id { set; get; }
        public DateTime CreatedDate { set; get; }
        public string CreatedBy { set; get; }
        public DateTime? LastUpdatedDate { set; get; }
        public string LastUpdatedBy { set; get; }

        public string PageTitle { get; set; }
        public string MetaKeywords { get; set; }
        public string MetaDescription { get; set; }

        public bool IsOf(string userName)
        {
            if (this.CreatedBy.ToLower() == userName.ToLower()) return true;
            return false;
        }
        public virtual bool IsNews(int days = 3)
        {
            if (this.CreatedDate.AddDays(days) >= DateTime.Now)
            {
                return true;
            }
            return false;
        }
    }
}