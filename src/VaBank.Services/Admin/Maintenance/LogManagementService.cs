using System;
using System.Collections.Generic;
using System.Linq;
using VaBank.Services.Contracts.Admin.Maintenance;

namespace VaBank.Services.Admin.Maintenance
{
    public class LogManagementService : BaseService, ILogManagementService
    {
        public SystemLogLookupModel GetSystemLogLookup()
        {
            //TODO: do real logic here
            return new SystemLogLookup(Enumerable.Empty<string>());
        }

        public IEnumerable<SystemLogEntryModel> GetSystemLogEntries(SystemLogQuery query)
        {
            //TODO: do real logic here
            return new[]
            {
                new SystemLogEntryModel
                {
                    EventId = 1,
                    Message = "Test Log 1"
                },
                new SystemLogEntryModel
                {
                    EventId = 2,
                    Message = "Test Log 2"
                },
                new SystemLogEntryModel
                {
                    EventId = 3,
                    Message = "Test Log 3"
                }
            };
        }
    }
}
