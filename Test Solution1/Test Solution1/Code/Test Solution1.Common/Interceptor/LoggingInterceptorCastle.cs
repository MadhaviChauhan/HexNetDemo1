using Castle.DynamicProxy;
using Test_Solution1.Common.CustomException;
using log4net;
using log4net.Core;
using System;
using System.Diagnostics;
using System.Linq;

namespace Test_Solution1.Common.Interceptor
{
    public class LoggingInterceptorCastle : IInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
            var log = LogManager.GetLogger(invocation.TargetType.FullName);
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            //IsDEbugEnabled will boost performance, since buildmessage will not be called

            if (log.IsDebugEnabled)
            {
                log.Debug(BuildLogMessage("Start", invocation, 0000));
            }
            try
            {
                //Set the application name of the EventLogAppender

                //string vav= ((log4net.Appender.EventLogAppender)log.Logger.Repository.GetAppenders().Single(a => a.Name == "EventLogAppender")).ApplicationName;




                invocation.Proceed();
            }
            catch (RulesViolationException rx)
            {
                throw;
            }
            catch (Exception ex)
            {
                //use of AbstractInvocation (castle), because 
                //log4net will navigate one level up stackframe to capture class name and method name
                log.Logger.Log(typeof(AbstractInvocation), Level.Error,
                               String.Format(
                                   "Error occurred in class {0} while executing function {1}. Arguments : {2}",
                                   invocation.TargetType.Name,
                                   invocation.Method.Name,
                                   GetArguments(invocation)), ex);
                throw;
            }
            stopwatch.Stop();
            if (log.IsDebugEnabled) log.Debug(BuildLogMessage("End", invocation, stopwatch.ElapsedMilliseconds));
        }

        private static string GetArguments(IInvocation invocation)
        {
            var msg = string.Empty;
            if (invocation.Arguments != null)
                msg = string.Join(", ", invocation.Arguments.Select(x => (x ?? String.Empty).ToString()));
            return msg;
        }

        private static string BuildLogMessage(string message, IInvocation invocation, long timerinmilisecond)
        {
            var msg = GetArguments(invocation);

            return string.Format("{0}:timer-[{4}]  {1}.{2}({3})",
                                 message,
                                 invocation.TargetType.Name,
                                 invocation.Method.Name,
                                 msg, timerinmilisecond);
        }
    }
}