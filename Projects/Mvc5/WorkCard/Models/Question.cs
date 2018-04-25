using CafeT.Objects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Web.Models
{
    public class Question : BaseObject
    {
        public string Title { set; get; }
        public string Content { set; get; }
        public virtual IEnumerable<Answer> Answers { set; get; }
        public Guid? ProjectId { set; get; }
        public Guid? StoryId { set; get; }
        public Guid? IssueId { set; get; }

        public bool IsRequired { set; get; } = false;
        public virtual IEnumerable<Contact> Contacts { set; get; }

        public IEnumerable<string> Links
        {
            get
            {
                return this.GetLinks();
            }
        }

        public Question() : base() { }

        public bool HasAnswer()
        {
            if (Answers == null || Answers.Count() < 1) return false;
            return true;
        }

        public void Notify(EmailService emailService)
        {
            emailService.SendAsync(this);
        }
    }
}