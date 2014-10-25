using System.Data.Entity.ModelConfiguration;
using VaBank.Core.Accounting.Entities;

namespace VaBank.Data.EntityFramework.Accounting.Mappings
{
    public class CardAccountMap : EntityTypeConfiguration<CardAccount>
    {
        public CardAccountMap()
        {
            ToTable("Account_Card", "Accounting");

            HasRequired(x => x.Card).WithOptional(x => x.Account).Map(x => x.MapKey("CardID"));
        }
    }
}
