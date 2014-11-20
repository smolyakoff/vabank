using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VaBank.Core.Processing.Entities;

namespace VaBank.Data.EntityFramework.Processing.Mappings
{
    public class CardTransferMap: EntityTypeConfiguration<CardTransfer>
    {
        public CardTransferMap()
        {
            ToTable("CardTransfer", "Processing");
            HasKey(x => x.Id).Property(x => x.Id).HasColumnName("OperationID");
            Property(x => x.Type);
        }
    }
}
