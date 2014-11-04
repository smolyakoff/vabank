using System;
using System.Web.Http;
using VaBank.Services.Contracts.Accounting;
using VaBank.Services.Contracts.Accounting.Queries;

namespace VaBank.UI.Web.Api.Admin
{
    [RoutePrefix("api/cards")]
    [Authorize(Roles = "Admin")]
    public class CardController : ApiController
    {
        private readonly ICardAccountService _cardAccountService;

        public CardController(ICardAccountService cardAccountService)
        {
            if (cardAccountService == null)
            {
                throw new ArgumentNullException("cardAccountService");
            }
            _cardAccountService = cardAccountService;
        }

        [HttpGet]
        [Route]
        public IHttpActionResult Get(CardQuery query)
        {
            return Ok(_cardAccountService.GetUserCards(query));
        }
    }
}
