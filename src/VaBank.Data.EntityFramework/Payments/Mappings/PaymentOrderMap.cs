using VaBank.Core.Payments.Entities;

namespace VaBank.Data.EntityFramework.Payments.Mappings
{
    internal class PaymentOrderMap : BasePaymentOrderMap<PaymentOrder>
    {
        public PaymentOrderMap()
        {
            ToTable("PaymentOrder", "Payments").HasKey(x => x.No);            
        }
    }
}
