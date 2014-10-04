using System;
using System.Collections.Generic;
using System.Linq;
using VaBank.Common.Data.Filtering;
using VaBank.Core.Entities;
using VaBank.Core.Repositories;
using VaBank.Data.EntityFramework;
using VaBank.Services.Contracts.Admin.Maintenance;
using VaBank.Services.Contracts.Common.Queries;
using VaBank.Services.Contracts.Common.Validation;

namespace VaBank.Services.Admin.Maintenance
{
    public class LogManagementService : BaseService, ILogManagementService
    {
        private readonly IRepository<Log> _logRepository; 

        public LogManagementService(IRepository<Log> logRepository, IValidatorFactory validatorFactory) 
            : base(validatorFactory)
        {
            if (logRepository == null)
                throw new ArgumentNullException("logRepository");
            _logRepository = logRepository;
        }

        public SystemLogLookupModel GetSystemLogLookup()
        {
            //TODO: do real logic here
            return new SystemLogLookup(Enumerable.Empty<string>());
        }

        public IEnumerable<SystemLogEntryBriefModel> GetSystemLogEntries(SystemLogQuery query)
        {
            //TODO: do real logic here
            EnsureIsValid(query);

            var context = new VaBankContext();
            var queryable = context.Logs.AsQueryable();
            var results = queryable
                .Where(query.Filter)
                .AsEnumerable<Log>()
                .Select(AutoMapper.Mapper.Map<Log, SystemLogEntryBriefModel>)
                .ToList();
            return results;
        }

        public SystemLogExceptionModel GetSystemLogException(IdentityQuery<long> eventId)
        {
            throw new System.NotImplementedException();
        }
    }
}
