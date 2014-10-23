using Autofac;
using AutoMapper;
using VaBank.Jobs.Common;
using VaBank.Services.Contracts.Common;
using VaBank.Services.Contracts.Maintenance.Commands;

namespace VaBank.Jobs.Maintenance.Audit
{
    public class AuditJob : EventListenerJob<AuditJobContext, IAuditedEvent>
    {
        public AuditJob(ILifetimeScope scope) : base(scope)
        {
        }

        protected override void Execute(AuditJobContext context)
        {
            context.LogManagementService.LogApplicationAction(Mapper.Map<LogAppActionCommand>(context.Data));
        }
    }
}
