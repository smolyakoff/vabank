using VaBank.Core.Common;

namespace VaBank.Core.Payments.Entities
{
    public class PaymentOrder : Entity, IPaymentOrder
    {
        internal PaymentOrder()
        {
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
