using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalyzeTask.Tool
{
    /// <summary>
    /// 日志方法扩展
    /// </summary>
    public static class Serilogs
    {
        public static ILogger WriteInfomaion(this ILogger logger, string log)
        {
            logger.Information(log);
            return logger;
        }

        public static ILogger WriteInfomaion(this ILogger logger, Exception e, string log)
        {
            logger.Information(e, log);
            return logger;
        }

        public static ILogger WriteDebug(this ILogger logger, string log)
        {
            logger.Debug(log);
            return logger;
        }

        public static ILogger WriteDebug(this ILogger logger, Exception e, string log)
        {
            logger.Debug(e, log);
            return logger;
        }

        public static ILogger WriteError(this ILogger logger, string log)
        {
            logger.Error(log);
            return logger;
        }

        public static ILogger WriteError(this ILogger logger, Exception e, string log)
        {
            logger.Error(e, log);
            return logger;
        }

        public static ILogger WriteWarning(this ILogger logger, string log)
        {
            logger.Warning(log);
            return logger;
        }

        public static ILogger WriteWarning(this ILogger logger, Exception e, string log)
        {
            logger.Warning(log);
            return logger;
        }
    }
}
