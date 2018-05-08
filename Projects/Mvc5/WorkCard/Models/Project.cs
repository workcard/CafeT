using System.Collections.Generic;
using System.Linq;

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
        public virtual List<Contact> Contacts { set; get; }
        public virtual IEnumerable<Story> Stories { set; get; }
        public Project() : base() {
            Contacts = new List<Contact>();
        }
        public Project(string Name) : base() { Title = Name;
            Contacts = new List<Contact>();
        }
        public override bool IsOf(string userName)
        {
            if(Contacts != null && Contacts.Any())
            {
                bool _isContact = Contacts.Select(t => t.Email)
                .Contains(userName.ToLower());
                return base.IsOf(userName) || _isContact;
            }
            return false;
        }
    }
}