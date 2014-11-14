using System.Data.Entity.ModelConfiguration;
using VaBank.Core.Processing.Entities;

namespace VaBank.Data.EntityFramework.Processing.Mappings
{
    internal class ExchangeRateMap : EntityTypeConfiguration<ExchangeRate>
    {
        public ExchangeRateMap()
        {
            ToTable("ExchangeRate", "Processing").HasKey(x => x.Id);
            Property(x => x.Id).HasColumnName("ExchangeRateID");
            Property(x => x.BuyRate).IsRequired();
            Property(x => x.SellRate).IsRequired();
            Property(x => x.TimestampUtc).IsRequired();
            Property(x => x.IsActual).IsRequired();

            HasRequired(x => x.Base).WithMany().Map(x => x.MapKey("BaseCurrencyISOName"));
            HasRequired(x => x.Foreign).WithMany().Map(x => x.MapKey("ForeignCurrencyISOName"));
        }
    }
}
