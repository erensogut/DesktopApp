using System;
using Quartz;
using Quartz.Impl;

namespace DesktopApp.Cron
{
	public class Scheduler
	{
        public static async Task StartAsync(int interval, HttpClientCall httpClientCall)
        {
            //IScheduler scheduler = StdSchedulerFactory.GetDefaultScheduler();
            StdSchedulerFactory factory = new StdSchedulerFactory();
            IScheduler scheduler = await factory.GetScheduler();

            scheduler.Start();

            IJobDetail job = JobBuilder.Create<FileCheckJob>()
//                .UsingJobData("token", token)
                .Build();
            job.JobDataMap["client"] = httpClientCall;


            ITrigger trigger = TriggerBuilder.Create()
                .WithDailyTimeIntervalSchedule
                  (s =>
                     s
                    .OnEveryDay()
                    //.WithInterval(1,IntervalUnit.Minute) 
                    .WithInterval(interval,IntervalUnit.Second) 
                  )
                .Build();

            await scheduler.ScheduleJob(job, trigger);

        }
    }
}

