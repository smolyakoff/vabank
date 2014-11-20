using System;
using System.Collections.Generic;
using System.Linq;
using VaBank.Common.Data;
using VaBank.Common.Data.Repositories;
using VaBank.Core.App.Entities;
using VaBank.Core.Maintenance.Entitities;
using VaBank.Core.Processing.Entities;
using VaBank.Services.Common;
using VaBank.Services.Common.Exceptions;
using VaBank.Services.Contracts.Common;
using VaBank.Services.Contracts.Common.Models;
using VaBank.Services.Contracts.Maintenance;
using VaBank.Services.Contracts.Maintenance.Commands;
using VaBank.Services.Contracts.Maintenance.Models;
using VaBank.Services.Contracts.Maintenance.Queries;

namespace VaBank.Services.Maintenance
{
    public class LogService : BaseService, ILogService
    {
        private readonly MaintenanceRepositories _db;

        public LogService(BaseServiceDependencies dependencies, MaintenanceRepositories repositories) 
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
                Commit();
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
                Commit();
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
                var audit = _db.AuditLogs.Query(DbQuery.For<ApplicationAction>().FromClientQuery(query));
                var models = audit.Map<AuditLogBriefEntry, AuditLogEntryBriefModel>()
                    .OrderByDescending(x => x.StartedUtc)
                    .ToList();
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
                var model = entry.ToClass<AuditLogEntry, AuditLogEntryModel>();
                return model;
            }
            catch (Exception ex)
            {
                throw new ServiceException("Can't get audit log entry.", ex);
            }
        }
        
        public void LogApplicationAction(LogAppActionCommand command)
        {
            EnsureIsValid(command);
            try
            {
                var operation = _db.Operations.Find(command.OperationId);
                if (operation == null)
                {
                    throw NotFound.ExceptionFor<Operation>(command.OperationId);
                }
                var appAction = ApplicationAction.Create(
                    operation, 
                    command.Code, 
                    command.TimestampUtc,
                    command.Description, 
                    command.Data);
                _db.AuditLogs.CreateAction(appAction);
                Commit();
            }
            catch (Exception ex) 
            {
                throw new ServiceException("Can't create application action.", ex);
            }
        }

        public IList<TransactionLogEntryBriefModel> GetTransactionLogEntries()
        {
            try
            {
                return _db.Transactions.ProjectAll<TransactionLogEntryBriefModel>();
            }
            catch (Exception ex)
            {
                throw new ServiceException("Can't get all transaction models.", ex);
            }
        }

        public TransactionLogEntryModel GetTransactionLogEntry(IdentityQuery<Guid> transactionId)
        {
            EnsureIsValid(transactionId);
            try
            {
                var versions = _db.HistoricalRepository.GetAllVersions<HistoricalTransaction>(transactionId.Id)
                    .Select(x => x.ToClass<HistoricalTransaction, TransactionLogEntryHistoricalModel>()).ToList();
                return new TransactionLogEntryModel
                {
                    TransactionId = transactionId.Id,
                    Versions = versions
                };
            }
            catch (Exception ex)
            {
                throw new ServiceException("Can't get all transaction models.", ex);
            }
        }
    }
}
