using System;
using VaBank.Services.Common;
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
            throw new NotImplementedException();
        }

        public ProcessedTransactionStatsModel GetProcessedTransactionStatistics(DateTimeRangeQuery query)
        {
            throw new NotImplementedException();
        }
    }
}
