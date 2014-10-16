using Hangfire;

namespace VaBank.Jobs.Common
{
    public interface IJobContext
    {
        IJobCancellationToken CancellationToken { get; set; }

        void Set(string key, object value);

        object Get(string key);
    }
}
