using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using VaBank.Core.App;
using VaBank.Data.EntityFramework.Common;

namespace VaBank.Data.EntityFramework.App.Mappings
{
    internal class OperationMap : EntityTypeConfiguration<Operation>
    {
        public OperationMap()
        {
            ToTable("Operation", "App");
            HasKey(x => x.Id);
            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.Name).HasMaxLength(RestrictionConstants.NameLength);
            Property(x => x.UserId).HasColumnName("AppUserId").IsOptional();
            Property(x => x.ClientApplicationId).HasColumnName("AppClientId").HasMaxLength(RestrictionConstants.NameLength).IsOptional();
        }
    }
}
