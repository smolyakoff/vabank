using System.Collections.Generic;
using VaBank.Services.Contracts.Payments.Models;
using VaBank.Services.Contracts.Payments.Queries;

namespace VaBank.Services.Contracts.Payments
{
    public interface IPaymentStatisticsService
    {
        PaymentCategoryCostsModel GetCostsByPaymentCategory(PaymentCategoryCostsQuery query);

        IList<PaymentCategoryUsagesModel> GetMostlyUsedPayments(MostlyUsedPaymentsQuery query);
    }
}
