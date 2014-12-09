using System.Data.Entity.ModelConfiguration;
using VaBank.Core.Payments.Entities;
using VaBank.Data.EntityFramework.Common;

namespace VaBank.Data.EntityFramework.Payments.Mappings
{
    internal class PaymentOrderMap : EntityTypeConfiguration<PaymentOrder>
    {
        public PaymentOrderMap()
        {
            ToTable("PaymentOrder", "Payments").HasKey(x => x.No);
            Property(x => x.PayerName).HasMaxLength(Restrict.Length.Name).IsRequired();
            Property(x => x.PayerBankCode).HasMaxLength(Restrict.Length.BankCode).IsRequired();
            Property(x => x.PayerAccountNo).HasMaxLength(Restrict.Length.AccountNo).IsRequired();
            Property(x => x.PayerTIN).HasMaxLength(Restrict.Length.TIN).IsRequired();
            Property(x => x.BeneficiaryName).HasMaxLength(Restrict.Length.Name).IsRequired();
            Property(x => x.BeneficiaryBankCode).HasMaxLength(Restrict.Length.BankCode).IsRequired();
            Property(x => x.BeneficiaryAccountNo).HasMaxLength(Restrict.Length.AccountNo).IsRequired();
            Property(x => x.BeneficiaryTIN).HasMaxLength(Restrict.Length.TIN).IsRequired();
            Property(x => x.Purpose).HasMaxLength(Restrict.Length.BigString).IsRequired();
            Property(x => x.Amount).IsRequired();
            Property(x => x.CurrencyISOName).HasMaxLength(Restrict.Length.CurrencyISO).IsRequired();
            Property(x => x.PaymentCode).HasMaxLength(Restrict.Length.PaymentCode).IsRequired();
        }
    }
}
