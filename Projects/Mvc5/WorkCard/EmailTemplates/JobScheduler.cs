using Quartz;
using Quartz.Impl;
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
            
            IJobDetail job = JobBuilder.Create<EmailJob>()
                .Build();

            ITrigger trigger = TriggerBuilder.Create()
                .WithDailyTimeIntervalSchedule
                 (s =>
                    s.WithIntervalInMinutes(60)
                   .OnEveryDay()
                   //.WithRepeatCount(8)
                   .StartingDailyAt(TimeOfDay.HourAndMinuteOfDay(7, 0))
                   .EndingDailyAt(TimeOfDay.HourAndMinuteOfDay(13, 0))
                 )
                 .Build();

              //.StartAt(DateBuilder.DateOf(7,0,0))
              //.WithSimpleSchedule(x => x
              //    .WithIntervalInMinutes(60)
              //    .RepeatForever())
              //.EndAt(DateBuilder.DateOf(18,0,0))
              //.Build();

            await sched.ScheduleJob(job, trigger);
        }
    }
}