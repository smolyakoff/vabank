using VaBank.Services.Contracts.Common.Queries;
using VaBank.Services.Contracts.Statistics.Models;

namespace VaBank.Services.Contracts.Statistics
{
    public interface IVaBankStatisticsService
    {
        OverallSystemInfoModel GetSystemInfo();

        ProcessedTransactionStatsModel GetProcessedTransactionStatistics(DateTimeRangeQuery query);
    }
}
