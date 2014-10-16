using System;
using System.Linq.Expressions;
using System.Reflection;
using Hangfire;
using Newtonsoft.Json;

namespace VaBank.Jobs.Common
{
    public static class VabankJob
    {
        private static readonly MethodInfo EnqueueWithParameterMethod;

        static VabankJob()
        {
            Expression<Action> call = () => Enqueue<ParameterJob<DefaultJobContext<int>, int>, DefaultJobContext<int>, int>(1);
            var expressionBody = (MethodCallExpression)call.Body;
            EnqueueWithParameterMethod = expressionBody.Method.GetGenericMethodDefinition();
        }

        public static string Enqueue<TJob>()
            where TJob : BaseJob<DefaultJobContext>
        {
            return BackgroundJob.Enqueue<TJob>(x => x.Execute(null, JobCancellationToken.Null));
        }

        public static string Enqueue<TJob, TContext>()
            where TJob : BaseJob<TContext> 
            where TContext : class, IJobContext
        {
            return BackgroundJob.Enqueue<TJob>(x => x.Execute(null, JobCancellationToken.Null));
        }

        internal static string Enqueue<T>(Type jobType, T argument)
        {
            var baseType = jobType.BaseType;
            while (baseType != null && !baseType.IsGenericType && baseType.GetGenericTypeDefinition() != typeof(BaseJob<>))
            {
                baseType = baseType.BaseType;
            }
            if (baseType == null)
            {
                throw new InvalidOperationException("No valid base type for a job found.");
            }
            var contextType = baseType.GetGenericArguments()[0];
            var concreteMethod = EnqueueWithParameterMethod.MakeGenericMethod(jobType, contextType, argument.GetType());
            return (string) concreteMethod.Invoke(null, new object[]{argument});
        }

        public static string Enqueue<TJob, T>(T argument)
            where TJob : ParameterJob<DefaultJobContext<T>, T>
        {
            var json = JsonConvert.SerializeObject(argument, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All
            });
            return BackgroundJob.Enqueue<TJob>(x => x.Execute(json, JobCancellationToken.Null));
        }

        public static string Enqueue<TJob, TContext, T>(T argument)
            where TJob : ParameterJob<TContext, T>
            where TContext : class, IJobContext<T>
        {
            var json = JsonConvert.SerializeObject(argument, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All
            });
            return BackgroundJob.Enqueue<TJob>(x => x.Execute(json, JobCancellationToken.Null));
        }

        public static void AddOrUpdateRecurring<TJob>(string jobId, string cronExpression)
            where TJob : BaseJob<DefaultJobContext>
        {
            jobId = string.IsNullOrEmpty(jobId) ? typeof (TJob).Name : jobId;
            RecurringJob.AddOrUpdate<TJob>(jobId, x => x.Execute(null, JobCancellationToken.Null), cronExpression);
        } 
    }
}
