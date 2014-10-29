using System.Data.Entity.ModelConfiguration;
using VaBank.Core.Accounting.Entities;
using VaBank.Data.EntityFramework.Common;

namespace VaBank.Data.EntityFramework.Accounting.Mappings
{
    public class CardSettingsMap : EntityTypeConfiguration<CardSettings>
    {
        public CardSettingsMap()
        {
            ToTable("CardSettings", "Accounting");
            HasKey(x => x.CardId);

            Property(x => x.FriendlyName).IsOptional().HasMaxLength(Restrict.Length.ShortName);
            Property(x => x.Blocked).IsRequired();
            Property(x => x.BlockedDateUtc).IsOptional();
        }
    }
}
