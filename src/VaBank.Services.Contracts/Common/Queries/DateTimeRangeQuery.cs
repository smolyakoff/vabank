using System;

namespace VaBank.Services.Contracts.Common.Queries
{
    public class DateTimeRangeQuery : IRangeQuery<DateTime>
    {
        public DateTime From { get; set; }

        public DateTime To { get; set; }
    }
}
