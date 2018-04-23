using System;
using System.ComponentModel;
using System.Net.Mail;

namespace iThinking.EmailHelper
{
    public class EmailSender
    {
        ///
        /// Sends an Email.
        ///
        public static bool Send(string sender, string senderName, string recipient, string recipientName, string subject, string body)
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
                var client = new SmtpClient();
                client.Send(message);
            }
            catch (Exception ex)
            {
                //handle exeption
                return false;
            }

            return true;
        }

        ///
        /// Sends an Email asynchronously
        ///
        public static void SendAsync(string sender, string senderName, string recipient, string recipientName, string subject, string body)
        {
            var message = new MailMessage()
            {
                From = new MailAddress(sender, senderName),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };
            message.To.Add(new MailAddress(recipient, recipientName));

            var client = new SmtpClient();
            client.SendCompleted += MailDeliveryComplete;
            client.SendAsync(message, message);
        }

        private static void MailDeliveryComplete(object sender, AsyncCompletedEventArgs e)
        {
            if (e.Error != null)
            {
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