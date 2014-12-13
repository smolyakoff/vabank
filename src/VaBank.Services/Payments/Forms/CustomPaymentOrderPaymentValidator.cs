using FluentValidation;
using VaBank.Common.Validation;

namespace VaBank.Services.Payments.Forms
{
    [ValidatorName("payment-custom-paymentorder")]
    public class CustomPaymentOrderPaymentValidator : ObjectValidator<CustomPaymentOrderPaymentValidator.Form>
    {
        public CustomPaymentOrderPaymentValidator()
        {
            RuleFor(x => x.Amount)
                   .GreaterThan(0)
                   .LessThan(5000000)
                   .WithLocalizedName(() => Names.Amount);
            RuleFor(x => x.BeneficiaryAccountNo)
                .NotEmpty()
                .Matches(@"^\d{13}$")
                .WithLocalizedName(() => Names.BeneficiaryAccountNo);
            RuleFor(x => x.BeneficiaryName)
                .NotEmpty()
                .Length(1, 256)
                .WithLocalizedName(() => Names.BeneficiaryName);
            RuleFor(x => x.BeneficiaryTIN)
                .NotEmpty()
                .Matches(@"^\d{9}$")
                .WithLocalizedName(() => Names.BeneficiaryTIN);
            RuleFor(x => x.BeneficiaryBankCode)
                .NotEmpty()
                .Matches(@"^\d{9}$")
                .WithLocalizedName(() => Names.BeneficiaryBankCode);
            RuleFor(x => x.PaymentCode)
                .Matches(@"^\d{4}")
                .When(x => x.PaymentCode == null)
                .WithLocalizedName(() => Names.PaymentCode);
            RuleFor(x => x.Purpose)
                .NotEmpty()
                .Length(1, 256)
                .WithLocalizedName(() => Names.Purpose);

        }

        public class Form
        {
            public decimal Amount { get; set; }

            public string BeneficiaryAccountNo { get; set; }

            public string BeneficiaryName { get; set; }

            public string BeneficiaryTIN { get; set; }

            public string BeneficiaryBankCode { get; set; }

            public string PaymentCode { get; set; }

            public string Purpose { get; set; }
        }
    }
}
