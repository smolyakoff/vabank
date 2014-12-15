using System.Linq;
using VaBank.Common.Data;
using VaBank.Common.Validation;
using VaBank.Core.Accounting.Entities;
using VaBank.Core.Processing.Entities;
using VaBank.Services.Contracts.Accounting.Queries;

namespace VaBank.Services.Accounting
{
    internal static class AccountingExtensions
    {
        public static DbQuery<Transaction> ToDbQuery(this CardAccountStatementQuery query, Account account)
        {
            Argument.NotNull(query, "query");

            return DbQuery.For<Transaction>()
                .FilterBy(x => x.CreatedDateUtc >= query.DateRange.LowerBound &&
                               x.CreatedDateUtc <= query.DateRange.UpperBound &&
                               x.AccountNo == account.AccountNo)
                .SortBy(x => x.OrderByDescending(y => y.CreatedDateUtc));
        }
    }
}
