using System;

namespace Web.Models
{
    public class Comment : BaseObject
    {
        public string Title { set; get; }
        public string Content { set; get; }
        public Guid? ProjectId { set; get; }
        public Guid? IssueId { set; get; }
        public Guid? QuestionId { set; get; }
        public Guid? JobId { set; get; }
        public Guid? ArticleId { set; get; }
        public Guid? DocumentId { set; get; }

        public Comment() : base() { }
    }
}