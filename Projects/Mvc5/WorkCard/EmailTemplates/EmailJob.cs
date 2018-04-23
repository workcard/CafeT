
using CafeT.Enumerable;
using CafeT.Objects.Enums;
using Quartz;
using Repository.Pattern.Ef6;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Net.Configuration;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Configuration;
using Web;
using Web.Managers;
using Web.Models;

namespace Web.ScheduledTasks
{
    public class EmailJob : IJob
    {
        Task IJob.Execute(IJobExecutionContext context)
        {
            EmailService service = new EmailService();

            using (ApplicationDbContext dbContext = new ApplicationDbContext())
            {

                try
                {
                    var issues = dbContext.Issues
                    .Where(t => (t.End.HasValue)
                                &&  (t.End.Value.Day == DateTime.Now.Day)
                                 && (t.End.Value.Month == DateTime.Now.Month)
                                  && (t.End.Value.Year == DateTime.Now.Year)
                                && (t.Status != IssueStatus.Done))
                    .ToList();
                    foreach (var issue in issues)
                    {
                        issue.Notify(service);
                        //service.SendAsync(issue, issue.CreatedBy);
                    }
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            return Task.CompletedTask;
        }
    }
}