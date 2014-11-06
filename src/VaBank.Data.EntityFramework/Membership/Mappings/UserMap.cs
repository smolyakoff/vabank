using System.Data.Entity.ModelConfiguration;
using VaBank.Core.Membership.Entities;

namespace VaBank.Data.EntityFramework.Membership.Mappings
{
    internal class UserMap : BaseUserMap<User>
    {
        public UserMap()
        {            
            ToTable("User", "Membership").HasKey(x => x.Id)
                .HasMany(x => x.Claims).WithRequired()
                .HasForeignKey(x => x.UserId);
            HasRequired(x => x.Profile).WithRequiredPrincipal();
            Property(x => x.Id).HasColumnName("UserID");            
            Property(x => x.RowVersion).IsRowVersion();
        }
    }
}