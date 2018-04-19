using CafeT.BusinessObjects.ELearning;
using System;

namespace Mvc5.CafeT.vn.Models
{
    public class CommentModel:Comment
    {
        public Guid? ArticleId { set; get; }
        public Guid? ProjectId { set; get; }
        public Guid? ExamId { set; get; }
        public Guid? CourseId { set; get; }
        public Guid? ProductId { set; get; }

        public CommentModel():base()
        {
        }
        public virtual bool IsOfArticle(Guid id)
        {
            if (ArticleId.HasValue && ArticleId.Value == id) return true;
            return false;
        }
    }
}