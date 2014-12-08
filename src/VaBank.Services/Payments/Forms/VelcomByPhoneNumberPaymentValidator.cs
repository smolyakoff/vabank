using FluentValidation;
using Newtonsoft.Json.Linq;
using VaBank.Common.Validation;

namespace VaBank.Services.Payments.Forms
{
    [StaticValidator]
    [ValidatorName("payment-cell-velcom-phoneno")]
    public class VelcomByPhoneNumberPaymentValidator : ObjectValidator<JObject>
    {
        public VelcomByPhoneNumberPaymentValidator()
        {
            //TODO: write real rules
            RuleFor(x => x["amount"].Value<decimal>()).GreaterThan(0).WithName("Amount");
            RuleFor(x => x["phoneNo"].Value<string>()).NotEmpty().WithName("PhoneNo");
        }
    }
}
