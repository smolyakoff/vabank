using Hangfire;

namespace VaBank.Jobs.Common
{
    public class DefaultJobContext<T> : IJobContext<T>
    {
        public IJobCancellationToken CancellationToken { get; set; }
        public T Data { get; set; }
    }
}
