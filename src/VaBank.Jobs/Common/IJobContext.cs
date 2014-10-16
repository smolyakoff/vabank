using Hangfire;

namespace VaBank.Jobs.Common
{
    public interface IJobContext
    {
        IJobCancellationToken CancellationToken { get; set; }
    }
}
