using System.Data.Entity.ModelConfiguration;
using VaBank.Core.App.Entities;
using VaBank.Data.EntityFramework.Common;

namespace VaBank.Data.EntityFramework.App.Mappings
{
    public class SecurityCodeMap : EntityTypeConfiguration<SecurityCode>
    {
        public SecurityCodeMap()
        {
            ToTable("SecurityCode", "App");
            HasKey(x => x.Id);

            Property(x => x.ExpirationDateUtc).IsRequired();
            Property(x => x.CodeHash).IsRequired().HasMaxLength(Restrict.Length.SecurityString);
            Property(x => x.IsActive).IsRequired();
        }
    }
}
