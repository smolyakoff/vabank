using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using VaBank.Core.App;
using VaBank.Core.App.Entities;
using VaBank.Data.EntityFramework.Common;

namespace VaBank.Data.EntityFramework.App.Mappings
{
    internal class OperationMap : EntityTypeConfiguration<Operation>
    {
        public OperationMap()
        {
            ToTable("Operation", "App");
            HasKey(x => x.Id);
            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.StartedUtc);
            Property(x => x.FinishedUtc).IsOptional();
            Property(x => x.Name).HasMaxLength(Restrict.Length.Name);
            Property(x => x.UserId).HasColumnName("AppUserId").IsOptional();
            Property(x => x.ClientApplicationId).HasColumnName("AppClientId").HasMaxLength(Restrict.Length.Name).IsOptional();

            HasOptional(x => x.User).WithMany().HasForeignKey(x => x.UserId);
            HasOptional(x => x.ApplicationClient).WithMany().HasForeignKey(x => x.ClientApplicationId);
        }
    }
}
