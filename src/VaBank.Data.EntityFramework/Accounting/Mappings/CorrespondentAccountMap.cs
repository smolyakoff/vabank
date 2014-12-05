using System.Data.Entity.ModelConfiguration;
using VaBank.Core.Accounting.Entities;

namespace VaBank.Data.EntityFramework.Accounting.Mappings
{
    public class CorrespondentAccountMap : EntityTypeConfiguration<CorrespondentAccount>
    {
        public CorrespondentAccountMap()
        {
            ToTable("CorrespondentAccount", "Accounting").HasKey(x => x.AccountNo);
            HasRequired(x => x.Bank).WithMany().Map(x => x.MapKey("BankCode"));
        }
    }
}
