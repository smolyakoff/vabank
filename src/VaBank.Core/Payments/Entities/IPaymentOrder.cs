namespace VaBank.Core.Payments.Entities
{
    public interface IPaymentOrder
    {
        string PayerName { get; }

        string PayerBankCode { get; }

        string PayerAccountNo { get; }

        string PayerTIN { get; }

        string BeneficiaryName { get; }

        string BeneficiaryBankCode { get; }

        string BeneficiaryAccountNo { get; }

        string BeneficiaryTIN { get; }

        string Purpose { get; }

        decimal Amount { get; }

        string CurrencyISOName { get; }

        string PaymentCode { get; }
    }
}
