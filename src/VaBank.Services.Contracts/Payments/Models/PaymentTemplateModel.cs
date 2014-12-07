using VaBank.Services.Contracts.Accounting.Models;

namespace VaBank.Services.Contracts.Payments.Models
{
    public class PaymentTemplateModel
    {
        public string Code { get; set; }

        public string Name { get; set; }

        public CurrencyModel Currency { get; set; }
    }
}
