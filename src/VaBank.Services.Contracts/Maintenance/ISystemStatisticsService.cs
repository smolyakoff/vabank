using System.Collections.Generic;
using VaBank.Services.Contracts.Maintenance.Models;
using VaBank.Services.Contracts.Maintenance.Queries;
using VaBank.Services.Contracts.Statistics.Models;

namespace VaBank.Services.Contracts.Maintenance
{
    public interface ISystemStatisticsService
    {
        OverallSystemInfoModel GetSystemInfo();

        IList<ProcessedTransactionStatsModel> GetProcessedTransactionStatistics(TransactionStatisticsQuery query);
    }
}
