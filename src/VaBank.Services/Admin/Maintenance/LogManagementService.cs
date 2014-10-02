using System.Collections.Generic;
using System.Linq;
using VaBank.Common.Data.Filtering;
using VaBank.Core.Entities;
using VaBank.Data.EntityFramework;
using VaBank.Services.Contracts.Admin.Maintenance;
using VaBank.Services.Validation;

namespace VaBank.Services.Admin.Maintenance
{
    public class LogManagementService : BaseService, ILogManagementService
    {
        public LogManagementService(IValidationFactory validationFactory) : base(validationFactory)
        {
        }

        public SystemLogLookupModel GetSystemLogLookup()
        {
            //TODO: do real logic here
            return new SystemLogLookup(Enumerable.Empty<string>());
        }

        public IEnumerable<SystemLogEntryModel> GetSystemLogEntries(SystemLogQuery query)
        {
            //TODO: do real logic here
            var validationResult = Validate(query);

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
