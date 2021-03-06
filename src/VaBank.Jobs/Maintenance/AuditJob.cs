﻿using Autofac;
using AutoMapper;
using Hangfire;
using VaBank.Jobs.Common;
using VaBank.Services.Contracts.Common.Events;
using VaBank.Services.Contracts.Maintenance.Commands;

namespace VaBank.Jobs.Maintenance
{
    [AutomaticRetry(Attempts = 1)]
    public class AuditJob : EventListenerJob<AuditJobContext, IAuditedEvent>
    {
        public AuditJob(ILifetimeScope scope) : base(scope)
        {
        }

        protected override void Execute(AuditJobContext context)
        {
            context.LogService.LogApplicationAction(Mapper.Map<LogAppActionCommand>(context.Data));
        }
    }
}
