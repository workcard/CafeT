using CafeT.Text;
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

        public Comment() : base() { }
    }
}