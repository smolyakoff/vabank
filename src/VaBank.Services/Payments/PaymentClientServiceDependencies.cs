using VaBank.Common.Data.Repositories;
using VaBank.Core.Payments.Entities;
using VaBank.Services.Common;

namespace VaBank.Services.Payments
{
    public class PaymentClientServiceDependencies : IDependencyCollection
    {
        public IRepository<PaymentTemplate> PaymentTemplates { get; set; }
    }
}
