using log4net;
using log4net.Core;
using System;
using System.Diagnostics;

namespace Test_Solution1.Common.Interceptor
{
    public static class EventStatus
    {
        public static string Started = "Started";
        public static string InProgress = "InProgress";
        public static string Completed = "Completed";
        public static string Failed = "Failed";
    }

    public static class LoggingExtensions
    {
        public static void Error(this ILog log, string processDetialId, string jobName, string message,
                                 Exception exception)
        {
            SetCustomPropertiesAndLog(log, processDetialId, jobName, EventStatus.Failed, exception, Level.Error, message);
        }

        public static void InProgressFormat(this ILog log, string processDetialId, string jobName, string message,
                                            params object[] args)
        {
            SetCustomPropertiesAndLog(log, processDetialId, jobName, EventStatus.InProgress, null, Level.Info, message,
                                      args);
        }

        public static void StartFormat(this ILog log, string processDetialId, string jobName, string message,
                                       params object[] args)
        {
            SetCustomPropertiesAndLog(log, processDetialId, jobName, EventStatus.Started, null, Level.Info, message,
                                      args);
        }

        public static void EndFormat(this ILog log, string processDetialId, string jobName, string message,
                                     params object[] args)
        {
            SetCustomPropertiesAndLog(log, processDetialId, jobName, EventStatus.Completed, null, Level.Info, message,
                                      args);
        }

        public static void Debug(this ILog log, string processDetialId, string jobName, string message)
        {
            SetCustomPropertiesAndLog(log, processDetialId, jobName, "", null, Level.Debug, message);
        }

        private static void SetCustomPropertiesAndLog(ILog log, string processDetialId, string jobName, string status,
                                                      Exception ex,
                                                      Level level, string message, params object[] args)
        {
            ThreadContext.Properties["ProcessDetailId"] = processDetialId;
            ThreadContext.Properties["JobName"] = jobName;
            ThreadContext.Properties["Status"] = status;
            Log(log, string.Format(message, args), level, ex);
        }

        private static void Log(ILog log, string message, Level level, Exception ex)
        {
            var stackframe = new StackFrame(1, false).GetMethod().DeclaringType;
            var loggingEvent = new LoggingEvent(stackframe,
                                                log.Logger.Repository,
                                                log.Logger.Name,
                                                level,
                                                message,
                                                ex);
            log.Logger.Log(loggingEvent);
        }
    }
}