using VaBank.Core.Payments.Entities;

namespace VaBank.Data.EntityFramework.Payments.Mappings
{
    internal class PaymentOrderTemplateMap : BasePaymentOrderMap<PaymentOrderTemplate>
    {
        public PaymentOrderTemplateMap()
        {
            ToTable("PaymentOrderTemplate", "Payments").HasKey(x => x.PaymentTemplateCode);
            HasRequired(x => x.PaymentTemplate).WithRequiredPrincipal().Map(x => x.MapKey("PaymentTemplateCode"));
        }
    }
}
