using System.Data.Entity.ModelConfiguration;
using VaBank.Core.Processing.Entities;
using VaBank.Data.EntityFramework.Common;

namespace VaBank.Data.EntityFramework.Processing.Mappings
{
    public class OperationMap : EntityTypeConfiguration<BankOperation>
    {
        public OperationMap()
        {
            ToTable("Operation", "Processing");
            HasKey(x => x.Id).Property(x => x.Id).HasColumnName("ID");
            HasRequired(x => x.Category).WithMany().Map(x => x.MapKey("CategoryCode"));
            Property(x => x.CreatedDateUtc).IsRequired();
            Property(x => x.CompletedDateUtc).IsOptional();
            Property(x => x.ErrorMessage).HasMaxLength(Restrict.Length.BigString).IsOptional();
            Property(x => x.Status).IsRequired();
        }
    }
}
