using AnalyzeTask.DataSource;
using AnalyzeTask.SchedulingTasks;
using Quartz;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AnalyzeTask.QuartzPack
{
    public class MyJobListener : JobListener
    {
        public ILogger log = new LoggerConfiguration().MinimumLevel.Debug().WriteTo.Console().CreateLogger();
        public override string Name => base.Name;
        /// <summary>
        /// 当任务执行前加载数据源
        /// </summary>
        /// <param name="context"></param>
        public int JobIndex = 0;
        public int JobCount = BaseJob.jobDetails.Count;
        private NeedSynData needSynData = new NeedSynData();
        public override void JobToBeExecuted(IJobExecutionContext context)
        {
            lock (this)
            {
                if (JobIndex == 0)
                {
                    log.Information("正在加载数据");
                    needSynData.GetNeedSynData();
                    log.Information("数据加载完成");
                }
                JobIndex++;
            }
        }

        public override void JobWasExecuted(IJobExecutionContext context, JobExecutionException jobException)
        {
            if (JobIndex >= JobCount)
            {
                JobIndex = 0;
            }
        }


    }
}
