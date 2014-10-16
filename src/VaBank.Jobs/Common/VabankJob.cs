using Hangfire;

namespace VaBank.Jobs.Common
{
    public static class VabankJob
    {
        public static string Enqueue<TJob>()
            where TJob : BaseJob<DefaultJobContext>
        {
            return BackgroundJob.Enqueue<TJob>(x => x.Execute(null, JobCancellationToken.Null));
        }

        public static string Enqueue<TJob, TContext>()
            where TJob : BaseJob<TContext> where TContext : class, IJobContext
        {
            return BackgroundJob.Enqueue<TJob>(x => x.Execute(null, JobCancellationToken.Null));
        }

        public static string Enqueue<TJob, TContext, T>(T argument)
            where TJob : BaseJob<TContext>
            where TContext : class, IJobContext<T>
        {
            return BackgroundJob.Enqueue<TJob>(x => x.Execute(argument, JobCancellationToken.Null));
        }

        public static void AddOrUpdateRecurring<TJob>(string jobId, string cronExpression)
            where TJob : BaseJob<DefaultJobContext>
        {
            jobId = string.IsNullOrEmpty(jobId) ? typeof (TJob).Name : jobId;
            RecurringJob.AddOrUpdate<TJob>(jobId, x => x.Execute(null, JobCancellationToken.Null), cronExpression);
        } 
    }
}
