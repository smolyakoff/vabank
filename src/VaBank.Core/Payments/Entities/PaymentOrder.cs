using VaBank.Core.Common;

namespace VaBank.Core.Payments.Entities
{
    public class PaymentOrder : Entity
    {
        internal PaymentOrder()
        {
        }

        internal PaymentOrder(
            long no,
            string payerName,
            string payerBankCode,
            string payerAccountNo,
            string payerTIN,
            string beneficiaryName,
            string beneficiaryBankCode,
            string beneficiaryAccountNo,
            string beneficiaryTIN,
            string purpose,
            decimal amount,
            string currencyISOName,
            string paymentCode
            )
        {
            No = no;
            PayerName = payerName;
            PayerBankCode = payerBankCode;
            PayerAccountNo = payerAccountNo;
            PayerTIN = payerTIN;
            BeneficiaryName = beneficiaryName;
            BeneficiaryBankCode = beneficiaryBankCode;
            BeneficiaryAccountNo = beneficiaryAccountNo;
            BeneficiaryTIN = beneficiaryTIN;
            Purpose = purpose;
            Amount = amount;
            CurrencyISOName = currencyISOName;
            PaymentCode = paymentCode;
        }

        public long No { get; protected set; }

        public string PayerName { get; internal set; }

        public string PayerBankCode { get; internal set; }

        public string PayerAccountNo { get; internal set; }

        public string PayerTIN { get; internal set; }

        public string BeneficiaryName { get; internal set; }

        public string BeneficiaryBankCode { get; internal set; }

        public string BeneficiaryAccountNo { get; internal set; }

        public string BeneficiaryTIN { get; internal set; }

        public string Purpose { get; internal set; }

        public decimal Amount { get; internal set; }

        public string CurrencyISOName { get; internal set; }

        public string PaymentCode { get; internal set; }
    }
}
