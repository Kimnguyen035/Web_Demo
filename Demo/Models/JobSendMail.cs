using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Demo.Models
{
    public class JobSendMail
    {
        public static void Start()
        {
            IScheduler scheduler = StdSchedulerFactory.GetDefaultScheduler();
            scheduler.Start();
            IJobDetail job = JobBuilder.Create<EmailJob>().Build();

            //ITrigger trigger = TriggerBuilder.Create()
            //.WithDailyTimeIntervalSchedule
            //  (
            //    s => s.WithIntervalInHours(24)
            //    .OnEveryDay()
            //    .StartingDailyAt(TimeOfDay.HourAndMinuteOfDay(8, 49))
            //  )
            //.Build();
            //.WithIdentity("trigger7", "group1")
            //.WithSimpleSchedule(x => x
            //.WithIntervalInMinutes(1)
            //.RepeatForever())
            //.EndAt(DateBuilder.DateOf(8, 51, 0))
            //.Build();
            //scheduler.ScheduleJob(job, trigger);
        }
    }
}