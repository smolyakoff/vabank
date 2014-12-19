using System;
using System.Collections.Generic;
using VaBank.Common.Data;
using VaBank.Common.Validation;
using VaBank.Core.Payments.Entities;
using VaBank.Services.Contracts.Payments.Queries;

namespace VaBank.Services.Payments
{
    internal static class PaymentsExtensions
    {
        public static DbQuery<CardPayment> ToCardPaymentQuery(this MostlyUsedPaymentsQuery query, IList<Guid> cardIds)
        {
            Argument.NotNull(query, "query");
            return DbQuery.For<CardPayment>()
                .FilterBy(x => cardIds.Contains(x.Card.Id))
                .AndFilterBy(x => x.CompletedDateUtc.HasValue &&
                                  x.CompletedDateUtc >= query.From &&
                                  x.CompletedDateUtc <= query.To);
        }

        public static DbQuery<CardPayment> ToCardPaymentQuery(this PaymentCategoryCostsQuery query)
        {
            return DbQuery.For<CardPayment>()
                .FilterBy(x => x.Card.Id == query.CardId)
                .AndFilterBy(x => x.CompletedDateUtc.HasValue && 
                    x.CompletedDateUtc >= query.From &&
                    x.CompletedDateUtc <= query.To);
        } 
    }
}
