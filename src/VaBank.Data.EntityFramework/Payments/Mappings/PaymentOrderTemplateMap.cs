using System.Data.Entity.ModelConfiguration;
using VaBank.Core.Payments.Entities;
using VaBank.Data.EntityFramework.Common;

namespace VaBank.Data.EntityFramework.Payments.Mappings
{
    internal class PaymentOrderTemplateMap : EntityTypeConfiguration<PaymentOrderTemplate>
    {
        public PaymentOrderTemplateMap()
        {
            ToTable("PaymentOrderTemplate", "Payments").HasKey(x => x.PaymentTemplateCode);
            HasRequired(x => x.PaymentTemplate).WithOptional();
            
            Property(x => x.PayerName).HasMaxLength(Restrict.Length.BigString).IsRequired();
            Property(x => x.PayerBankCode).HasMaxLength(Restrict.Length.BigString).IsRequired();
            Property(x => x.PayerAccountNo).HasMaxLength(Restrict.Length.BigString).IsRequired();
            Property(x => x.PayerTIN).HasMaxLength(Restrict.Length.BigString).IsRequired();
            Property(x => x.BeneficiaryName).HasMaxLength(Restrict.Length.BigString).IsRequired();
            Property(x => x.BeneficiaryBankCode).HasMaxLength(Restrict.Length.BigString).IsRequired();
            Property(x => x.BeneficiaryAccountNo).HasMaxLength(Restrict.Length.BigString).IsRequired();
            Property(x => x.BeneficiaryTIN).HasMaxLength(Restrict.Length.BigString).IsRequired();
            Property(x => x.Purpose).HasMaxLength(Restrict.Length.BigString).IsRequired();
            Property(x => x.Amount).IsRequired();
            Property(x => x.CurrencyISOName).HasMaxLength(Restrict.Length.BigString).IsRequired();
            Property(x => x.PaymentCode).HasMaxLength(Restrict.Length.BigString).IsRequired();
        }
    }
}
