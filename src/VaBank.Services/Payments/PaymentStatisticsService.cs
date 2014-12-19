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
                var groups = _deps.CardPayments.Query(query.ToCardPaymentQuery()).GroupBy(x => x.Category);
                var model = new PaymentCategoryCostsModel
                {
                    Card = card.ToModel<CardNameModel>(),
                    Currency = card.Account.Currency.ToModel<CurrencyModel>(),
                    Data = groups.Select(x => new PaymentCategoryCostsItemModel()
                    {
                        Category = x.First().ToModel<PaymentCategoryModel>(),
                        Amount = - x.Sum(y => y.Withdrawal.AccountAmount)
                    }).ToList()
                };
                model.Total = model.Data.Sum(x => x.Amount);
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
                //TODO: of course it will be great to group in database first
                var cardIds = _deps.UserCards.Select(DbQuery.For<UserCard>().FilterBy(x => x.Owner.Id == query.UserId), x => x.Id);
                var cardPayments = _deps.CardPayments.Query(query.ToCardPaymentQuery(cardIds));
                var groups = cardPayments.GroupBy(x => x.Category);
                var usages = groups.Select(g => new PaymentCategoryUsagesModel()
                {
                    Category = g.First().ToModel<PaymentCategoryModel>(),
                    Usages = g.Count()
                });
                return usages.OrderByDescending(x => x.Usages).Take(query.MaxResults).ToList();
            }
            catch (Exception ex)
            {
                throw new ServiceException("Can't get mostly used payments.", ex);
            }
        }
    }
}
