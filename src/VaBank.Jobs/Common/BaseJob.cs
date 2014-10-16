using System;
using System.Linq;
using Autofac;
using Common.Logging;
using Hangfire;
using Newtonsoft.Json;

namespace VaBank.Jobs.Common
{
    public abstract class BaseJob<TContext> : IJob
        where TContext : class, IJobContext

    {
        protected readonly ILifetimeScope RootScope;

        protected readonly ILog Logger;

        protected BaseJob(ILifetimeScope scope)
        {
            if (scope == null)
            {
                throw new ArgumentNullException("scope");
            }
            RootScope = scope;
            Logger = LogManager.GetLogger(GetType().FullName);
        }

        public virtual void Execute(string json, IJobCancellationToken cancellationToken)
        {
            using (var scope = RootScope.BeginLifetimeScope())
            {
                var data = JsonConvert.DeserializeObject(json, new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.All
                });
                var context = CreateContext(scope, data);
                context.CancellationToken = cancellationToken;
                try
                {
                    Execute(context);
                }
                catch (OperationCanceledException ex)
                {
                    var message = string.Format("Job with name [{0}] was cancelled.", JobName);
                    Logger.Warn(message, ex);
                    throw;
                }
                catch (Exception ex)
                {
                    var message = string.Format("Job with name [{0}] was stopped due to exception.", JobName);
                    Logger.Error(message, ex);
                    throw;
                }
            }
        }

        protected abstract void Execute(TContext context);

        protected virtual TContext CreateContext(ILifetimeScope scope, object data)
        {
            var context = scope.Resolve<TContext>();
            if (context == null)
            {
                var message = string.Format("Job's [{0}] context resolved to null.", JobName);
                throw new InvalidOperationException(message);
            }
            return context;
        }

        protected string JobName
        {
            get { return GetJobName(); }
        }

        private string GetJobName()
        {
            var jobType = GetType();
            if (!jobType.IsDefined(typeof (JobNameAttribute), false))
            {
                return jobType.Name;
            }
            var nameAttribute =
                jobType.GetCustomAttributes(typeof (JobNameAttribute), false).FirstOrDefault() as JobNameAttribute;
            if (nameAttribute == null)
            {
                throw new InvalidOperationException("Job name attribute is null");
            }
            return nameAttribute.Name;
        }
    }
}
