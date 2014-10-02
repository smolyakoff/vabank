using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VaBank.Core.Entities;

namespace VaBank.Data.EntityFramework.Mappings
{
    public class LogMap: EntityTypeConfiguration<Log>
    {
        public LogMap()
        {
            ToTable("SystemLog", "Maintenance");
            HasKey(t => t.Id);
            Property(t => t.Id).HasColumnName("EventId");
            Property(t => t.Application);
            Property(t => t.Level);
            Property(t => t.TimeStampUtc).IsRequired();
            Property(t => t.Type);
            Property(t => t.User);
            Property(t => t.Message);
            Property(t => t.Exception);
            Property(t => t.Source);

        }
    }
}
