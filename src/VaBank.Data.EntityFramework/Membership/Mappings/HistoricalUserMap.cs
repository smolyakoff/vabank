using System.Data.Entity.ModelConfiguration;
using VaBank.Core.Membership.Entities;

namespace VaBank.Data.EntityFramework.Membership.Mappings
{
    internal class HistoricalUserMap : BaseUserMap<HistoricalUser>
    {
        public HistoricalUserMap()
        {
            ToTable("User_History", "Membership").HasKey(x => x.HistoryId);
            Property(x => x.Id).HasColumnName("UserID");
            Property(x => x.HistoryOperationId).IsRequired();
            Property(x => x.HistoryTimestampUtc).IsRequired();
        }
    }
}
