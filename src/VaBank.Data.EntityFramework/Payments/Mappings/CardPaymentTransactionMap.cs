using System.Data.Entity.ModelConfiguration;
using VaBank.Core.Payments.Entities;

namespace VaBank.Data.EntityFramework.Payments.Mappings
{
    public class CardPaymentTransactionMap : EntityTypeConfiguration<CardPaymentTransaction>
    {
        public CardPaymentTransactionMap()
        {
            ToTable("PaymentTransaction", "Payments").HasKey(x => x.Id);
            Property(x => x.Id).HasColumnName("TransactionId");

            HasRequired(x => x.Order).WithMany().Map(x => x.MapKey("PaymentOrderNo"));
        }
    }
}
