using FluentValidation;
using Newtonsoft.Json.Linq;
using VaBank.Common.Validation;

namespace VaBank.Services.Payments.Forms
{
    [ValidatorName("payment-custom-paymentorder")]
    public class CustomPaymentOrderPaymentValidator : AbstractValidator<JObject>
    {
        public CustomPaymentOrderPaymentValidator()
        {
            RuleFor(x => x["amount"].Value<decimal>())
                   .GreaterThan(0)
                   .LessThan(5000000)
                   .WithLocalizedName(() => Names.Amount)
                   .WithName("Amount");
            RuleFor(x => x["beneficiaryAccountNo"].Value<string>())
                .NotEmpty()
                .Matches(@"^\d{13}$")
                .WithLocalizedName(() => Names.BeneficiaryAccountNo)
                .WithName("BeneficiaryAccountNo");
            RuleFor(x => x["beneficiaryName"].Value<string>())
                .NotEmpty()
                .WithLocalizedName(() => Names.BeneficiaryName)
                .WithName("BeneficiaryName");
            RuleFor(x => x["beneficiayTIN"].Value<string>())
                .NotEmpty()
                .Matches(@"^\d{9}$")
                .WithLocalizedName(() => Names.BeneficiayTIN)
                .WithName("BeneficiayTIN");
            RuleFor(x => x["beneficiaryBankCode"].Value<string>())
                .NotEmpty()
                .Matches(@"^\d{9}$")
                .WithLocalizedName(() => Names.BeneficiaryBankCode)
                .WithName("BeneficiaryBankCode");
            RuleFor(x => x["purpose"].Value<string>())
                .NotEmpty()
                .Length(256)
                .WithLocalizedName(() => Names.Purpose)
                .WithName("Purpose");

        }
    }
}
