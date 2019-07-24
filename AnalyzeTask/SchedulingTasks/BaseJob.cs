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
using AnalyzeTask.QuartzPack;
using Quartz.Impl.Matchers;
using AnalyzeTask.Tool.Dapper;
using System.Data;

namespace AnalyzeTask.SchedulingTasks
{
    /// <summary>
    /// 插件化处理
    /// </summary>
    public abstract class BaseJob : IJob
    {
        private static IScheduler CurrentSched { get; set; }
        protected static ILogger log = new LoggerConfiguration().MinimumLevel.Debug().WriteTo.Console().WriteTo.File("logs\\Log.txt", rollingInterval: RollingInterval.Day).CreateLogger();
        protected ITrigger cronTrigger = TriggerBuilder.Create().WithIdentity(Guid.NewGuid().ToString(), JobKey.DefaultGroup.ToLower()).StartNow().WithCronSchedule(ConfigurationManager.AppSettings["Cron"]).Build();
        public static List<IJobDetail> jobDetails = new List<IJobDetail>();
        public static List<ITrigger> cronTriggers = new List<ITrigger>();
        protected DbContext dbContext = new DbContext();
        protected string TaskName { get; set; }
        public static void Initialize(List<BaseJob> baseJob)
        {
            log.WriteInfomaion("日志控件加载");
            CurrentSched = CurrentSched = StdSchedulerFactory.GetDefaultScheduler();
            for (int i = 0; i < baseJob.Count; i++)
            {
                jobDetails.Add(baseJob[i].CreateJobDetail(Guid.NewGuid().ToString(), JobKey.DefaultGroup.ToLower()));
                cronTriggers.Add(baseJob[i].CreateITrigger(Guid.NewGuid().ToString(), JobKey.DefaultGroup.ToLower()));
            }
            log.WriteInfomaion("初始化完成");
        }
        public virtual void Execute(IJobExecutionContext context)
        {
            log.WriteInfomaion($"Hello");
        }

        protected void NextTime(IJobExecutionContext context)
        {
            log.WriteInfomaion($"{TaskName}=>下次执行时间为：{context.NextFireTimeUtc}");
        }

        public abstract Task RunServer(Action action);

        public abstract IJobDetail CreateJobDetail(string TaskName, string TaskGroup);

        public abstract ITrigger CreateITrigger(string TriggerName, string TriggerGroup);

        public static void Run()
        {
            for (int i = 0; i < jobDetails.Count; i++)
            {
                CurrentSched.ScheduleJob(jobDetails[i], cronTriggers[i]);
            }
            GroupMatcher<JobKey> matcher = GroupMatcher<JobKey>.GroupEquals(JobKey.DefaultGroup.ToLower());
            JobListener jobListener = new MyJobListener();
            CurrentSched.ListenerManager.AddJobListener(jobListener, matcher);
            CurrentSched.Start();
        }
    }
}
