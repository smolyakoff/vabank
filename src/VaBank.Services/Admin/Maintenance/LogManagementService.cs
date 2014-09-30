using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using AutoMapper.QueryableExtensions;
using VaBank.Common.Data.Filtering;
using VaBank.Common.Data.Linq;
using VaBank.Core.Entities;
using VaBank.Data.EntityFramework;
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
            var context = new VaBankContext();
            var queryable = context.Logs.AsQueryable();
            var results = queryable
                .Where(query.Filter)
                .AsEnumerable<Log>()
                .Select(AutoMapper.Mapper.Map<Log, SystemLogEntryModel>)
                .ToList();
            return results;
        }
    }
}
