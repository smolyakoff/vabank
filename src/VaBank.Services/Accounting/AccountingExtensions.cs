using System.Linq;
using VaBank.Common.Data;
using VaBank.Common.Validation;
using VaBank.Core.Accounting.Entities;
using VaBank.Core.Processing.Entities;
using VaBank.Services.Contracts.Processing.Queries;

namespace VaBank.Services.Accounting
{
    internal static class AccountingExtensions
    {
        public static DbQuery<CardTransaction> ToDbQuery(this CardAccountStatementQuery query, UserCard card)
        {
            Argument.NotNull(query, "query");

            return DbQuery.For<CardTransaction>()
                .FilterBy(x => x.CreatedDateUtc >= query.DateRange.LowerBound &&
                               x.CreatedDateUtc <= query.DateRange.UpperBound &&
                               x.Card.Id == query.CardId &&
                               x.AccountNo == card.Account.AccountNo)
                .SortBy(x => x.OrderByDescending(y => y.CreatedDateUtc));
        }
    }
}
