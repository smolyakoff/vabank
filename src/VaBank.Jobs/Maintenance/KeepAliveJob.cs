using System.Net;
using Autofac;
using VaBank.Jobs.Common;

namespace VaBank.Jobs.Maintenance
{
    [JobName("KeepAlive")]
    public class KeepAliveJob : BaseJob<DefaultJobContext>
    {
        public KeepAliveJob(ILifetimeScope scope) : base(scope)
        {
        }

        protected override void Execute(DefaultJobContext context)
        {
            var client = new WebClient();
            client.DownloadData("https://vabank.azurewebsites.net/api/maintenance/keep-alive");
        }
    }
}
