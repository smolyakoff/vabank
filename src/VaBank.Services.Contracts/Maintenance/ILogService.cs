using System;
using System.Collections.Generic;
using VaBank.Common.Data;
using VaBank.Services.Contracts.Common.Models;
using VaBank.Services.Contracts.Maintenance.Commands;
using VaBank.Services.Contracts.Maintenance.Models;
using VaBank.Services.Contracts.Maintenance.Queries;

namespace VaBank.Services.Contracts.Maintenance
{
    public interface ILogService : IService
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

        IList<TransactionLogEntryBriefModel> GetTransactionLogEntries(TransactionLogQuery query);

        TransactionLogEntryModel GetTransactionLogEntry(IdentityQuery<Guid> transactionId);
    }
}
