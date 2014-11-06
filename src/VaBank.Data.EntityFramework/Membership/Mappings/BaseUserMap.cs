using System.Data.Entity.ModelConfiguration;
using VaBank.Core.Membership.Entities;
using VaBank.Data.EntityFramework.Common;

namespace VaBank.Data.EntityFramework.Membership.Mappings
{
    internal class BaseUserMap<TUser> : EntityTypeConfiguration<TUser>
        where TUser : class, IUser
    {
        public BaseUserMap()
        {
            Property(x => x.LockoutEndDateUtc).IsOptional();
            Property(x => x.PasswordHash).HasMaxLength(Restrict.Length.SecurityString)
                .IsRequired();
            Property(x => x.PasswordSalt).HasMaxLength(Restrict.Length.SecurityString)
                .IsRequired();
            Property(x => x.LockoutEnabled).IsRequired();
            Property(x => x.UserName).HasMaxLength(Restrict.Length.ShortName)
                .IsRequired();
            Property(x => x.AccessFailedCount).IsRequired();
            Property(x => x.Deleted).IsRequired();
        }
    }
}
