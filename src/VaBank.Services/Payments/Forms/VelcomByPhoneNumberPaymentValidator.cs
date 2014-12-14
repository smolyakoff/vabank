using FluentValidation;
using VaBank.Common.Validation;

namespace VaBank.Services.Payments.Forms
{
    [StaticValidator]
    [ValidatorName("payment-cell-velcom-phoneno")]
    internal class VelcomByPhoneNumberPaymentValidator : ObjectValidator<VelcomByPhoneNumberPaymentValidator.Form>
    {
        public VelcomByPhoneNumberPaymentValidator()
        {
            RuleFor(x => x.Amount)
                .GreaterThan(0)
                .LessThan(5000000)
                .WithLocalizedName(() => Names.Amount);
            RuleFor(x => x.PhoneNo)
                .NotEmpty()
                .Matches(@"^\(?(29|33|44|25)\)? *\d{7}$")
                .WithLocalizedName(() => Names.Phone)
                .WithLocalizedMessage(() => Messages.InvalidPhoneNo);
        }

        internal class Form
        {
            public decimal Amount { get; set; }

            public string PhoneNo { get; set; }
        }
    }
}
