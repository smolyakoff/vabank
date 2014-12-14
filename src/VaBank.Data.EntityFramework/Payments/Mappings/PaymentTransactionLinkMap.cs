using System.Data.Entity.ModelConfiguration;
using VaBank.Core.Payments.Entities;

namespace VaBank.Data.EntityFramework.Payments.Mappings
{
    public class PaymentTransactionLinkMap : EntityTypeConfiguration<PaymentTransactionLink>
    {
        public PaymentTransactionLinkMap()
        {
            ToTable("PaymentTransaction", "Payments");
            HasKey(x => x.TransactionId);
            Property(x => x.TransactionId).HasColumnName("TransactionID");

            HasRequired(x => x.Order).WithMany().Map(x => x.MapKey("PaymentOrderNo"));
            HasRequired(x => x.Transaction).WithRequiredDependent();
        }
    }
}
