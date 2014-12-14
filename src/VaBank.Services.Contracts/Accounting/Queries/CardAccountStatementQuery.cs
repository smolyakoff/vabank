using System;
using VaBank.Common.Util;

namespace VaBank.Services.Contracts.Accounting.Queries
{
    public class CardAccountStatementQuery
    {
        public string AccountNo { get; set; }

        public Range<DateTime> DateRange { get; set; }
    }
}
