using System.Collections.Generic;
using VaBank.Services.Contracts.Common.Queries;
using VaBank.Services.Contracts.Statistics.Models;

namespace VaBank.Services.Contracts.Statistics
{
    public interface IVaBankStatisticsService
    {
        OverallSystemInfoModel GetSystemInfo();

        IList<ProcessedTransactionStatsModel> GetProcessedTransactionStatistics(DateTimeRangeQuery query);
    }
}
