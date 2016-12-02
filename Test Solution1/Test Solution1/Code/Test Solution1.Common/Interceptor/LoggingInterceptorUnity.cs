
using log4net;
using log4net.Core;
using Microsoft.Practices.Unity.InterceptionExtension;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Test_Solution1.Common.Interceptor
{
    public class LoggingInterceptorUnity : IInterceptionBehavior
    {

        /// <summary>
        /// Returns the interfaces required by the behavior for the objects it intercepts.
        /// </summary>
        /// <returns>
        /// The required interfaces.
        /// </returns>
        public IEnumerable<Type> GetRequiredInterfaces()
        {
            return Type.EmptyTypes;
        }

        /// <summary>
        /// Implement this method to execute your behavior processing.
        /// </summary>
        /// <param name="input">Inputs to the current call to the target.</param>
        /// <param name="getNext">Delegate to execute to get the next delegate 
        /// in the behavior chain</param>
        /// <returns>
        /// Return value from the target.
        public IMethodReturn Invoke(IMethodInvocation input, GetNextInterceptionBehaviorDelegate getNext)
        {
            var log = LogManager.GetLogger(input.Target.ToString());
            IMethodReturn msg;
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            if (log.IsDebugEnabled) log.Debug(BuildLogMessage("Start", input, 0000));

            try
            {
                msg = getNext()(input, getNext);
                if (msg.Exception != null)
                {
                    log.Logger.Log(typeof(IInterceptionBehavior), Level.Error,
                              String.Format(
                                  "Error occurred in class {0} while executing function {1}. Arguments : {2}",
                                 input.Target.ToString(),
                                   input.ToString(),
                                  GetArguments(input)), msg.Exception);
                }
            }

            catch (Exception ex)
            {
                //use of AbstractInvocation (castle), because 
                //log4net will navigate one level up stackframe to capture class name and method name
                log.Logger.Log(typeof(IInterceptionBehavior), Level.Error,
                               String.Format(
                                   "Error occurred in class {0} while executing function {1}. Arguments : {2}",
                                  input.Target.ToString(),
                                    input.ToString(),
                                   GetArguments(input)), ex);
                throw;
            }
            stopwatch.Stop();
            if (log.IsDebugEnabled) log.Debug(BuildLogMessage("End", input, stopwatch.ElapsedMilliseconds));

            return msg;
        }

        /// <summary>
        /// Optimization hint for proxy generation - will this behavior actually
        /// perform any operations when invoked?
        /// </summary>
        public bool WillExecute
        {
            get { return true; }
        }
        private static string BuildLogMessage(string message, IMethodInvocation input, long timerinmilisecond)
        {
            var msg = GetArguments(input);

            return string.Format("{0} :timer-[{4}]  {1}.{2}({3})",
                                 message,
                                 input.Target.ToString(),
                                 input.MethodBase.Name,
                                 msg, timerinmilisecond);
        }

        private static string GetArguments(IMethodInvocation input)
        {
            var msg = string.Empty;
            if (input.Arguments != null)
            {
                for (int i = 0; i < input.Arguments.Count; i++)
                {
                    msg += (msg == string.Empty) ? input.Arguments[i].ToString() : ", " + input.Arguments[i].ToString();
                }

            }

            return msg;
        }
    }
}