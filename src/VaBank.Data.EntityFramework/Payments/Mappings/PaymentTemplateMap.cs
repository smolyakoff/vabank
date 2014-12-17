using System.Data.Entity.ModelConfiguration;
using VaBank.Core.Payments.Entities;
using VaBank.Data.EntityFramework.Common;

namespace VaBank.Data.EntityFramework.Payments.Mappings
{
    public class PaymentTemplateMap : EntityTypeConfiguration<PaymentTemplate>
    {
        public PaymentTemplateMap()
        {
            ToTable("PaymentTemplate", "Payments");
            HasKey(x => x.Code);
            Property(x => x.DisplayName).IsRequired().HasMaxLength(Restrict.Length.Name);
            Property(x => x.FormTemplate).IsRequired().IsMaxLength();
            Property(x => x.InfoTemplate).IsOptional().IsMaxLength();

            HasRequired(x => x.OrderTemplate).WithRequiredPrincipal();
        }
    }
}