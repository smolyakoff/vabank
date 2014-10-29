using System;
using System.Web.Http;
using VaBank.Common.Data;
using VaBank.Services.Contracts.Accounting;
using VaBank.Services.Contracts.Accounting.Commands;
using VaBank.UI.Web.Api.Infrastructure.Filters;

namespace VaBank.UI.Web.Api.Customer
{
    [Authorize(Roles = "Customer")]
    public class CustomerCardController : ApiController
    {
        private readonly ICardAccountManagementService _cardAccountManagementService;

        public CustomerCardController(ICardAccountManagementService cardAccountManagementService)
        {
            if (cardAccountManagementService == null)
            {
                throw new ArgumentNullException("cardAccountManagementService");
            }
            _cardAccountManagementService = cardAccountManagementService;
        }

        [Route("api/users/{id:guid}/cards")]
        [HttpGet]
        public IHttpActionResult Get([FromUri]IdentityQuery<Guid> userId)
        {
            return Ok(_cardAccountManagementService.GetCustomerCards(userId));
        }

        [Route("api/cards/{id:guid}/settings")]
        [HttpPut]
        [Transaction]
        public IHttpActionResult UpdateSettings([FromUri]Guid id, UpdateCardSettingsCommand command)
        {
            command.CardId = id;
            return Ok(_cardAccountManagementService.UpdateCardSettings(command));
        }

        [Route("api/cards/{id:guid}/block")]
        [HttpPost]
        [Transaction]
        public IHttpActionResult Block([FromUri]Guid id, SetCardBlockCommand command)
        {
            command.CardId = id;
            return Ok(_cardAccountManagementService.SetCardBlock(command));
        }
    }
}