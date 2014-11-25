using VaBank.Core.Processing.Entities;

namespace VaBank.Data.EntityFramework.Processing.Mappings
{
    public class HistoricalTransactionMap : BaseTransactionMap<HistoricalTransaction>
    {
        public HistoricalTransactionMap()
        {
            ToTable("Transaction_History", "Processing");
            HasKey(x => x.HistoryId);
            Property(x => x.Id).HasColumnName("TransactionID").IsRequired();
            Property(x => x.HistoryId).IsRequired();
            Property(x => x.HistoryTimestampUtc).IsRequired();
            Property(x => x.HistoryAction).IsFixedLength().IsUnicode(false);

            HasRequired(x => x.HistoryOperation).WithMany().HasForeignKey(x => x.HistoryOperationId);
        }
    }
}
