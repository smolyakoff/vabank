using System;
using System.Web.Http;
using VaBank.Common.Data;
using VaBank.Common.Util;
using VaBank.Services.Contracts.Accounting;
using VaBank.Services.Contracts.Accounting.Commands;
using VaBank.Services.Contracts.Processing.Queries;
using VaBank.UI.Web.Api.Infrastructure.Filters;

namespace VaBank.UI.Web.Api.Customer
{
    [Authorize(Roles = "Customer")]
    public class CustomerCardController : ApiController
    {
        private readonly ICardAccountService _cardAccountService;

        public CustomerCardController(ICardAccountService cardAccountService)
        {
            if (cardAccountService == null)
            {
                throw new ArgumentNullException("cardAccountService");
            }
            _cardAccountService = cardAccountService;
        }

        [Route("api/users/{id:guid}/cards")]
        [HttpGet]
        public IHttpActionResult Get([FromUri]IdentityQuery<Guid> userId)
        {
            return Ok(_cardAccountService.GetCustomerCards(userId));
        }

        [Route("api/cards/{id:guid}/settings")]
        [HttpPut]
        [Transaction]
        public IHttpActionResult UpdateSettings([FromUri]Guid id, UpdateCardSettingsCommand command)
        {
            command.CardId = id;
            return Ok(_cardAccountService.UpdateCardSettings(command));
        }

        [Route("api/cards/{id:guid}/block")]
        [HttpPost]
        [Transaction]
        public IHttpActionResult Block([FromUri]Guid id, SetCardBlockCommand command)
        {
            command.CardId = id;
            return Ok(_cardAccountService.SetCardBlock(command));
        }

        [Route("api/cards/{id:guid}/statement")]
        [HttpGet]
        public IHttpActionResult Statement(Guid id, DateTime from, DateTime to)
        {
            var query = new CardAccountStatementQuery
            {
                CardId = id,
                DateRange = new Range<DateTime>(from.ToUniversalTime(), to.ToUniversalTime())
            };
            return Ok(_cardAccountService.GetCardAccountStatement(query));
        }
    }
}