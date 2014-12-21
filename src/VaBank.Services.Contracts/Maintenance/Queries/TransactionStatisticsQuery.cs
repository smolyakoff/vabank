using System;
using VaBank.Services.Contracts.Common.Queries;

namespace VaBank.Services.Contracts.Maintenance.Queries
{
    public class TransactionStatisticsQuery : DateTimeRangeQuery
    {
        public TransactionStatisticsQuery()
        {
            From = DateTime.UtcNow.AddDays(-10);
            To = DateTime.UtcNow.Date.AddDays(1);
        }
    }
}
