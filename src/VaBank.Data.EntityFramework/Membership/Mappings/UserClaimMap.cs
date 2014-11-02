using System.Data.Entity.ModelConfiguration;
using VaBank.Core.Membership;
using VaBank.Core.Membership.Entities;
using VaBank.Data.EntityFramework.Common;

namespace VaBank.Data.EntityFramework.Membership.Mappings
{
    internal class UserClaimMap : EntityTypeConfiguration<UserClaim>
    {
        public UserClaimMap()
        {
            ToTable("UserClaim", "Membership").HasKey(x => new { x.UserId, x.Type, x.Value });
            Property(x => x.Type).IsRequired().IsUnicode(false);
            Property(x => x.Value).HasMaxLength(512).IsRequired();
        }
    }
}