using Autofac;
using AutoMapper;
using VaBank.Jobs.Common;
using VaBank.Services.Contracts.Common;
using VaBank.Services.Contracts.Maintenance.Commands;

namespace VaBank.Jobs.Maintenance
{
    public class AuditJob : EventListenerJob<DefaultJobContext<IAuditedEvent>, IAuditedEvent>
    {
        public AuditJob(ILifetimeScope scope) : base(scope)
        {
        }

        protected override void Execute(DefaultJobContext<IAuditedEvent> context)
        {
            context.LogManagementService.LogApplicationAction(Mapper.Map<LogAppActionCommand>(context.Data));
        }
    }
}
