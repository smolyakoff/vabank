namespace VaBank.Services.Contracts.Payments.Models
{
    public class PaymentOrderModel
    {
        public string No { get;  set; }

        public string PayerName { get;  set; }

        public string PayerBankCode { get;  set; }

        public string PayerAccountNo { get;  set; }

        public string PayerTIN { get;  set; }

        public string BeneficiaryName { get;  set; }

        public string BeneficiaryBankCode { get;  set; }

        public string BeneficiaryAccountNo { get;  set; }

        public string BeneficiaryTIN { get;  set; }

        public string Purpose { get;  set; }

        public decimal Amount { get;  set; }

        public string CurrencyISOName { get;  set; }

        public string PaymentCode { get;  set; }
    }
}
