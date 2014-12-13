using Newtonsoft.Json.Linq;

namespace VaBank.Services.Contracts.Payments.Models
{
    public class PaymentTemplateModel
    {
        public string Code { get; set; }

        public string HierarchicalName { get; set; }

        public JObject FormTemplate { get; set; }
    }
}
