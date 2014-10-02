using System.Data.Entity.ModelConfiguration;
using VaBank.Core.Entities.Membership;

namespace VaBank.Data.EntityFramework.Mappings
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
        }
    }
}
