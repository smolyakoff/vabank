using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using VaBank.Core.App;
using VaBank.Data.EntityFramework.Membership.Mappings;

namespace VaBank.Data.EntityFramework.App.Mappings
{
    public class OperationMarkerMap : EntityTypeConfiguration<OperationMarker>
    {
        public OperationMarkerMap()
        {
            ToTable("OperationMarker", "App");
            HasKey(x => x.Id);
            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.Name).HasMaxLength(RestrictionConstants.NameLength);
            Property(x => x.UserId).HasColumnName("AppUserId").IsOptional();
            Property(x => x.ClientName).HasColumnName("AppClientId").HasMaxLength(RestrictionConstants.NameLength).IsOptional();
        }
    }
}
