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
        private readonly ICardAccountManagementService _cardAccountManagementService;

        public CardController(ICardAccountManagementService cardAccountManagementService)
        {
            if (cardAccountManagementService == null)
            {
                throw new ArgumentNullException("cardAccountManagementService");
            }
            _cardAccountManagementService = cardAccountManagementService;
        }

        [HttpGet]
        [Route]
        public IHttpActionResult Get(CardQuery query)
        {
            return Ok(_cardAccountManagementService.GetUserCards(query));
        }
    }
}
