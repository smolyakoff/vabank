using System.Data.Entity.ModelConfiguration;
using VaBank.Core.Accounting.Entities;
using VaBank.Data.EntityFramework.Common;

namespace VaBank.Data.EntityFramework.Accounting.Mappings
{
    public class CardVendorMap : EntityTypeConfiguration<CardVendor>
    {
        public CardVendorMap()
        {
            ToTable("CardVendor", "Accounting");
            HasKey(x => x.Id);
            Property(x => x.Name).HasMaxLength(Restrict.Length.ShortName).IsRequired();
            
            HasRequired(x => x.Image).WithOptional().Map(x => x.MapKey("ResourceId")).WillCascadeOnDelete(true);
        }
    }
}
