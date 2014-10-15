using Hangfire;

namespace VaBank.Jobs.Common
{
    public interface ISimpleJob : IJob
    {
        void Execute(IJobCancellationToken cancellationToken);
    }
}
