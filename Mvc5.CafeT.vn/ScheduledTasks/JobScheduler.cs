﻿using Mvc5.CafeT.vn.Models;
using Quartz;
using Quartz.Impl;
using System.Linq;

namespace Mvc5.CafeT.vn.ScheduledTasks
{
    public static class JobScheduler
    {
        public static void Start()
        {
            //IScheduler scheduler =  StdSchedulerFactory.GetDefaultScheduler();
            //scheduler.Start();

            //IJobDetail job = JobBuilder.Create<EmailJob>().Build();
            ////IJobDetail job = JobBuilder.Create<CrawlJob>().Build();
            //ITrigger trigger = TriggerBuilder.Create()
            //    .WithDailyTimeIntervalSchedule
            //      (s =>
            //         s.WithIntervalInHours(12)
            //        .OnEveryDay()
            //        .StartingDailyAt(TimeOfDay.HourAndMinuteOfDay(0, 0))
            //      )
            //    .Build();

            //scheduler.ScheduleJob(job, trigger);
        }
    }
}