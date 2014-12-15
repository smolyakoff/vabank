using Newtonsoft.Json.Linq;

namespace VaBank.Services.Contracts.Payments.Models
{
    public class PaymentArchiveFormModel
    {
        public JObject Form { get; set; }

        public PaymentTemplateModel Template { get; set; }
    }
}
