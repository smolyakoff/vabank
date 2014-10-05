using System.Collections.Generic;
using VaBank.Services.Contracts.Common.Models;
using VaBank.Services.Contracts.Common.Queries;

namespace VaBank.Services.Contracts.Maintenance
{
    public interface ILogManagementService : IService
    {
        SystemLogLookupModel GetSystemLogLookup();

        IEnumerable<SystemLogEntryBriefModel> GetSystemLogEntries(SystemLogQuery query);

        SystemLogExceptionModel GetSystemLogException(IdentityQuery<long> query);

        UserMessage ClearSystemLog(SystemLogQuery query);

        UserMessage ClearSystemLog(SystemLogClearCommand command);
    }
}
