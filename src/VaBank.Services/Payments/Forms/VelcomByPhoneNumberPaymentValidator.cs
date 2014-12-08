using FluentValidation;
using VaBank.Core.Membership.Resources;
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
            RuleFor(x => x["amount"].Value<decimal>())
                .GreaterThan(0)
                .LessThan(5000000)
                .WithLocalizedName(() => Names.Amount)
                .WithName("Amount");
            RuleFor(x => x["phoneNo"].Value<string>())
                .NotEmpty()
                .Matches(@"^\(?(29|33|44|25)\)? *\d{7}$")
                .WithLocalizedName(() => Names.Phone)
                .WithLocalizedMessage(() => Messages.CheckNumberPhone)
                .WithName("PhoneNo");
        }
    }
}
