using VaBank.Common.Validation;

namespace VaBank.Services.Payments.Forms
{
    [StaticValidator]
    [ValidatorName("payment-cell-velcom-phoneno")]
    internal class VelcomByPhoneNumberPaymentValidator : ObjectValidator<VelcomByPhoneNumberPaymentValidator.Form>
    {
        public VelcomByPhoneNumberPaymentValidator()
        {
            //TODO: write real rules
        }

        internal class Form
        {
             public decimal Amount { get; set; }

             public string PhoneNo { get; set; }
        }
    }
}
