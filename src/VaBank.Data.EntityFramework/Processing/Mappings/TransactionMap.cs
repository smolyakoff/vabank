using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VaBank.Core.Processing.Entities;
using VaBank.Data.EntityFramework.Common;

namespace VaBank.Data.EntityFramework.Processing.Mappings
{
    public class TransactionMap : EntityTypeConfiguration<Transaction>
    {
        public TransactionMap()
        {
            ToTable("Transaction", "Processing");
            HasKey(x => x.Id).Property(x => x.Id).HasColumnName("TransactionID");
            //Ignore(x => x.AccountNo);
            HasRequired(x => x.Account).WithMany().Map(x => x.MapKey("AccountNo"));
            HasRequired(x => x.Currency).WithMany().Map(x => x.MapKey("CurrencyISOName"));
            Property(x => x.TransactionAmount).IsRequired();
            Property(x => x.AccountAmount).IsRequired();
            Property(x => x.CreatedDateUtc).IsRequired();
            Property(x => x.PostDateUtc).IsRequired();
            Property(x => x.RemainingBalance).IsRequired();
            Property(x => x.Location).HasMaxLength(Restrict.Length.Name).IsRequired();
            Property(x => x.Description).HasMaxLength(Restrict.Length.BigString).IsOptional();
            Property(x => x.ErrorMessage).HasMaxLength(Restrict.Length.BigString).IsOptional();
            Property(x => x.Status).IsRequired();
        }
    }
}
