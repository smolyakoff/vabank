using System.Data.Entity.ModelConfiguration;
using VaBank.Core.Accounting.Entities;

namespace VaBank.Data.EntityFramework.Accounting.Mappings
{
    public class CardAccountMap : EntityTypeConfiguration<CardAccount>
    {
        public CardAccountMap()
        {
            ToTable("User_Account", "Accounting");

            HasMany(x => x.Cards).WithOptional(x => x.Account).Map(x => x.MapKey("AccountNo"));
        }
    }
}
