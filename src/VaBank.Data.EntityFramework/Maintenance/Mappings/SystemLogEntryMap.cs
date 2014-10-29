using System.Data.Entity.ModelConfiguration;
using VaBank.Core.Maintenance;
using VaBank.Core.Maintenance.Entitities;
using VaBank.Data.EntityFramework.Common;
using VaBank.Data.EntityFramework.Membership.Mappings;

namespace VaBank.Data.EntityFramework.Maintenance.Mappings
{
    public class SystemLogEntryMap : EntityTypeConfiguration<SystemLogEntry>
    {
        public SystemLogEntryMap()
        {
            ToTable("SystemLog", "Maintenance");
            HasKey(t => t.Id);
            Property(t => t.Id).HasColumnName("EventId");
            Property(t => t.Application).HasMaxLength(Restrict.Length.ShortName).IsRequired();
            Property(t => t.Level).HasMaxLength(20).IsRequired();
            Property(t => t.TimeStampUtc).IsRequired();
            Property(t => t.Type).HasMaxLength(Restrict.Length.ShortName).IsRequired();
            Property(t => t.User).HasMaxLength(Restrict.Length.Name).IsOptional();
            Property(t => t.Message).IsRequired();
            Property(t => t.Exception).IsOptional();
            Property(t => t.Source).IsRequired();
        }
    }
}