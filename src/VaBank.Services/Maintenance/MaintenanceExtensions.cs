using System;
using System.Linq;
using VaBank.Common.Data;
using VaBank.Common.Validation;
using VaBank.Core.Maintenance.Entitities;
using VaBank.Core.Processing.Entities;
using VaBank.Services.Common;
using VaBank.Services.Contracts.Common.Models;
using VaBank.Services.Contracts.Maintenance.Commands;
using VaBank.Services.Contracts.Maintenance.Queries;

namespace VaBank.Services.Maintenance
{
    internal static class MaintenanceExtensions
    {
        public static IQuery ToDbQuery(this SystemLogClearCommand command)
        {
            if (command == null)
            {
                throw new ArgumentNullException("command");
            }
            return DbQuery.For<SystemLogEntry>().FilterBy(x => command.Ids.Contains(x.Id));
        }

        public static IQuery ToDbQuery(this TransactionLogQuery query)
        {
            Argument.NotNull(query, "query");
            var dbQuery = DbQuery.For<Transaction>().FromClientQuery(query);
            if (query.Status != null && query.Status.Length > 0)
            {
                var statuses = query.Status.Map<ProcessStatusModel, ProcessStatus>().ToList();
                dbQuery.AndFilterBy(x => statuses.Contains(x.Status));
            }
            dbQuery.SortBy(x => x.OrderByDescending(t => t.CreatedDateUtc));
            return dbQuery;
        } 
    }
}
