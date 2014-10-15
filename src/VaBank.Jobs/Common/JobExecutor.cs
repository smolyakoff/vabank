using System;
using System.Linq;
using Autofac;
using Common.Logging;
using Hangfire;

namespace VaBank.Jobs.Common
{
    public class JobExecutor
    {
        private readonly ILifetimeScope _lifetimeScope;

        public JobExecutor(ILifetimeScope lifetimeScope)
        {
            if (lifetimeScope == null)
            {
                throw new ArgumentNullException("lifetimeScope");
            }
            _lifetimeScope = lifetimeScope;
        }

        public void Execute(Type jobType, IJobCancellationToken cancellationToken)
        {
            using (var scope = _lifetimeScope.BeginLifetimeScope())
            {
                var job = scope.Resolve(jobType) as ISimpleJob;
                var jobName = GetJobName(jobType);
                if (job == null)
                {
                    var message = string.Format("Job {0} is not registered in container.", jobName);
                    throw new InvalidOperationException(message);
                }
                var logger = LogManager.GetLogger(job.GetType());
                try
                {
                     job.Execute(cancellationToken);
                }
                catch (OperationCanceledException ex)
                {
                    var message = string.Format("Job with name [{0}] was cancelled.", jobName);
                    logger.Warn(message, ex);
                    throw;
                }
                catch (Exception ex)
                {
                    var message = string.Format("Job with name [{0}] was stopped due to exception.", jobName);
                    logger.Error(message, ex);
                    throw;
                }              
            }
        }

        public void Execute<TJob>(IJobCancellationToken cancellationToken) where TJob : class, ISimpleJob
        {
            using (var scope = _lifetimeScope.BeginLifetimeScope())
            {
                var job = scope.Resolve<TJob>();
                var jobName = GetJobName(typeof (TJob));
                if (job == null)
                {
                    var message = string.Format("Job {0} is not registered in container.", jobName);
                    throw new InvalidOperationException(message);
                }
                var logger = LogManager.GetLogger(job.GetType());
                try
                {
                     job.Execute(cancellationToken);
                }
                catch (OperationCanceledException ex)
                {
                    var message = string.Format("Job with name [{0}] was cancelled.", jobName);
                    logger.Warn(message, ex);
                    throw;
                }
                catch (Exception ex)
                {
                    var message = string.Format("Job with name [{0}] was stopped due to exception.", jobName);
                    logger.Error(message, ex);
                    throw;
                }              
            }
        }

        public void Execute<TJob, T>(T argument, IJobCancellationToken cancellationToken) where TJob : class, IJob<T>
        {
            using (var scope = _lifetimeScope.BeginLifetimeScope())
            {
                var job = scope.Resolve<TJob>();
                var jobName = GetJobName(typeof(TJob));
                if (job == null)
                {
                    var message = string.Format("Job {0} is not registered in container.", jobName);
                    throw new InvalidOperationException(message);
                }
                var logger = LogManager.GetLogger(job.GetType());
                try
                {
                    job.Execute(argument, cancellationToken);
                }
                catch (OperationCanceledException ex)
                {
                    var message = string.Format("Job with name [{0}] was cancelled.", jobName);
                    logger.Warn(message, ex);
                    throw;
                }
                catch (Exception ex)
                {
                    var message = string.Format("Job with name [{0}] was stopped due to exception.", jobName);
                    logger.Error(message, ex);
                    throw;
                }    
            }
        }

        private static string GetJobName(Type jobType)
        {
            if (!jobType.IsDefined(typeof(JobNameAttribute), false))
            {
                return jobType.Name;
            }
            var nameAttribute = jobType.GetCustomAttributes(typeof(JobNameAttribute), false).FirstOrDefault() as JobNameAttribute;
            if (nameAttribute == null)
            {
                throw new InvalidOperationException("Job name attribute is null");
            }
            return nameAttribute.Name;
        }
    }
}
