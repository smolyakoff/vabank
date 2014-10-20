using System;
using System.Collections.Generic;
using VaBank.Services.Contracts.Common.Models;
using VaBank.Services.Contracts.Common.Queries;
using VaBank.Services.Contracts.Maintenance.Commands;
using VaBank.Services.Contracts.Maintenance.Models;
using VaBank.Services.Contracts.Maintenance.Queries;

namespace VaBank.Services.Contracts.Maintenance
{
    public interface ILogManagementService : IService
    {
        SystemLogLookupModel GetSystemLogLookup();

        IList<SystemLogEntryBriefModel> GetSystemLogEntries(SystemLogQuery query);

        SystemLogExceptionModel GetSystemLogException(IdentityQuery<long> query);

        UserMessage ClearSystemLog(SystemLogQuery query);

        UserMessage ClearSystemLog(SystemLogClearCommand command);

        AuditLogLookupModel GetAuditLogLookup();

        IList<AuditLogEntryBriefModel> GetAuditLogEntries(AuditLogQuery query);

        AuditLogEntryModel GetAuditLogEntry(IdentityQuery<Guid> operationId);

        void LogApplicationAction(LogAppActionCommand command);
    }
}
