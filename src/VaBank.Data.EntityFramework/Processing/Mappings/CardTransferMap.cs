using System.Data.Entity.ModelConfiguration;
using VaBank.Core.Processing.Entities;
using VaBank.Core.Transfers.Entities;

namespace VaBank.Data.EntityFramework.Processing.Mappings
{
    public class CardTransferMap: EntityTypeConfiguration<CardTransfer>
    {
        public CardTransferMap()
        {
            ToTable("CardTransfer", "Processing");
            HasKey(x => x.Id).Property(x => x.Id).HasColumnName("OperationID");
            Property(x => x.Type);

            HasRequired(x => x.SourceCard).WithMany().Map(x => x.MapKey("FromCardId"));
            HasRequired(x => x.DestinationCard).WithMany().Map(x => x.MapKey("ToCardId"));
        }
    }
}
