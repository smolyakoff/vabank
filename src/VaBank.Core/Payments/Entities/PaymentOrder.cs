using VaBank.Core.Common;

namespace VaBank.Core.Payments.Entities
{
    public class PaymentOrder : Entity, IPaymentOrder
    {
        protected PaymentOrder()
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

        public string PayerName { get; protected set; }

        public string PayerBankCode { get; protected set; }

        public string PayerAccountNo { get; protected set; }

        public string PayerTIN { get; protected set; }

        public string BeneficiaryName { get; protected set; }

        public string BeneficiaryBankCode { get; protected set; }

        public string BeneficiaryAccountNo { get; protected set; }

        public string BeneficiaryTIN { get; protected set; }

        public string Purpose { get; protected set; }

        public decimal Amount { get; protected set; }

        public string CurrencyISOName { get; protected set; }

        public string PaymentCode { get; protected set; }
    }
}
