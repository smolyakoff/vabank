using System.Data.Entity.ModelConfiguration;
using VaBank.Core.Payments.Entities;
using VaBank.Data.EntityFramework.Common;

namespace VaBank.Data.EntityFramework.Payments.Mappings
{
    public class PaymentTemplateMap : EntityTypeConfiguration<PaymentTemplate>
    {
        public PaymentTemplateMap()
        {
            ToTable("PaymentTemplate", "Payments").HasKey(x => x.Code);
            Property(x => x.Form).IsRequired().IsMaxLength();

            HasRequired(x => x.Account).WithMany().Map(x => x.MapKey("AccountNo"));
            HasRequired(x => x.Category).WithRequiredPrincipal();
            HasRequired(x => x.Currency).WithMany().Map(x => x.MapKey("CurrencyISOName"));
        }
    }
}
