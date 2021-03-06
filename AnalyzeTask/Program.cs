﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnalyzeTask.SchedulingTasks;
using AnalyzeTask.Tool;
using Quartz;
using Quartz.Impl;
using Serilog;

namespace AnalyzeTask
{
    class Program
    {

        static void Main(string[] args)
        {
            BaseJob.Initialize(new List<BaseJob> {
                new TestJob(),
                new TestJob2()
            });
            BaseJob.Run();
            Console.ReadKey();
        }
    }
}
