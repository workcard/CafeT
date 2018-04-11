using SmartTracking.Models.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace SmartTracking.Helpers
{
    public class EmailHelpers
    {
        private const string HtmlEmailHeader = "<html><head><title></title></head><body style='font-family:arial; font-size:14px;'>";
        private const string HtmlEmailFooter = "</body></html>";

        public void SendAsyncMail()
        {
            MailMessage mail = new MailMessage();

            mail.From = new MailAddress("Enter from mail address");
            mail.To.Add(new MailAddress("Enter to address #1"));
            mail.To.Add(new MailAddress("Enter to address #2"));
            mail.Subject = "Enter mail subject";
            mail.Body = "Enter mail body";

            SmtpClient smtpClient = new SmtpClient();
            Object state = mail;

            //event handler for asynchronous call
            smtpClient.SendCompleted += new SendCompletedEventHandler(smtpClient_SendCompleted);
            try
            {
                smtpClient.SendAsync(mail, state);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        void smtpClient_SendCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            MailMessage mail = e.UserState as MailMessage;

            if (!e.Cancelled && e.Error != null)
            {
                //message.Text = "Mail sent successfully";
            }
        }

        public static bool Send(Email email)
        {
            MailMessage message = new MailMessage();

            foreach (var x in email.To)
            {
                message.To.Add(x);
            }
            foreach (var x in email.CC)
            {
                message.CC.Add(x);
            }
            foreach (var x in email.BCC)
            {
                message.Bcc.Add(x);
            }

            message.Subject = email.Subject;
            message.Body = string.Concat(HtmlEmailHeader, email.Body, HtmlEmailFooter);
            message.BodyEncoding = System.Text.Encoding.UTF8;
            message.From = new MailAddress(email.From);
            message.SubjectEncoding = System.Text.Encoding.UTF8;
            message.IsBodyHtml = true;

            SmtpClient client = new SmtpClient();
            client.Host = email.ServerName;
            client.Port = email.Port;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.EnableSsl = Convert.ToBoolean(email.EnableSsl);
            client.UseDefaultCredentials = false;
            client.Timeout = 20000;

            NetworkCredential smtpUserInfo = new NetworkCredential(email.UserName, email.Password);
            client.Credentials = smtpUserInfo;

            try
            {
                client.Send(message);
                client.SendMailAsync(message);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }
    }
}
