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
                   .WithLocalizedName(() => Names.Amount);
            RuleFor(x => x["beneficiaryAccountNo"].Value<string>())
                .NotEmpty()
                .Matches(@"^\d{13}$")
                .WithLocalizedName(() => Names.BeneficiaryAccountNo);
            RuleFor(x => x["beneficiaryName"].Value<string>())
                .NotEmpty()
                .WithLocalizedName(() => Names.BeneficiaryName);
            RuleFor(x => x["beneficiaryTIN"].Value<string>())
                .NotEmpty()
                .Matches(@"^\d{9}$")
                .WithLocalizedName(() => Names.BeneficiaryTIN);
            RuleFor(x => x["beneficiaryBankCode"].Value<string>())
                .NotEmpty()
                .Matches(@"^\d{9}$")
                .WithLocalizedName(() => Names.BeneficiaryBankCode);
            RuleFor(x => x["purpose"].Value<string>())
                .NotEmpty()
                .Length(256)
                .WithLocalizedName(() => Names.Purpose);

        }

        internal class Form
        {
            public decimal Amount { get; set; }

            public string BeneficiaryAccountNo { get; set; }

            public string BeneficiaryName { get; set; }

            public string BeneficiaryTIN { get; set; }

            public string BeneficiaryBankCode { get; set; }

            public string Purpose { get; set; }
        }
    }
}
