using System;
using System.Collections.Generic;
using System.Linq;
using VaBank.Common.Data.Repositories;
using VaBank.Core.Common;
using VaBank.Core.Maintenance;
using VaBank.Services.Common;
using VaBank.Services.Contracts.Admin.Maintenance;
using VaBank.Services.Contracts.Common;
using VaBank.Services.Contracts.Common.Models;
using VaBank.Services.Contracts.Common.Queries;
using VaBank.Services.Contracts.Common.Validation;

namespace VaBank.Services.Maintenance
{
    public class LogManagementService : BaseService, ILogManagementService
    {
        private readonly MaintenanceRepositories _db;

        public LogManagementService(IUnitOfWork unitOfWork, IValidatorFactory validatorFactory, MaintenanceRepositories repositories) 
            : base(unitOfWork, validatorFactory)
        {
            repositories.EnsureIsResolved();
            _db = repositories;
        }

        public SystemLogLookupModel GetSystemLogLookup()
        {
            try
            {
                var types = _db.LogEntries.ProjectAll<SystemLogTypeModel>().Select(x => x.Type).Distinct();
                return new SystemLogLookup(types);
            }
            catch (Exception ex)
            {
                throw new ServiceException("Cannot get system log lookup.", ex);
            }
        }

        public IEnumerable<SystemLogEntryBriefModel> GetSystemLogEntries(SystemLogQuery query)
        {
            EnsureIsValid(query);
            try
            {
                var entries = _db.LogEntries.Project<SystemLogEntryBriefModel>(query);
                return entries;
            }
            catch (Exception ex)
            {
                throw new ServiceException("Cannot get system log entries.", ex);
            }
        }

        public SystemLogExceptionModel GetSystemLogException(IdentityQuery<long> query)
        {
            EnsureIsValid(query);
            try
            {
                var exception = _db.LogEntries.ProjectOne<SystemLogEntry, SystemLogExceptionModel>(
                    query.SetConcreteFilter<SystemLogEntry, long>(x => x.Id));
                return exception;
            }
            catch (Exception ex)
            {
                throw new ServiceException("Cannot get system log entries.", ex);
            }
        }

        public UserMessage ClearSystemLog(SystemLogQuery query)
        {
            EnsureIsValid(query);
            try
            {
                var entries = _db.LogEntries.Query(query);
                foreach (var systemLogEntry in entries)
                {
                    _db.LogEntries.Delete(systemLogEntry);
                }
                UnitOfWork.Commit();
                return UserMessage.Format(Messages.SystemLogClearSuccess, new object[] {entries.Count()});
            }
            catch (Exception ex)
            {
                throw new ServiceException("Cannot get system log entries.", ex);
            }
        }
    }
}
