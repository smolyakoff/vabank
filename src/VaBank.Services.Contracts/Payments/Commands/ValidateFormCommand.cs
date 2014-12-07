using Newtonsoft.Json.Linq;

namespace VaBank.Services.Contracts.Payments.Commands
{
    public class ValidateFormCommand
    {
        public string Code { get; set; }

        public JObject Form { get; set; }
    }
}
