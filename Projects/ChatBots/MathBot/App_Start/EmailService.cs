using CafeT.Text;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using CafeT.Html;
using FluentEmail.Markdown;
using FluentEmail;
using CafeT.Objects;

namespace CafeT.Emails
{
    public class OutlookEmailService: IDisposable
    {
        public string ToEmail { set; get; }
        public void Dispose()
        {
            this.Dispose();
        }
        public void SendAsMarkDown(string message)
        {
            Email.DefaultRenderer = new MarkdownRenderer();

            // Use FluentEmail without the need to call UsingTemplateEngine() each time
            try
            {
                var email = Email
                    .From("taipm.vn@outlook.com")
                    .BodyAsHtml()
                    .Subject("MathBot - Your email")
                    .UsingTemplateEngine(new MarkdownRenderer())
                    ;

                SmtpClient client = new SmtpClient
                {
                    EnableSsl = true,
                    Host = "smtp.live.com",
                    Port = 587,
                    Credentials = new NetworkCredential("taipm.vn@outlook.com", "P@$$w0rdPMT789")
                };

                var _emails = message.GetEmails().AsEnumerable();
                if (_emails != null && _emails.Count() > 0)
                {
                    _emails = _emails.Distinct();
                    foreach (var _email in _emails)
                    {
                        //email.UsingTemplateFromFile(@"~/EmailTemplates/CreateIssueEmail.md",
                        //                        new
                        //                        {
                        //                            Id = model.Id,
                        //                            Name = _email,
                        //                            Title = model.Title,
                        //                            Content = model.Content,
                        //                            Start = model.Start,
                        //                            End = model.End,
                        //                            CreatedBy = model.CreatedBy,
                        //                            CreatedDate = model.CreatedDate
                        //                        });
                        email.Body(message);
                        email.BodyAsHtml();
                        email.To(_email);
                        email.UsingClient(client);
                        email.Send();
                    }
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

        //public Task SendAsync(IdentityMessage message)
        //{
        //    MailMessage _msg = new MailMessage("taipm.vn@outlook.com", ToEmail);
        //    _msg.Subject = message.Subject;
        //    _msg.To.Add(new MailAddress(message.Destination));
        //    _msg.Body = message.Body;
        //    _msg.IsBodyHtml = true;

        //    using (SmtpClient client = new SmtpClient
        //    {
        //        EnableSsl = true,
        //        Host = " smtp.live.com",
        //        Port = 587,
        //        Credentials = new NetworkCredential("taipm.vn@outlook.com", "P@$$w0rdPMT789")
        //    })
        //    {
        //        client.Send(_msg);
        //    }
        //    return Task.FromResult(0);
        //}

        //public Task SendAsync(WorkIssue model)
        //{
        //    IdentityMessage _msg = new IdentityMessage();
        //    if(!model.Title.IsNullOrEmptyOrWhiteSpace())
        //    _msg.Subject = model.Title.RemoveHtml();
        //    _msg.Body = model.Content;
        //    _msg.Destination = ToEmail;
        //    return this.SendAsync(_msg);
        //}
    }

    //public class EmailService : IIdentityMessageService, IDisposable
    //{
    //    public Task SendAsync(IdentityMessage message)
    //    {
    //        MailMessage _msg = new MailMessage("taipm.vn@gmail.com", "taipm.vn@outlook.com");
    //        _msg.Subject = message.Subject;
    //        _msg.To.Add(new MailAddress(message.Destination));
    //        _msg.Body = message.Body;
    //        _msg.IsBodyHtml = true;
            
    //        using (SmtpClient client = new SmtpClient
    //        {
    //            EnableSsl = true,
    //            Host = "smtp.gmail.com",
    //            Port = 587,
    //            Credentials = new NetworkCredential("taipm.vn@gmail.com", "P@$$w0rd789PMT123")
    //        })
    //        {
    //            client.Send(_msg);
    //        }
    //        return Task.FromResult(0);
    //    }

    //    public Task SendAsync(WorkIssue model)
    //    {
    //        MailMessage _msg = new MailMessage("taipm.vn@gmail.com", "taipm.vn@outlook.com");
    //        _msg.Subject = "[Issue] " + model.Title.ToStandard();
    //        if (model.CreatedBy.IsEmail())
    //        {
    //            _msg.To.Add(new MailAddress(model.CreatedBy));
    //            _msg.Body = model.Description;
    //            _msg.IsBodyHtml = true;

    //            using (SmtpClient client = new SmtpClient
    //            {
    //                EnableSsl = true,
    //                Host = "smtp.gmail.com",
    //                Port = 587,
    //                Credentials = new NetworkCredential("taipm.vn@gmail.com", "P@$$w0rd789PMT123")
    //            })
    //            {
    //                client.Send(_msg);
    //            }
    //        }
    //        return Task.FromResult(0);
    //    }

    //    public void Dispose()
    //    {
    //        this.Dispose();
    //    }
    //}
}