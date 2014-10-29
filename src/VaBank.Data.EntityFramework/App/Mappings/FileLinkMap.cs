using System.Data.Entity.ModelConfiguration;
using VaBank.Core.App.Entities;

namespace VaBank.Data.EntityFramework.App.Mappings
{
    internal class FileLinkMap : EntityTypeConfiguration<FileLink>
    {
        public FileLinkMap()
        {
            Map(m => m.Requires("Type").HasValue("FileLink"));
            Property(x => x.Uri).IsRequired();
        }
    }
}
