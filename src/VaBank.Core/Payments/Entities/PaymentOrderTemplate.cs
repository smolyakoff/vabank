using VaBank.Core.Common;

namespace VaBank.Core.Payments.Entities
{
    public class PaymentOrderTemplate : Entity, IPaymentOrder
    {
        internal PaymentOrderTemplate()
        {
        }

        public string PaymentTemplateCode { get; private set; }

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
