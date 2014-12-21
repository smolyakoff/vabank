using System.Web.Http;
using VaBank.Common.Validation;
using VaBank.Services.Contracts.Accounting;
using VaBank.Services.Contracts.Accounting.Commands;
using VaBank.UI.Web.Api.Infrastructure.Filters;

namespace VaBank.UI.Web.Api.Admin
{
    [Authorize(Roles = "Admin")]
    [RoutePrefix("api/cards")]
    public class CardAdminController : ApiController
    {
        private readonly ICardAccountService _cardAccountService;

        public CardAdminController(ICardAccountService cardAccountService)
        {
            Argument.NotNull(cardAccountService, "cardAccountService");
            _cardAccountService = cardAccountService;
        }

        [HttpPost]
        [Route("{cardId:guid}/activate")]
        [Transaction]
        public IHttpActionResult ActivateCard(SetCardActivationCommand command)
        {
            return Ok(_cardAccountService.SetCardActivation(command));
        }
    }
}