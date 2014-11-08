using System.Data.Entity.ModelConfiguration;
using VaBank.Core.Processing.Entities;

namespace VaBank.Data.EntityFramework.Processing.Mappings
{
    internal class CurrencyRateMap : EntityTypeConfiguration<CurrencyRate>
    {
        public CurrencyRateMap()
        {
            ToTable("ExchangeRate", "Processing").HasKey(x => x.Id);
            HasRequired(x => x.From).WithOptional().Map(x => x.MapKey("FromCurrencyISOName"));
            HasRequired(x => x.To).WithOptional().Map(x => x.MapKey("ToCurrencyISOName"));
            Property(x => x.Id).HasColumnName("ExchangeRateID");
            Property(x => x.BuyRate).IsRequired();
            Property(x => x.SellRate).IsRequired();
            Property(x => x.TimestampUtc).IsRequired();
            Property(x => x.IsActual).IsRequired();
        }
    }
}
