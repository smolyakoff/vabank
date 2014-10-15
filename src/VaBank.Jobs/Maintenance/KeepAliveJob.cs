using System.Net;
using Hangfire;
using VaBank.Jobs.Common;

namespace VaBank.Jobs.Maintenance
{
    [JobName("KeepAlive")]
    public class KeepAliveJob : ISimpleJob
    {
        public void Execute(IJobCancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var client = new WebClient();
            client.DownloadData("https://vabank.azurewebsites.net/api/maintenance/keep-alive");
        }
    }
}
