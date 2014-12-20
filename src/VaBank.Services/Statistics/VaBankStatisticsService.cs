using System;
using System.Collections.Generic;
using System.Linq;
using VaBank.Common.Data;
using VaBank.Core.Processing.Entities;
using VaBank.Services.Common;
using VaBank.Services.Contracts.Common;
using VaBank.Services.Contracts.Common.Queries;
using VaBank.Services.Contracts.Statistics;
using VaBank.Services.Contracts.Statistics.Models;

namespace VaBank.Services.Statistics
{
    public class VaBankStatisticsService : BaseService, IVaBankStatisticsService
    {
        private readonly VaBankStatisticsServiceDependencies _deps;

        public VaBankStatisticsService(BaseServiceDependencies baseDeps, VaBankStatisticsServiceDependencies deps)
            : base(baseDeps)
        {
            deps.EnsureIsResolved();
            _deps = deps;
        }

        public OverallSystemInfoModel GetSystemInfo()
        {
            try
            {
                var serverVersion = _deps.DbInformation.GetServerVersion();
                var transactionsCount = _deps.Transactions.Count(
                    DbQuery.For<Transaction>().FilterBy(x => x.Status == ProcessStatus.Completed));
                var usersCount = _deps.Users.Count();
                return new OverallSystemInfoModel
                {
                    ProcessedTransactionsCount = transactionsCount,
                    ServerVersion = serverVersion,
                    UsersCount = usersCount
                };
            }
            catch (Exception ex)
            {
                throw new ServiceException("Can't get system information.", ex);
            }
        }

        public IList<ProcessedTransactionStatsModel> GetProcessedTransactionStatistics(DateTimeRangeQuery query)
        {
            EnsureIsValid(query);
            try
            {
                var groups = _deps.Transactions.Select(
                    DbQuery.For<Transaction>()
                        .FilterBy(x => x.Status == ProcessStatus.Completed)
                        .AndFilterBy(
                            x => x.PostDateUtc.HasValue && x.PostDateUtc >= query.From && x.PostDateUtc <= query.To),
                    x => x.PostDateUtc).GroupBy(x => x.Value);
                return (from @group in groups
                    select new ProcessedTransactionStatsModel
                    {
                        Date = @group.Key,
                        TransactionsCount = @group.Count()
                    }).ToList();
            }
            catch (Exception ex)
            {
                throw new ServiceException("Can't get processed transactions statistics.", ex);
            }
        }
    }
}
