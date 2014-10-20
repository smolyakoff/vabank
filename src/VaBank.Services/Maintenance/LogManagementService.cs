using System;
using System.Collections.Generic;
using System.Linq;
using VaBank.Common.Data;
using VaBank.Common.Data.Repositories;
using VaBank.Core.App;
using VaBank.Core.Maintenance;
using VaBank.Core.Membership;
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

        public LogManagementService(BaseServiceDependencies dependencies, MaintenanceRepositories repositories) 
            : base(dependencies)
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

        public IList<SystemLogEntryBriefModel> GetSystemLogEntries(SystemLogQuery query)
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

        public AuditLogLookupModel GetAuditLogLookup()
        {
            try
            {
                var codes = _db.AuditLogs.GetUniqueCodes();
                var model = new AuditLogLookupModel();
                model.Codes.AddRange(codes);
                return model;
            }
            catch (Exception ex)
            {
                throw new ServiceException("Cannot get audit log lookup model.", ex);
            }
        }

        public IList<AuditLogEntryBriefModel> GetAuditLogEntries(AuditLogQuery query)
        {
            EnsureIsValid(query);
            try
            {
                var audit = _db.AuditLogs.GetAuditEntries(DbQuery.For<ApplicationAction>()
                    .FilterBy(query.ClientFilter));

                var userIds =
                    audit.Where(x => x.Operation.UserId.HasValue).Select(s => s.Operation.UserId.Value).ToList();
                var usersNameKeyPairs =
                    _db.Users.Query(DbQuery.For<User>().FilterBy(x => userIds.Distinct().Contains(x.Id)))
                        .ToDictionary(k => k.Id, e => e.UserName);
                var models = audit.Select(x => x.ToClass<AuditLogBriefEntry, AuditLogEntryBriefModel>()).ToList();

                foreach (var model in models)
                {
                    if (model.UserId.HasValue)
                    {
                        model.UserName = usersNameKeyPairs[model.UserId.Value];
                    }
                }
                return models;
            }
            catch (Exception ex)
            {
                throw new ServiceException("Can't get audit log entries.", ex);
            }
        }

        public AuditLogEntryModel GetAuditLogEntry(IdentityQuery<Guid> operationId)
        {
            EnsureIsValid(operationId);
            try
            {
                var entry = _db.AuditLogs.GetAuditEntryDetails(operationId.Id);
                return entry.ToClass<AuditLogEntry, AuditLogEntryModel>();
            }
            catch (Exception ex)
            {
                throw new ServiceException("Can't get audit log entry.", ex);
            }
        }
        
        public void CreateApplicationAction(CreateAppActionCommand command)
        {
            EnsureIsValid(command);
            try
            {
                var appAction = command.ToEntity<CreateAppActionCommand, ApplicationAction>();
                _db.AuditLogs.CreateAction(appAction);
                UnitOfWork.Commit();
            }
            catch (Exception ex) 
            {
                throw new ServiceException("Can't create application action.", ex);
            }
        }
    }
}
