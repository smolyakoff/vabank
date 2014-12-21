using System;
using System.Collections.Generic;
using System.Linq;
using VaBank.Common.Data;
using VaBank.Core.Processing;
using VaBank.Core.Processing.Entities;
using VaBank.Services.Common;
using VaBank.Services.Contracts.Common;
using VaBank.Services.Contracts.Common.Queries;
using VaBank.Services.Contracts.Maintenance;
using VaBank.Services.Contracts.Maintenance.Models;
using VaBank.Services.Contracts.Maintenance.Queries;
using VaBank.Services.Contracts.Statistics.Models;

namespace VaBank.Services.Maintenance
{
    public class SystemStatisticsService : BaseService, ISystemStatisticsService
    {
        private readonly SystemStatisticsServiceDependencies _deps;

        public SystemStatisticsService(BaseServiceDependencies baseDeps, SystemStatisticsServiceDependencies deps)
            : base(baseDeps)
        {
            deps.EnsureIsResolved();
            _deps = deps;
        }

        public OverallSystemInfoModel GetSystemInfo()
        {
            try
            {
                var dbVersion = _deps.DbInformation.GetDbVersion();
                var completedOperations =_deps.BankOperations.Count(DbQuery.For<BankOperation>().FilterBy(BankOperation.Spec.Finished));
                var usersCount = _deps.Users.Count();
                return new OverallSystemInfoModel
                {
                    CompletedBankOperationsCount = completedOperations,
                    DbVersion = dbVersion,
                    UsersCount = usersCount
                };
            }
            catch (Exception ex)
            {
                throw new ServiceException("Can't get system information.", ex);
            }
        }

        public IList<ProcessedTransactionStatsModel> GetProcessedTransactionStatistics(TransactionStatisticsQuery query)
        {
            EnsureIsValid(query);
            try
            {
                var dbQuery = DbQuery.For<Transaction>()
                    .FilterBy(Specs.ForTransaction.Finished)
                    .AndFilterBy(x => x.CreatedDateUtc >= query.From.Date && x.CreatedDateUtc <= query.To.Date);
                var groups = _deps.Transactions.Select(dbQuery, x => x.CreatedDateUtc).GroupBy(x => x.Date);
                var span = query.To.Date - query.From.Date;
                return Enumerable.Range(0, span.Days).Select(x =>
                {
                    var date = query.From.Date.AddDays(x);
                    var group = groups.FirstOrDefault(g => g.Key == date);
                    return new ProcessedTransactionStatsModel
                    {
                        Date = date,
                        TransactionsCount = group == null ? 0 : group.Count()
                    };
                }).ToList();
            }
            catch (Exception ex)
            {
                throw new ServiceException("Can't get processed transactions statistics.", ex);
            }
        }
    }
}
