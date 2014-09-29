using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using AutoMapper.QueryableExtensions;
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
            var levels = new string[] {"Info", "Debug"};
            Expression<Func<Log, bool>> exp = x => levels.Contains(x.Level);

            var results = context.Logs.AsQueryable()
                .Where(query.Filter.ToExpression<Log>())
                .Select(AutoMapper.Mapper.Map<Log,SystemLogEntryModel>)
                .ToList();
            return results;
        }
    }
}
