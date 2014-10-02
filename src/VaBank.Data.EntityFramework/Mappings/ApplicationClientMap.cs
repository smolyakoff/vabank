using System.Data.Entity.ModelConfiguration;
using VaBank.Core.Entities.Membership;

namespace VaBank.Data.EntityFramework.Mappings
{
    internal class ApplicationClientMap : EntityTypeConfiguration<ApplicationClient>
    {
        public ApplicationClientMap()
        {
            ToTable("ApplicationClient", "Membership").HasKey(x => x.Id);
        }
    }
}
