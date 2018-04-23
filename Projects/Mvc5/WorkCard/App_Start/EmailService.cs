﻿using CafeT.Html;
using CafeT.Objects.Enums;
using CafeT.Text;
using Microsoft.AspNet.Identity;
using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Web.Controllers;
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

            //_msg.To.Add(new MailAddress(toEmail));

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
        //public string GenerateLink(WorkIssue model)
        //{
        //    return string.Empty;
        //    //WorkIssue model
        //}

        public string GetUrl(WorkIssue model)
        {
            return "http://workcard.vn/workissues/details/" + model.Id.ToString();
        }

        public Task SendAsync(WorkIssue model, string toEmail)
        {
            MailMessage _msg = new MailMessage();
            _msg.Subject = "[Issue] " + model.Title.HtmlToText().ToStandard();

            _msg.To.Add(new MailAddress(toEmail));
            
            _msg.IsBodyHtml = true;
            _msg.Body = model.Content;
            _msg.Body += GetUrl(model);
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