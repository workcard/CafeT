using Quartz;
using Quartz.Impl;
using System.Collections.Specialized;
using System.Threading.Tasks;

namespace Web.ScheduledTasks
{
    public class JobScheduler
    {
        public static async Task StartAsync()
        {
            // construct a scheduler factory
            //NameValueCollection props = new NameValueCollection
            //{
            //    {
            //        "quartz.serializer.type",
            //        "binary"
            //    }
            //};

            StdSchedulerFactory factory = new StdSchedulerFactory(/*props*/);
            // get a scheduler
            IScheduler sched = await factory.GetScheduler();
            await sched.Start();
            
            // define the job and tie it to our HelloJob class
            IJobDetail job = JobBuilder.Create<EmailJob>()
                //.WithIdentity("myJob", "group1")
                .Build();

            // Trigger the job to run now, and then every 40 seconds
            ITrigger trigger = TriggerBuilder.Create()
              //.WithIdentity("myTrigger", "group1")
              .StartNow()
              .WithSimpleSchedule(x => x
                  .WithIntervalInMinutes(45)
                  .RepeatForever())
              .Build();

            await sched.ScheduleJob(job, trigger);
        }
    }
}