using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnalyzeTask.Tool;
using AnalyzeTask.SchedulingTasks;
using System.Threading;

namespace AnalyzeTask.SchedulingTasks
{
    public class TestJob : BaseJob, IJob
    {
        public override void Execute(IJobExecutionContext context)
        {
            //此处使用try catch保护其他线程工作
            try
            {
                TaskName = "TestJob";
                Task task = RunServer(() =>
                {
                    Thread.Sleep(3000);
                    log.WriteInfomaion("测试完成");
                });
                task.Wait();
                log.WriteInfomaion("所有测试任务执行完成");
                base.NextTime(context);
            }
            catch (Exception ex)
            {
                log.WriteError(ex, "程序异常");
            }
        }

        public override Task RunServer(Action action)
        {
            Task task = Task.Run(action);
            return task;
        }

        public override IJobDetail CreateJobDetail(string TaskName, string TaskGroup)
        {
            IJobDetail simpleJob = JobBuilder.Create<TestJob>().WithIdentity(TaskName, TaskGroup).Build();
            return simpleJob;
        }

        public override ITrigger CreateITrigger(string TriggerName, string TriggerGroup)
        {
            cronTrigger = base.cronTrigger;
            return cronTrigger;
        }
    }
}
