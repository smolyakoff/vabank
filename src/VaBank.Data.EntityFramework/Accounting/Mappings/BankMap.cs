using System.Data.Entity.ModelConfiguration;
using VaBank.Core.Accounting.Entities;
using VaBank.Data.EntityFramework.Common;

namespace VaBank.Data.EntityFramework.Accounting.Mappings
{
    internal class BankMap : EntityTypeConfiguration<Bank>
    {
        public BankMap()
        {
            ToTable("Bank", "Accounting").HasKey(x => x.Code);
            HasOptional(x => x.Parent).WithMany(x => x.ChildrenBanks).Map(x => x.MapKey("Parent"));
            Property(x => x.Name).IsRequired().HasMaxLength(Restrict.Length.Name);
        }
    }
}
