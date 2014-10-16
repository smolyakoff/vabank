using Hangfire;

namespace VaBank.Jobs.Common
{
    public class DefaultJobContext : IJobContext
    {
        public IJobCancellationToken CancellationToken { get; set; }
    }
}
