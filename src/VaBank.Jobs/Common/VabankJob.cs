using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Hangfire;
using Newtonsoft.Json;

namespace VaBank.Jobs.Common
{
    public static class VabankJob
    {
        private static readonly MethodInfo EnqueueWithParameterMethod;

        private static readonly MethodInfo ScheduleWithParameterMethod;

        static VabankJob()
        {
            Expression<Action> call = () => Enqueue<ParameterJob<DefaultJobContext<int>, int>, DefaultJobContext<int>, int>(1);
            var expressionBody = (MethodCallExpression)call.Body;
            EnqueueWithParameterMethod = expressionBody.Method.GetGenericMethodDefinition();

            Expression<Action> call2 = () => Schedule<ParameterJob<DefaultJobContext<int>, int>, DefaultJobContext<int>, int>(1, TimeSpan.FromSeconds(5));
            var expressionBody2 = (MethodCallExpression)call2.Body;
            ScheduleWithParameterMethod = expressionBody2.Method.GetGenericMethodDefinition();
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

        public static string Enqueue<TJob, T>(T argument)
            where TJob : ParameterJob<DefaultJobContext<T>, T>
        {
            var json = JsonConvert.SerializeObject(argument, Serialization.Settings);
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

        public static string Schedule<TJob, TContext, T>(T argument, TimeSpan delay)
            where TJob : ParameterJob<TContext, T>
            where TContext : class, IJobContext<T>
        {
            var json = JsonConvert.SerializeObject(argument, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All
            });
            return BackgroundJob.Schedule<TJob>(x => x.Execute(json, JobCancellationToken.Null), delay);
        }

        public static void AddOrUpdateRecurring<TJob, TJobContext>(string jobId, string cronExpression)
            where TJob : BaseJob<TJobContext>
            where TJobContext : class, IJobContext
        {
            jobId = string.IsNullOrEmpty(jobId) ? typeof (TJob).Name : jobId;
            RecurringJob.AddOrUpdate<TJob>(jobId, x => x.Execute(null, JobCancellationToken.Null), cronExpression);
        }

        internal static string Enqueue<T>(Type jobType, T argument)
        {
            var baseType = GetGenericBaseType(jobType, typeof (BaseJob<>));
            if (baseType == null)
            {
                throw new InvalidOperationException("No valid base type for a job found.");
            }
            var contextType = baseType.GetGenericArguments()[0];
            var contextBaseInterface =
                contextType.GetInterfaces().FirstOrDefault(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof (IJobContext<>));
            if (contextBaseInterface == null)
            {
                throw new InvalidOperationException("No valid base type for a job context.");
            }
            var argumentType = contextBaseInterface.GetGenericArguments()[0];
            var concreteMethod = EnqueueWithParameterMethod.MakeGenericMethod(jobType, contextType, argumentType);
            return (string)concreteMethod.Invoke(null, new object[] { argument });
        }

        internal static string Schedule<T>(Type jobType, T argument, TimeSpan delay)
        {
            var baseType = GetGenericBaseType(jobType, typeof(BaseJob<>));
            if (baseType == null)
            {
                throw new InvalidOperationException("No valid base type for a job found.");
            }
            var contextType = baseType.GetGenericArguments()[0];
            var contextBaseInterface =
                contextType.GetInterfaces().FirstOrDefault(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IJobContext<>));
            if (contextBaseInterface == null)
            {
                throw new InvalidOperationException("No valid base type for a job context.");
            }
            var argumentType = contextBaseInterface.GetGenericArguments()[0];
            var concreteMethod = ScheduleWithParameterMethod.MakeGenericMethod(jobType, contextType, argumentType);
            return (string)concreteMethod.Invoke(null, new object[] { argument, delay });
        }

        private static Type GetGenericBaseType(Type type, Type genericType)
        {
            var baseType = type.BaseType;
            while (baseType != null && baseType.GetGenericTypeDefinition() != genericType)
            {
                baseType = baseType.BaseType;
            }
            return baseType;
        }
    }
}
