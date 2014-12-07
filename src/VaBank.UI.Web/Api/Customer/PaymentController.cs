using System.Web.Http;
using VaBank.Common.Data;
using VaBank.Common.Validation;
using VaBank.Services.Contracts.Payments;
using VaBank.Services.Contracts.Payments.Commands;

namespace VaBank.UI.Web.Api.Customer
{
    [RoutePrefix("api/payments")]
    public class PaymentController : ApiController
    {
        private readonly IPaymentClientService _paymentService;

        public PaymentController(IPaymentClientService paymentService)
        {
            Argument.NotNull(paymentService, "paymentService");
            _paymentService = paymentService;
        }

        [HttpGet]
        [Route("{code}/template")]
        public IHttpActionResult GetTemplate(string code)
        {
            var id = new IdentityQuery<string>(code);
            var template = _paymentService.GetTemplate(id);
            return Ok(template);
        }

        [HttpPost]
        [Route("{code}/validate")]
        public IHttpActionResult Validate(ValidateFormCommand command)
        {
            var result = _paymentService.Validate(command);
            if (result == null || !result.IsValidatorFound)
            {
                return NotFound();
            }
            return Ok(new
            {
                Faults = result.ValidationFaults, 
                IsValid = result.IsValid
            });
        }
    }
}
