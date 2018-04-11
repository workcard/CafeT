using System.Collections.Generic;

namespace Web.Models
{
    public class EmailModel
    {
        public List<string> To { get; set; }
        public List<string> CC { get; set; }
        public List<string> BCC { get; set; }
        public string From { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string ServerName { get; set; }
        public int Port { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool EnableSsl { get; set; }

        public EmailModel()
        {
            To = new List<string>();
            CC = new List<string>();
            BCC = new List<string>();
        }
    }
}
