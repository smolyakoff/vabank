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

        private readonly IPaymentStatisticsService _paymentStatisticsService;

        public PaymentController(IPaymentClientService paymentService, IPaymentStatisticsService paymentStatisticsService)
        {
            Argument.NotNull(paymentService, "paymentService");
            Argument.NotNull(paymentStatisticsService, "paymentStatisticsService");
            _paymentService = paymentService;
            _paymentStatisticsService = paymentStatisticsService;
        }

        [HttpGet]
        [Route("api/users/{userId:guid}/payments")]
        public IHttpActionResult Query(PaymentArchiveQuery query)
        {
            return Ok(_paymentService.QueryArchive(query));
        }

        [HttpGet]
        [Route("api/users/{userId:guid}/payments/mostly-used")]
        public IHttpActionResult StatsMostlyUsed([FromUri]MostlyUsedPaymentsQuery query)
        {
            var stats = _paymentStatisticsService.GetMostlyUsedPayments(query);
            return Ok(stats);
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

        [HttpGet]
        [Route("api/payments/tree")]
        public IHttpActionResult GetPaymentsTree()
        {
            return Ok(_paymentService.GetPaymentsTree());
        }
    }
}
