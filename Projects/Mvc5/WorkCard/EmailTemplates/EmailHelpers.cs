using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Net.Configuration;
using System.Net.Mail;
using System.Web.Configuration;

namespace iThinking.Common.Helpers
{
    public class EmailHelpers
    {
        public static bool IsValidEmail(string source)
        {
            return new EmailAddressAttribute().IsValid(source);
        }
    }

    public static class EmailSender
    {
        ///
        /// Sends an Email.
        ///
        public static bool Send(SmtpClient client, string sender, string senderName, string recipient, string recipientName, string subject, string body)
        {
            Configuration configurationFile = WebConfigurationManager.OpenWebConfiguration("~/web.config");
            MailSettingsSectionGroup mailSettings = configurationFile.GetSectionGroup("system.net/mailSettings") 
                as MailSettingsSectionGroup;
            string uid = mailSettings.Smtp.Network.UserName;
            string pwd = mailSettings.Smtp.Network.Password;

            var message = new MailMessage()
            {
                From = new MailAddress(sender, senderName),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };

            message.To.Add(new MailAddress(recipient, recipientName));

            try
            {
                client.Send(message);
            }
            catch (Exception ex)
            {
                //handle exeption
                Console.WriteLine(ex.ToString());
                return false;
            }

            return true;
        }

        ///
        /// Sends an Email asynchronously
        ///
        public static void SendAsync(string userName, string password, string sender, string senderName, string recipient, string recipientName, string subject, string body)
        {
            var message = new MailMessage()
            {
                From = new MailAddress(sender, senderName),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };
            message.To.Add(new MailAddress(recipient, recipientName));

            try
            {
                SmtpClient client = new SmtpClient();
                client.Credentials = new System.Net.NetworkCredential(userName, password);

                client.SendCompleted += MailDeliveryComplete;
                client.SendAsync(message, message);
            }
            catch (Exception ex)
            {
                string a = ex.Message;
            }
        }

        private static void MailDeliveryComplete(object sender, AsyncCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                string error = e.Error.Message;
                //handle error
            }
            else if (e.Cancelled)
            {
                //handle cancelled
            }
            else
            {
                //handle sent email
                MailMessage message = (MailMessage)e.UserState;
            }
        }
    }
}
