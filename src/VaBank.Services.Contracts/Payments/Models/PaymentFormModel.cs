using Newtonsoft.Json.Linq;

namespace VaBank.Services.Contracts.Payments.Models
{
    public class PaymentFormModel
    {
        public JObject Form { get; set; }

        public PaymentTemplateModel Template { get; set; }
    }
}
