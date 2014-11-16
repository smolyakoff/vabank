using System;
using VaBank.Common.Util;

namespace VaBank.Services.Contracts.Processing.Queries
{
    public class CardAccountStatementQuery
    {
        public Guid CardId { get; set; }

        public Range<DateTime> DateRange { get; set; }
    }
}
