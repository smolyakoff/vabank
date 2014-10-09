using System.Data.Entity.ModelConfiguration;
using VaBank.Core.Membership;

namespace VaBank.Data.EntityFramework.Membership.Mappings
{
    internal class ApplicationTokenMap : EntityTypeConfiguration<ApplicationToken>
    {
        public ApplicationTokenMap()
        {
            ToTable("ApplicationToken", "Membership").HasKey(x => x.Id);
            Property(x => x.IssuedUtc).IsRequired();
            Property(x => x.ExpiresUtc).IsRequired();
            Property(x => x.ProtectedTicket).HasMaxLength(RestrictionConstants.SecurityStringLength).IsRequired();
            HasRequired(x => x.Client).WithMany().Map(k => k.MapKey("ClientId"));
            HasRequired(x => x.User).WithOptional().Map(k => k.MapKey("UserId"));
        }
    }
}