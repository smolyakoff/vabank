using System.Data.Entity.ModelConfiguration;
using VaBank.Core.Processing.Entities;
using VaBank.Data.EntityFramework.Common;

namespace VaBank.Data.EntityFramework.Processing.Mappings
{
    public class OperationCategoryMap: EntityTypeConfiguration<OperationCategory>
    {
        public OperationCategoryMap()
        {
            ToTable("OperationCategory", "Processing");
            HasKey(x => x.Code).Property(x => x.Code).HasColumnName("Code");

            HasOptional(x => x.ParentCategory).WithMany(x => x.ChildrenCategories).Map(y => y.MapKey("Parent"));

            Property(x => x.Name).HasMaxLength(Restrict.Length.ShortName).IsRequired();
            Property(x => x.Description).HasMaxLength(Restrict.Length.BigString).IsOptional();
        }
    }
}
