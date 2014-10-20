using Autofac;
using VaBank.Jobs.Common;
using VaBank.Services.Contracts.Common;

namespace VaBank.Jobs.Maintenance
{
    public class AuditJob : EventListenerJob<DefaultJobContext<IAuditedEvent>, IAuditedEvent>
    {
        public AuditJob(ILifetimeScope scope) : base(scope)
        {
        }

        protected override void Execute(DefaultJobContext<IAuditedEvent> context)
        {
            Logger.Info(context.Data.Description);
        }
    }
}
