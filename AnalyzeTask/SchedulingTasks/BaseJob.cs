using Quartz;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnalyzeTask.Tool;
using Quartz.Impl;
using System.Configuration;

namespace AnalyzeTask.SchedulingTasks
{
    /// <summary>
    /// 插件化处理
    /// </summary>
    public abstract class BaseJob : IJob
    {
        public static IScheduler CurrentSched { get; private set; }
        public static ILogger log = new LoggerConfiguration().MinimumLevel.Debug().WriteTo.Console().WriteTo.File("logs\\Log.txt", rollingInterval: RollingInterval.Day).CreateLogger();
        public ITrigger cronTrigger = TriggerBuilder.Create().WithIdentity(Guid.NewGuid().ToString(), "default").StartNow().WithCronSchedule(ConfigurationManager.AppSettings["Cron"]).Build();
        private static List<IJobDetail> jobDetails = new List<IJobDetail>();
        private static List<ITrigger> cronTriggers = new List<ITrigger>();
        public string TaskName { get; set; }
        public static void Initialize(List<BaseJob> baseJob)
        {
            log.WriteInfomaion("日志控件加载");
            CurrentSched = CurrentSched = StdSchedulerFactory.GetDefaultScheduler();
            for (int i = 0; i < baseJob.Count; i++)
            {
                jobDetails.Add(baseJob[i].CreateJobDetail(Guid.NewGuid().ToString(), "default"));
                cronTriggers.Add(baseJob[i].CreateITrigger(Guid.NewGuid().ToString(), "default"));
            }
            log.WriteInfomaion("初始化完成");
        }
        public virtual void Execute(IJobExecutionContext context)
        {
            log.WriteInfomaion($"Hello");
        }

        public void NextTime(IJobExecutionContext context)
        {
            log.WriteInfomaion($"{TaskName}下次执行时间为：{context.NextFireTimeUtc}");
        }

        public abstract Task RunServer(Action action);

        public abstract IJobDetail CreateJobDetail(string TaskName, string TaskGroup);

        public abstract ITrigger CreateITrigger(string TriggerName, string TriggerGroup);

        public static void AllRun()
        {
            for (int i = 0; i < jobDetails.Count; i++)
            {
                CurrentSched.ScheduleJob(jobDetails[i], cronTriggers[i]);
            }
            CurrentSched.Start();
        }
    }
}
