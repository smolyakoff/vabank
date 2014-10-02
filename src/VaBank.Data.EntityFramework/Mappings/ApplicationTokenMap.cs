using System.Data.Entity.ModelConfiguration;
using VaBank.Core.Entities.Membership;

namespace VaBank.Data.EntityFramework.Mappings
{
    internal class ApplicationTokenMap : EntityTypeConfiguration<ApplicationToken>
    {
        public ApplicationTokenMap()
        {
            ToTable("ApplicationToken", "Membership").HasKey(x => x.Id)
                .HasRequired(x => x.Client).WithRequiredPrincipal().Map(x => x.MapKey("ClientID"));
        }
    }
}
