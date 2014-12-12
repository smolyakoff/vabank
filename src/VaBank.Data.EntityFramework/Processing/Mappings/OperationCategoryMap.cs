using System.Data.Entity.ModelConfiguration;
using VaBank.Core.Processing.Entities;
using VaBank.Data.EntityFramework.Common;

namespace VaBank.Data.EntityFramework.Processing.Mappings
{
    public class OperationCategoryMap: EntityTypeConfiguration<OperationCategory>
    {
        public OperationCategoryMap()
        {
            ToTable("OperationCategory", "Accounting");
            HasKey(x => x.Code).Property(x => x.Code).HasColumnName("Code");

            HasOptional(x => x.Parent).WithMany(x => x.Children).Map(y => y.MapKey("Parent"));

            Property(x => x.Name).HasMaxLength(Restrict.Length.ShortName).IsRequired();
            Property(x => x.Description).HasMaxLength(Restrict.Length.BigString).IsOptional();
        }
    }
}
