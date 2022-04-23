using System;
using Quartz;

namespace DesktopApp.Cron
{

    public class FileCheckJob : IJob
    {

        public async Task Execute(IJobExecutionContext context)
        {

            JobDataMap dataMap = context.JobDetail.JobDataMap;
            var client = (HttpClientCall)dataMap["client"];
            if (client.lastHash == 0)
            {
                client.DownloadFile();
                return;
            }
            else if (await client.IsNewExist())
            {
                client.DownloadFile();
                return;
            }
            await Console.Out.WriteLineAsync("Timer job is executed succesfully.");
        }

    }
}

