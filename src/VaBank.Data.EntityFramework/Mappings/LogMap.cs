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
            ToTable("Logs");
            HasKey(t => t.Id);
            HasRequired<string>(t => t.Application);
            HasRequired<string>(t => t.Level);
            Property(t => t.TimeStampUtc).IsRequired();
            HasRequired<string>(t => t.Type);
            HasRequired<string>(t => t.User);
            HasRequired<string>(t => t.Message);
            HasRequired<string>(t => t.Exception);
            HasRequired<string>(t => t.Source);

        }
    }
}
