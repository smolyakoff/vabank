using System.Data.Entity.ModelConfiguration;
using VaBank.Core.Accounting.Entities;

namespace VaBank.Data.EntityFramework.Accounting.Mappings
{
    public class UserAccountMap : EntityTypeConfiguration<UserAccount>
    {
        public UserAccountMap()
        {
            ToTable("User_Account", "Accounting");

            HasRequired(x => x.Owner).WithMany().Map(x => x.MapKey("UserID"));
        }
    }
}
