using System;
using System.Collections.Generic;
using System.Linq;
using VaBank.Common.Data;
using VaBank.Core.Accounting.Entities;
using VaBank.Core.Payments.Entities;
using VaBank.Services.Common;
using VaBank.Services.Common.Security;
using VaBank.Services.Contracts.Accounting.Models;
using VaBank.Services.Contracts.Common;
using VaBank.Services.Contracts.Payments;
using VaBank.Services.Contracts.Payments.Models;
using VaBank.Services.Contracts.Payments.Queries;

namespace VaBank.Services.Payments
{
    public class PaymentStatisticsService : BaseService, IPaymentStatisticsService
    {
        private readonly PaymentStatisticsServiceDependencies _deps;

        public PaymentStatisticsService(BaseServiceDependencies dependencies, PaymentStatisticsServiceDependencies deps)
            : base(dependencies)
        {
            deps.EnsureIsResolved();
            _deps = deps;
        }

        public PaymentCategoryCostsModel GetCostsByPaymentCategory(PaymentCategoryCostsQuery query)
        {
            EnsureIsValid(query);
            try
            {
                var card = _deps.UserCards.SurelyFind(query.CardId);
                var cardPayments =
                    _deps.CardPayments.Query(
                        DbQuery.For<CardPayment>()
                            .FilterBy(x => x.Card.Id == query.CardId)
                            .AndFilterBy(
                                x =>
                                    x.CompletedDateUtc.HasValue && x.CompletedDateUtc >= query.From &&
                                    x.CompletedDateUtc <= query.To)).GroupBy(x => x.Category);
                
                var model = new PaymentCategoryCostsModel
                {
                    Card = card.ToModel<CardNameModel>(),
                    Currency = card.Account.Currency.ToModel<CurrencyModel>()
                };

                var total = 0m;
                foreach (var cardPayment in cardPayments)
                {
                    var spent = cardPayment.Aggregate(0m,
                        (cost, payment) => cost + payment.Withdrawal.TransactionAmount);
                    total += spent;
                    var itemModel = new PaymentCategoryCostsItemModel
                    {
                        Category = cardPayment.First().ToModel<PaymentCategoryModel>(),
                        Spent = spent
                    };
                    model.Data.Add(itemModel);
                }
                model.TotalSpent = total;
                
                return model;
            }
            catch (Exception ex)
            {
                throw new ServiceException("Can't get costs by payment category.", ex);
            }
        }

        public IList<PaymentCategoryUsagesModel> GetMostlyUsedPayments(MostlyUsedPaymentsQuery query)
        {
            EnsureIsValid(query);
            EnsureIsSecure<MostlyUsedPaymentsQuery, UserQueryValidator>(query);
            try
            {
                var cardIds = _deps.UserCards.Select(DbQuery.For<UserCard>().FilterBy(x => x.Owner.Id == query.UserId),
                    x => x.Id);
                var cardPayments =
                    _deps.CardPayments.Query(
                        DbQuery.For<CardPayment>().FilterBy(x => cardIds.Contains(x.Card.Id)).AndFilterBy(x =>
                            x.CompletedDateUtc.HasValue && x.CompletedDateUtc >= query.From &&
                            x.CompletedDateUtc <= query.To)).GroupBy(x => x.Category);

                return (from cardPayment in cardPayments
                    let count = cardPayment.Count()
                    select new PaymentCategoryUsagesModel
                    {
                        Category = cardPayment.First().ToModel<PaymentCategoryModel>(),
                        Usages = count
                    }).OrderBy(x => x.Usages).Take(query.MaxResults).ToList();
            }
            catch (Exception ex)
            {
                throw new ServiceException("Can't get mostly used payments.", ex);
            }
        }
    }
}
