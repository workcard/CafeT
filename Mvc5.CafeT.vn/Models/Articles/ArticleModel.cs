using CafeT.BusinessObjects;
using CafeT.Text;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Mvc5.CafeT.vn.Models
{

    public class ArticleModel : Article//, ICloneable
    {
        public string EnglishContent { set; get; }
        public string VietnameseContent { set; get; }
        public int? Points { set; get; }

        public Guid? CourseId { set; get; }
        public Guid? ProjectId { set; get; }
        public Guid? CompanyId { set; get; }
        public Guid? JobId { set; get; }
        [Display(Name = "Phân loại")]
        public Guid? CategoryId { set; get; }

        public string Followers { set; get; }
        public string FromUrl { set; get; }

        public virtual ImageModel Avatar { set; get; }
        public virtual ArticleCategory Category { set; get; }
        public virtual IEnumerable<QuestionModel> Questions { set; get; }
        public virtual IEnumerable<CommentModel> Comments { set; get; }
        public virtual IEnumerable<FileModel> Files { set; get; }

        public ArticleModel() : base()
        {
            Points = 0;
        }

        //public ArticleModel(string title)
        //{
        //}

        public ArticleModel ToStandard()
        {
            Title = Title.ToStandard();
            Summary = Summary.ToStandard();
            Content = Content.ToStandard();
            return this;
        }

        public bool IsPublished()
        {
            if (Status == PublishStatus.IsPublished)
                return true;
            else
                return false;
        }

        //public Dictionary<string, string> GetWarning()
        //{
        //    return new Dictionary<string, string>();
        //}

        //public bool HasWarning()
        //{
        //    var _warnings = GetWarning();
        //    if(_warnings.Count > 0)
        //    {
        //        return true;
        //    }
        //    return false;
        //}
    }
}