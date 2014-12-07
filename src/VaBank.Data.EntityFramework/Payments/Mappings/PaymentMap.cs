using System.Data.Entity.ModelConfiguration;
using VaBank.Core.Payments.Entities;
using VaBank.Data.EntityFramework.Common;

namespace VaBank.Data.EntityFramework.Payments.Mappings
{
    internal class PaymentMap : EntityTypeConfiguration<Payment>
    {
        public PaymentMap()
        {
            ToTable("Payment", "Payments").HasKey(x => x.Id);
            Property(x => x.Id).HasColumnName("OperationId");
            Property(x => x.Form).IsRequired().IsMaxLength();
            Property(x => x.Name).IsRequired().HasMaxLength(Restrict.Length.BigString);

            HasRequired(x => x.Order).WithRequiredPrincipal().Map(x => x.MapKey("OrderNo"));
            HasRequired(x => x.Template).WithMany().Map(x => x.MapKey("TemplateCode"));
        }
    }
}
