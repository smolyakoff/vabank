using System.Data.Entity.ModelConfiguration;
using VaBank.Core.Payments.Entities;

namespace VaBank.Data.EntityFramework.Payments.Mappings
{
    internal class CardPaymentMap : EntityTypeConfiguration<CardPayment>
    {
        public CardPaymentMap()
        {
            ToTable("CardPayment", "Payments").HasKey(x => x.Id);
            Property(x => x.Id).HasColumnName("OperationId");

            HasRequired(x => x.Card).WithMany().Map(x => x.MapKey("CardId"));
        }
    }
}
