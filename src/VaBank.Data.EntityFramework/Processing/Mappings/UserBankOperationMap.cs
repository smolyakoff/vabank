using System.Data.Entity.ModelConfiguration;
using VaBank.Core.Processing.Entities;

namespace VaBank.Data.EntityFramework.Processing.Mappings
{
    public class UserBankOperationMap : EntityTypeConfiguration<UserBankOperation>
    {
        public UserBankOperationMap()
        {
            ToTable("Operation_User", "Processing");
            HasKey(x => x.OperationId).Property(x => x.OperationId).HasColumnName("OperationID");

            HasRequired(x => x.Operation).WithOptional();
            HasRequired(x => x.User).WithMany().Map(x => x.MapKey("UserID"));
        }
    }
}
