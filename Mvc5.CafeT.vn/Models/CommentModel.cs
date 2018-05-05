
using System;
using Web.Models;

namespace Mvc5.CafeT.vn.Models
{
    public class CommentModel:BaseObject
    {
        public string Title { set; get; }
        public string Content { set; get; }
       
        public CommentModel(string title):base()
        {
            Title = title;
        }

        public Guid? ArticleId { set; get; }
        public Guid? ProjectId { set; get; }
        public Guid? ExamId { set; get; }
        public Guid? CourseId { set; get; }
        public Guid? ProductId { set; get; }
        public int CountViews { set; get; } = 0;
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