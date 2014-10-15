using Hangfire;

namespace VaBank.Jobs.Common
{
    public interface IJob<in T> : IJob
    {
        void Execute(T argument, IJobCancellationToken token);
    }
}
