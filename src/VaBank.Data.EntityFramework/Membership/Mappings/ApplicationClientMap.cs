using System.Data.Entity.ModelConfiguration;
using VaBank.Core.Membership;

namespace VaBank.Data.EntityFramework.Membership.Mappings
{
    internal class ApplicationClientMap : EntityTypeConfiguration<ApplicationClient>
    {
        public ApplicationClientMap()
        {
            ToTable("ApplicationClient", "Membership").HasKey(x => x.Id);
            Property(x => x.Name).HasMaxLength(RestrictionConstants.NameLength).IsRequired();
            Property(x => x.AllowedOrigin).HasMaxLength(RestrictionConstants.UrlLength)
                .IsOptional();
            Property(x => x.ApplicationType).IsRequired();
            Property(x => x.RefreshTokenLifeTime).IsRequired();
            Property(x => x.Active).IsRequired();
            Property(x => x.Secrete).IsRequired();
        }
    }
}