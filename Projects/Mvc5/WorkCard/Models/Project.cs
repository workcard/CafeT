using System.Collections.Generic;

namespace Web.Models
{
    public class Project : BaseObject
    {
        public string Title { set; get; }
        public string Description { set; get; }

        public virtual IEnumerable<WorkIssue> Issues { set; get; }
        public virtual IEnumerable<Question> Questions { set; get; }
        public virtual IEnumerable<Comment> Comments { set; get; }
        public virtual IEnumerable<Document> Documents { set; get; }
        public virtual IEnumerable<Contact> Contacts { set; get; }
        public virtual IEnumerable<Story> Stories { set; get; }
        public Project() : base() { }
        public Project(string Name) : base() { Title = Name; }
    }
}