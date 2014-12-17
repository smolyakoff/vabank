using System.Web.Http;
using VaBank.Common.Data;
using VaBank.Common.Validation;
using VaBank.Services.Contracts.Payments;
using VaBank.Services.Contracts.Payments.Commands;
using VaBank.Services.Contracts.Payments.Queries;

namespace VaBank.UI.Web.Api.Customer
{
    [Authorize(Roles = "Customer")]
    public class PaymentController : ApiController
    {
        private readonly IPaymentClientService _paymentService;

        public PaymentController(IPaymentClientService paymentService)
        {
            Argument.NotNull(paymentService, "paymentService");
            _paymentService = paymentService;
        }

        [HttpGet]
        [Route("api/users/{userId:guid}/payments")]
        public IHttpActionResult Query(PaymentArchiveQuery query)
        {
            return Ok(_paymentService.QueryArchive(query));
        }

        [HttpGet]
        [Route("api/payments/{id:long}")]
        public IHttpActionResult Get([FromUri] IdentityQuery<long> query)
        {
            var payment = _paymentService.GetArchiveDetails(query);
            return payment == null ? (IHttpActionResult)NotFound() : Ok(payment);
        }

        [HttpGet]
        [Route("api/payments/{id:long}/form")]
        public IHttpActionResult GetForm([FromUri] IdentityQuery<long> query)
        {
            var payment = _paymentService.GetFormWithTemplate(query);
            return payment == null ? (IHttpActionResult)NotFound() : Ok(payment);
        }

        [HttpGet]
        [Route("api/payment-templates/{code}")]
        public IHttpActionResult GetTemplate(string code)
        {
            var id = new IdentityQuery<string>(code);
            var template = _paymentService.GetTemplate(id);
            return Ok(template);
        }

        [HttpPost]
        [Route("api/payments")]
        public IHttpActionResult Submit(SubmitPaymentCommand command)
        {
            return Ok(_paymentService.Submit(command));
        }
    }
}
