using System.Data.Entity.ModelConfiguration;
using VaBank.Core.App;
using VaBank.Core.App.Entities;
using VaBank.Data.EntityFramework.Common;
using VaBank.Data.EntityFramework.Membership.Mappings;

namespace VaBank.Data.EntityFramework.App.Mappings
{
    public class ResourceMap : EntityTypeConfiguration<Resource>
    {
        public ResourceMap()
        {
            ToTable("Resource", "App");
            HasKey(x => x.Id);
            Property(x => x.Location).IsRequired().HasMaxLength(Restrict.Length.Name);
        }

    }
}
