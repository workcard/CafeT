using System.Collections.Generic;

namespace Web.Models
{
    public class Message
    {
        public string Title { set; get; }
        public string Content { set; get; }
        public List<string> ToEmails { set; get; } = new List<string>();

        public void Notify(EmailService emailService)
        {
            emailService.SendAsync(this);
        }
    }
}