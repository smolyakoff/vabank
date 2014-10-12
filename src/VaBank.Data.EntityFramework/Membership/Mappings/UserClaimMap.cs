using System.Data.Entity.ModelConfiguration;
using VaBank.Core.Membership;

namespace VaBank.Data.EntityFramework.Membership.Mappings
{
    internal class UserClaimMap : EntityTypeConfiguration<UserClaim>
    {
        public UserClaimMap()
        {
            ToTable("UserClaim", "Membership").HasKey(x => new {x.UserId, x.Type});
            Property(x => x.Value).HasMaxLength(RestrictionConstants.BigStringLength)
                .IsRequired();
        }
    }
}