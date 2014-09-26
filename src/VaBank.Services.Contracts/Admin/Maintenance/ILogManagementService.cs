using System.Collections.Generic;

namespace VaBank.Services.Contracts.Admin.Maintenance
{
    public interface ILogManagementService : IService
    {
        SystemLogLookupModel GetSystemLogLookup();
        IEnumerable<SystemLogEntryModel> GetSystemLogEntries(SystemLogQuery query);
    }
}
