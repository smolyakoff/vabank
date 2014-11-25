using VaBank.Core.Processing.Entities;

namespace VaBank.Data.EntityFramework.Processing.Mappings
{
    public class TransactionMap : BaseTransactionMap<Transaction>
    {
        public TransactionMap()
        {
            ToTable("Transaction", "Processing");
            HasKey(x => x.Id).Property(x => x.Id).HasColumnName("TransactionID");
            HasRequired(x => x.Account).WithMany().HasForeignKey(x => x.AccountNo);
            HasRequired(x => x.Currency).WithMany().Map(x => x.MapKey("CurrencyISOName"));
        }
    }
}
