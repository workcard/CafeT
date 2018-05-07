using CafeT.Html;
using CafeT.Objects.Enums;
using CafeT.Text;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Web.Models;

namespace Web
{
    public class EmailService : IIdentityMessageService, IDisposable
    {
        public MailMessage BuildMessage(WorkIssue model)
        {
            MailMessage _msg = new MailMessage();
            string title = string.Empty;
            string footer = string.Empty;

            if(model.Status == IssueStatus.Done)
            {
                title = title + "[Done]";
            }
            if (model.Status == IssueStatus.New)
            {
                title = title + "[New]";
            }
            title = title + " " + model.Title.HtmlToText().ToStandard();

            _msg.Subject = title;
            _msg.Body = model.Content;
            _msg.IsBodyHtml = true;

            return _msg;
        }

        public Task SendAsync(IdentityMessage message)
        {
            MailMessage _msg = new MailMessage();
            
            _msg.Subject = message.Subject;
            _msg.To.Add(new MailAddress(message.Destination));
            _msg.Body = message.Body;
            _msg.IsBodyHtml = true;
            
            using (SmtpClient client = new SmtpClient
            {
                EnableSsl = true,
                Host = "smtp.gmail.com",
                Port = 587,
                Credentials = new NetworkCredential("taipm@workcard.vn", "P@$$w0rdPMT")
            })
            {
                client.Send(_msg);
            }
            return Task.FromResult(0);
        }

        public Task SendAsync(Message message)
        {
            MailMessage _msg = new MailMessage();

            _msg.Subject = message.Title;
            _msg.Body = message.Content;
            _msg.IsBodyHtml = true;
            
            if (message.ToEmails.Count > 0)
            {
                foreach(string email in message.ToEmails)
                {
                    _msg.To.Add(new MailAddress(email));
                }
            }
            
            using (SmtpClient client = new SmtpClient
            {
                EnableSsl = true,
                Host = "smtp.gmail.com",
                Port = 587,
                Credentials = new NetworkCredential("taipm@workcard.vn", "P@$$w0rdPMT")
            })
            {
                client.Send(_msg);
            }
            return Task.FromResult(0);
        }
     
        public string GetIssueUrl(WorkIssue model)
        {
            return "http://workcard.vn/workissues/details/" + model.Id.ToString();
        }
        public string GetQuestionUrl(Question model)
        {
            return "http://workcard.vn/Questions/details/" + model.Id.ToString();
        }
        public Task SendAsync(WorkIssue model, string toEmail)
        {
            MailMessage _msg = new MailMessage();
            _msg.Subject = "[Issue] " + model.Title.HtmlToText().ToStandard();
            _msg.To.Add(new MailAddress(toEmail));
            _msg.IsBodyHtml = true;
            _msg.Body = model.Content;
            _msg.Body += "<br />" + GetIssueUrl(model);
            using (SmtpClient client = new SmtpClient
            {
                EnableSsl = true,
                Host = "smtp.gmail.com",
                Port = 587,
                Credentials = new NetworkCredential("taipm@workcard.vn", "P@$$w0rdPMT")
            })
            {
                client.Send(_msg);
            }
            return Task.FromResult(0);
        }

        
        public Task SendAsync(Question model)
        {
            MailMessage _msg = new MailMessage();
            if(!model.Title.IsNullOrEmptyOrWhiteSpace())
            _msg.Subject = "[?] " + model.Title.HtmlToText().ToStandard();
            else
            {
                _msg.Subject = "[?] " + model.Content.HtmlToText().ToStandard()
                    .GetFirstSentence();
            }

            _msg.IsBodyHtml = true;
            _msg.Body = model.Content;
            _msg.Body += "<br /> Link: " + GetQuestionUrl(model);
           
            List<string> toEmails = model.Content.GetEmails().ToList();
            toEmails.Add(model.CreatedBy);

            if(model.IssueId.HasValue)
            {
                using (var dbContext = new ApplicationDbContext())
                {
                    var issue = dbContext.Issues.Find(model.IssueId.Value);
                    List<string> emails = issue.GetEmails().ToList();
                    toEmails.AddRange(emails);
                    _msg.Body += "<br /> Trong issue: " + issue.Title;
                    _msg.Body += "<br /> Link: " + GetIssueUrl(issue);
                }
            }

            toEmails = toEmails.Distinct().ToList();

            foreach(var toEmail in toEmails)
            {
                _msg.To.Add(new MailAddress(toEmail));
            }
            
            using (SmtpClient client = new SmtpClient
            {
                EnableSsl = true,
                Host = "smtp.gmail.com",
                Port = 587,
                Credentials = new NetworkCredential("taipm@workcard.vn", "P@$$w0rdPMT")
            })
            {
                client.Send(_msg);
            }
            return Task.FromResult(0);
        }

        public Task SendAsync(string title, string content, string toEmail)
        {
            MailMessage _msg = new MailMessage();
            _msg.Subject = title;

            _msg.To.Add(new MailAddress(toEmail));
            _msg.Body = content;
            _msg.IsBodyHtml = true;

            using (SmtpClient client = new SmtpClient
            {
                EnableSsl = true,
                Host = "smtp.gmail.com",
                Port = 587,
                Credentials = new NetworkCredential("taipm@workcard.vn", "P@$$w0rdPMT")
            })
            {
                client.Send(_msg);
            }
            return Task.FromResult(0);
        }

        public void Dispose()
        {
            this.Dispose();
        }
    }
}