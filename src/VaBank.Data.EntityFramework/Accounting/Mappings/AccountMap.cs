using System.Data.Entity.ModelConfiguration;
using VaBank.Core.Accounting.Entities;

namespace VaBank.Data.EntityFramework.Accounting.Mappings
{
    public class AccountMap : EntityTypeConfiguration<Account>
    {
        public AccountMap()
        {
            ToTable("Account", "Accounting");
            HasKey(x => x.AccountNo);

            Property(x => x.Balance).IsRequired();
            Property(x => x.OpenDateUtc).IsRequired();
            Property(x => x.ExpirationDateUtc).IsRequired();
            Property(x => x.Type).IsRequired();
            Property(x => x.RowVersion).IsRowVersion();

            HasRequired(x => x.Currency).WithMany().Map(x => x.MapKey("CurrencyISOName"));
        }
    }
}
