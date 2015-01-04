using FluentValidation;
using VaBank.Common.Validation;

namespace VaBank.Services.Payments.Forms
{
    [ValidatorName("payment-internet-byfly")]
    [StaticValidator]
    internal class ByFlyInternetPaymentValidator : ObjectValidator<ByFlyInternetPaymentValidator.Form>
    {
        public ByFlyInternetPaymentValidator()
        {
            RuleFor(x => x.Amount)
                .GreaterThan(1000).WithLocalizedMessage(() => Messages.MinAmountPayment, 1000)
                .LessThanOrEqualTo(1000000).WithLocalizedMessage(() => Messages.MaxAmountPayment, 1000000);

            RuleFor(x => x.ContractNo)
                .Matches(@"^\d{13}$")
                .WithLocalizedMessage(() => Messages.InvalidByFlyContractNo);
        }

        internal class Form
        {
            public decimal Amount { get; set; }

            public string ContractNo { get; set; }
        }
    }
}
