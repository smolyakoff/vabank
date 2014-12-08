using FluentValidation;
using Newtonsoft.Json.Linq;
using VaBank.Common.Validation;

namespace VaBank.Services.Payments.Forms
{
    [ValidatorName("payment-custom-paymentorder")]
    public class CustomPaymentOrderPaymentValidator : AbstractValidator<JObject>
    {
        //TODO: write real rules
    }
}
