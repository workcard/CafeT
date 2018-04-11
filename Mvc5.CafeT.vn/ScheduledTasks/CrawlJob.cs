using Mvc5.CafeT.vn.Models;
using Quartz;
using System;
using System.Linq;

namespace Mvc5.CafeT.vn.ScheduledTasks
{
    public class CrawlJob : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            string _crawlerInput = "/App_Data/Crawlers/Input/";
            string _crawlerOutput = "/App_Data/Crawlers/Output/";
            string _appPath = AppDomain.CurrentDomain.BaseDirectory;

            string _fullInput = _appPath + _crawlerInput; 
            string _fullOutput = _appPath + _crawlerOutput; 

            CrawlerManager _crawlerManager = new CrawlerManager(_fullInput, _fullOutput);
            _crawlerManager.Run();
        }

        public void ExcuteWithDb(IJobExecutionContext context)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var _crawlers = db.Crawlers.Where(t => t.Enable == true).AsEnumerable();
            }
        }
    }
}