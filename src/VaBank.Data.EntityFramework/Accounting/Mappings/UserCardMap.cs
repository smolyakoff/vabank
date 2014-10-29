using System.Data.Entity.ModelConfiguration;
using VaBank.Core.Accounting.Entities;

namespace VaBank.Data.EntityFramework.Accounting.Mappings
{
    public class UserCardMap : EntityTypeConfiguration<UserCard>
    {
        public UserCardMap()
        {
            ToTable("User_Card_Account", "Accounting");

            HasRequired(x => x.Owner).WithMany().Map(x => x.MapKey("UserID"));
            HasRequired(x => x.Settings).WithRequiredPrincipal();
        }
    }
}
