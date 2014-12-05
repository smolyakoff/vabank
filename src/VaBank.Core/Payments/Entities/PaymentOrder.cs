using VaBank.Core.Common;

namespace VaBank.Core.Payments.Entities
{
    public class PaymentOrder : Entity, IPaymentOrder
    {
        internal PaymentOrder()
        {
        }
        
        public long No { get; private set; }

        public string PayerName { get; private set; }

        public string PayerBankCode { get; private set; }

        public string PayerAccountNo { get; private set; }

        public string PayerTIN { get; private set; }

        public string BeneficiaryName { get; private set; }

        public string BeneficiaryBankCode { get; private set; }

        public string BeneficiaryAccountNo { get; private set; }

        public string BeneficiaryTIN { get; private set; }

        public string Purpose { get; private set; }

        public decimal Amount { get; private set; }

        public string CurrencyISOName { get; private set; }

        public string PaymentCode { get; private set; }
    }
}
