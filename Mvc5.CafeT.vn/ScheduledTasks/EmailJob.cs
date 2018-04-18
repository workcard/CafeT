using Mvc5.CafeT.vn.Models;
using Quartz;
using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Mvc5.CafeT.vn.ScheduledTasks
{
    public class EmailJob : IJob
    {
        protected EmailService Service;
        
        public void Execute(IJobExecutionContext context)
        {
            using (var message = new MailMessage("taipm.vn@gmail.com", "taipm.vn@outlook.com"))
            {
                message.Subject = "Test";
                message.Body = "Test at " + DateTime.Now;
                using (SmtpClient client = new SmtpClient
                {
                    EnableSsl = true,
                    Host = "smtp.gmail.com",
                    Port = 587,
                    Credentials = new NetworkCredential("taipm.vn@gmail.com", "P@$$w0rd123PMT789")
                })
                {
                    client.Send(message);
                }
            }
        }

        public void DailySummaryEmail()
        {
            using (Service = new EmailService())
            {                
                DailySummary _summary = new DailySummary();
            }
        }

        Task IJob.Execute(IJobExecutionContext context)
        {
            throw new NotImplementedException();
        }
    }
}