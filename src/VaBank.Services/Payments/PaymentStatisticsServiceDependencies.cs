using VaBank.Common.Data.Repositories;
using VaBank.Core.Accounting.Entities;
using VaBank.Core.Payments.Entities;
using VaBank.Services.Common;

namespace VaBank.Services.Payments
{
    public class PaymentStatisticsServiceDependencies : IDependencyCollection
    {
        public IQueryRepository<CardPayment> CardPayments { get; set; }
        public IQueryRepository<UserCard> UserCards { get; set; } 
    }
}
