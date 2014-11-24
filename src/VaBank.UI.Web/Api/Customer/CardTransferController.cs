using System.Net;
using System.Web.Http;
using VaBank.Common.Validation;
using VaBank.Services.Contracts.Processing;
using VaBank.Services.Contracts.Processing.Commands;
using VaBank.UI.Web.Api.Infrastructure.Filters;

namespace VaBank.UI.Web.Api.Customer
{
    [Authorize(Roles = "Customer")]
    [RoutePrefix("api/transfers")]
    public class CardTransferController : ApiController
    {
        private readonly ICardTransferClientService _transferService;

        public CardTransferController(ICardTransferClientService transferService)
        {
            Argument.NotNull(transferService, "transferService");
            _transferService = transferService;
        }

        [HttpGet]
        [Route("lookup")]
        public IHttpActionResult Lookup()
        {
            return Ok(_transferService.GetLookup());
        }

        [HttpPost]
        [Route("personal")]
        [Transaction]
        public IHttpActionResult Personal(PersonalCardTransferCommand command)
        {
            _transferService.Transfer(command);
            return StatusCode(HttpStatusCode.Accepted);
        }

        [HttpPost]
        [Route("interbank")]
        [Transaction]
        public IHttpActionResult Interbank(InterbankCardTransferCommand command)
        {
            _transferService.Transfer(command);
            return StatusCode(HttpStatusCode.Accepted);
        }
    }
}