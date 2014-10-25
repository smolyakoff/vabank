using System.Data.Entity.ModelConfiguration;
using VaBank.Core.Accounting.Entities;

namespace VaBank.Data.EntityFramework.Accounting.Mappings
{
    public class CardMap : EntityTypeConfiguration<Card>
    {
        public CardMap()
        {
            ToTable("Card", "Accounting");
            HasKey(x => x.Id).Property(x => x.Id).HasColumnName("CardId");

            Property(x => x.CardNo).IsRequired();
            Property(x => x.HolderFirstName).IsRequired();
            Property(x => x.HolderLastName).IsRequired();
            Property(x => x.ExpirationDateUtc).IsRequired();

            HasRequired(x => x.CardVendor).WithMany().Map(x => x.MapKey("CardVendorId"));
        }
    }
}
