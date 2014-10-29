using System.Data.Entity.ModelConfiguration;
using VaBank.Core.Accounting;
using VaBank.Core.Accounting.Entities;
using VaBank.Data.EntityFramework.Common;

namespace VaBank.Data.EntityFramework.Accounting.Mappings
{
    public class CurrencyMap : EntityTypeConfiguration<Currency>
    {
        public CurrencyMap()
        {
            ToTable("Currency", "Accounting");
            HasKey(x => x.ISOName).Property(x => x.ISOName).HasColumnName("CurrencyISOName");

            Property(x => x.Symbol).IsRequired().HasMaxLength(3);
            Property(x => x.Name).IsRequired().HasMaxLength(Restrict.Length.Name);
        }
    }
}
