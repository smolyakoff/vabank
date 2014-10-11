using System;
using System.Collections.Generic;
using System.Linq;
using FluentValidation;
using VaBank.Common.Data;
using VaBank.Common.Data.Repositories;
using VaBank.Core.Common;
using VaBank.Core.Maintenance;
using VaBank.Services.Common;
using VaBank.Services.Contracts.Common;
using VaBank.Services.Contracts.Common.Models;
using VaBank.Services.Contracts.Common.Queries;
using VaBank.Services.Contracts.Maintenance;
using VaBank.Services.Contracts.Maintenance.Commands;
using VaBank.Services.Contracts.Maintenance.Models;
using VaBank.Services.Contracts.Maintenance.Queries;


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
                var entries = _db.LogEntries.ProjectThenQuery<SystemLogEntryBriefModel>(
                    query.ToDbQuery<SystemLogEntryBriefModel>());
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
                var exception = _db.LogEntries.ProjectIdentity<long, SystemLogEntry, SystemLogExceptionModel>(query);
                return exception;
            }
            catch (Exception ex)
            {
                throw new ServiceException("Cannot get system log entry exception", ex);
            }
        }

        public UserMessage ClearSystemLog(SystemLogQuery query)
        {
            EnsureIsValid(query);
            try
            {
                var deleted = _db.LogEntries.Delete(query.ToDbQuery<SystemLogEntry>());
                UnitOfWork.Commit();
                return UserMessage.ResourceFormat(() => Messages.SystemLogClearSuccess, deleted);
            }
            catch (Exception ex)
            {
                throw new ServiceException("Cannot clear system log entries.", ex);
            }
        }

        public UserMessage ClearSystemLog(SystemLogClearCommand command)
        {
            EnsureIsValid(command);
            try
            {
                var deleted = _db.LogEntries.Delete(command.ToDbQuery());
                UnitOfWork.Commit();
                return UserMessage.ResourceFormat(() => Messages.SystemLogClearSuccess, deleted);
            }
            catch (Exception ex)
            {
                throw new ServiceException("Cannot clear system log entries.", ex);
            }
        }
    }
}
