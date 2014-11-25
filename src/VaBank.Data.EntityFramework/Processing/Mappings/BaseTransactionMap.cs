using System.Data.Entity.ModelConfiguration;
using VaBank.Core.Processing.Entities;
using VaBank.Data.EntityFramework.Common;

namespace VaBank.Data.EntityFramework.Processing.Mappings
{
    public class BaseTransactionMap<TTransaction> : EntityTypeConfiguration<TTransaction>
        where TTransaction : class , ITransaction 
    {
        public BaseTransactionMap()
        {
            Property(x => x.TransactionAmount).IsRequired();
            Property(x => x.AccountAmount).IsRequired();
            Property(x => x.CreatedDateUtc).IsRequired();
            Property(x => x.PostDateUtc).IsOptional();
            Property(x => x.RemainingBalance).IsRequired();
            Property(x => x.Location).HasMaxLength(Restrict.Length.Name).IsRequired();
            Property(x => x.Description).HasMaxLength(Restrict.Length.BigString).IsOptional();
            Property(x => x.ErrorMessage).HasMaxLength(Restrict.Length.BigString).IsOptional();
            Property(x => x.Status).IsRequired();
        } 
    }
}
