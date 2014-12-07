using System.Data.Entity.ModelConfiguration;
using VaBank.Core.Payments.Entities;
using VaBank.Data.EntityFramework.Common;

namespace VaBank.Data.EntityFramework.Payments.Mappings
{
    internal class BasePaymentOrderMap<TPaymentOrder> : EntityTypeConfiguration<TPaymentOrder>
        where TPaymentOrder : class, IPaymentOrder
    {
        public BasePaymentOrderMap()
        {
            Property(x => x.Amount).IsRequired();
            Property(x => x.BeneficiaryAccountNo).IsRequired().HasMaxLength(Restrict.Length.AccountNo);
            Property(x => x.BeneficiaryBankCode).IsRequired().HasMaxLength(Restrict.Length.BankCode);
            Property(x => x.BeneficiaryName).IsRequired().HasMaxLength(Restrict.Length.Name);
            Property(x => x.BeneficiaryTIN).IsRequired().HasMaxLength(Restrict.Length.TIN);
            Property(x => x.CurrencyISOName).IsRequired().HasMaxLength(Restrict.Length.CurrencyISO);
            Property(x => x.PayerAccountNo).IsRequired().HasMaxLength(Restrict.Length.AccountNo);
            Property(x => x.PayerBankCode).IsRequired().HasMaxLength(Restrict.Length.BankCode);
            Property(x => x.PayerName).IsRequired().HasMaxLength(Restrict.Length.Name);
            Property(x => x.PayerTIN).IsRequired().HasMaxLength(Restrict.Length.TIN);
            Property(x => x.PaymentCode).IsRequired().HasMaxLength(Restrict.Length.PaymentCode);
            Property(x => x.Purpose).IsRequired().HasMaxLength(Restrict.Length.BigString);
        }
    }
}
