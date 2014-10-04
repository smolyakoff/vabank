using System.Data.Entity.ModelConfiguration;
using VaBank.Core.Membership;

namespace VaBank.Data.EntityFramework.Maintenance.Mappings
{
    internal class UserMap : EntityTypeConfiguration<User>
    {
        public UserMap()
        {
            ToTable("User", "Membership").HasKey(x => x.Id)
                .HasMany(x => x.Claims).WithRequired()
                .HasForeignKey(x => x.UserId);
            Property(x => x.Id).HasColumnName("UserID");
            HasRequired(x => x.Profile).WithRequiredPrincipal();
            Property(x => x.LockoutEndDateUtc).IsOptional();
            Property(x => x.PasswordHash).HasMaxLength(RestrictionConstants.SecurityStringLength)
                .IsRequired();
            Property(x => x.PasswordSalt).HasMaxLength(RestrictionConstants.SecurityStringLength)
                .IsRequired();
            Property(x => x.SecurityStamp).HasMaxLength(RestrictionConstants.SecurityStringLength)
                .IsRequired();
            Property(x => x.LockoutEnabled).IsRequired();
            Property(x => x.UserName).HasMaxLength(RestrictionConstants.ShortNameLength)
                .IsRequired();
            Property(x => x.AccessFailedCount).IsRequired();
            Property(x => x.Deleted).IsRequired();
        }
    }
}