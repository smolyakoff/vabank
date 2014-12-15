using VaBank.Common.Data.Repositories;
using VaBank.Core.Accounting.Entities;
using VaBank.Core.Payments.Entities;
using VaBank.Core.Payments.Factories;
using VaBank.Core.Processing.Entities;
using VaBank.Services.Common;

namespace VaBank.Services.Payments
{
    public class PaymentClientServiceDependencies : IDependencyCollection
    {
        public IRepository<PaymentTemplate> PaymentTemplates { get; set; }

        public IRepository<UserCard> UserCards { get; set; }
 
        public IRepository<Payment> Payments { get; set; }

        public IQueryRepository<CardPayment> CardPayments { get; set; }

        public IQueryRepository<UserBankOperation> UserBankOperations { get; set; }

        public IRepository<PaymentTransactionLink> PaymentTransactionLinks { get; set; }

        public CardPaymentFactory CardPaymentFactory { get; set; }
    }
}
