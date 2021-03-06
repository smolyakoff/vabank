﻿using System.Data.Entity.ModelConfiguration;
using VaBank.Core.Processing.Entities;

namespace VaBank.Data.EntityFramework.Processing.Mappings
{
    public class TransferMap: EntityTypeConfiguration<Transfer>
    {
        public TransferMap()
        {
            ToTable("Transfer", "Processing");
            HasKey(x => x.Id);

            Property(x => x.Amount).IsRequired();
            Property(x => x.Id).HasColumnName("OperationID");
            
            HasRequired(x => x.Currency).WithMany().Map(x => x.MapKey("CurrencyISOName"));
            HasRequired(x => x.From).WithMany().Map(x => x.MapKey("FromAccountNo"));
            HasRequired(x => x.To).WithMany().Map(x => x.MapKey("ToAccountNo"));
            HasOptional(x => x.Withdrawal).WithMany().Map(x => x.MapKey("FromTransactionID"));
            HasOptional(x => x.Deposit).WithMany().Map(x => x.MapKey("ToTransactionID"));
        }
    }
}
