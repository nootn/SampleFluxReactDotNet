using System.Threading;
using Castle.DynamicProxy;
using DotNetAppStarterKit.Core.Logging;

namespace SampleFluxReactDotNet.Web.Application.Interceptors
{
    public class LoggerInterceptor : IInterceptor
    {
        private readonly ILogWithCallerInfo<LoggerInterceptor> _log;

        public LoggerInterceptor(ILogWithCallerInfo<LoggerInterceptor> log)
        {
            _log = log;
        }

        public void Intercept(IInvocation invocation)
        {
            var typeName = invocation.TargetType.Name;
            var methodName = invocation.Method.Name;
            var args = string.Join(", ", invocation.Arguments);
            var callInfo = string.Format("{0}.{1}({2})", typeName, methodName, args);
            _log.Debug("[ThreadId: {1}] Starting call: {0}", callInfo, Thread.CurrentThread.ManagedThreadId);
            try
            {
                invocation.Proceed();
            }
            finally
            {
                _log.Debug("[ThreadId: {1}] Finished call: {0}", callInfo, Thread.CurrentThread.ManagedThreadId);
            }
        }
    }
}