using AnalyzeTask.Tool;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AnalyzeTask.SchedulingTasks
{
    public class TestJob2 : BaseJob, IJob
    {
        public override void Execute(IJobExecutionContext context)
        {
            try
            {
                TaskName = "TestJob2";
                Task task = RunServer(() => {
                    Thread.Sleep(1000);
                    log.WriteInfomaion("测试完成2");
                });
                task.Wait();
                log.WriteInfomaion("所有测试任务执行完成2");
                base.NextTime(context);
            }
            catch (Exception ex)
            {
                log.WriteError(ex, "程序异常");
            }
        }

        public override Task RunServer(Action action)
        {
            throw new Exception();
            Task task = Task.Run(action);
            return task;
        }

        /// <summary>
        /// 注册方法
        /// </summary>
        /// <param name="TaskName"></param>
        /// <param name="TaskGroup"></param>
        /// <returns></returns>
        public override IJobDetail CreateJobDetail(string TaskName, string TaskGroup)
        {
            IJobDetail simpleJob = JobBuilder.Create<TestJob2>().WithIdentity(TaskName, TaskGroup).Build();
            return simpleJob;
        }

        /// <summary>
        /// 每个插件在此自定义触发器
        /// </summary>
        /// <param name="TriggerName"></param>
        /// <param name="TriggerGroup"></param>
        /// <returns></returns>
        public override ITrigger CreateITrigger(string TriggerName, string TriggerGroup)
        {
            cronTrigger = base.cronTrigger;
            return cronTrigger;
        }
    }
}
