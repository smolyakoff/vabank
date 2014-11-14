using System;
using System.Web.Http;
using VaBank.Common.Data;
using VaBank.Services.Contracts.Accounting;
using VaBank.Services.Contracts.Accounting.Commands;
using VaBank.Services.Contracts.Accounting.Queries;
using VaBank.UI.Web.Api.Infrastructure.Filters;

namespace VaBank.UI.Web.Api.Admin
{
    [RoutePrefix("api/accounts/card")]
    [Authorize(Roles = "Admin")]
    public class CardAccountAdminController : ApiController
    {
        private readonly ICardAccountService _cardAccountService;

        public CardAccountAdminController(ICardAccountService cardAccountService)
        {
            if (cardAccountService == null)
            {
                throw new ArgumentNullException("cardAccountService");
            }
            _cardAccountService = cardAccountService;
        }

        [HttpGet]
        [Route("lookup")]
        public IHttpActionResult Lookup()
        {
            return Ok(_cardAccountService.GetAccountingLookup());
        }

        [HttpGet]
        [Route]
        public IHttpActionResult Get(AccountQuery query)
        {
            return Ok(_cardAccountService.GetCardAccounts(query));
        }

        [HttpPost]
        [Route]
        [Transaction]
        public IHttpActionResult Create(CreateCardAccountCommand command)
        {
            return Ok(_cardAccountService.CreateCardAccount(command));
        }

        [HttpPost]
        [Route("{id}/assign")]
        [Transaction]
        public IHttpActionResult AssignCard(SetCardAssignmentCommand command)
        {
            return Ok(_cardAccountService.SetCardAssignment(command));
        }

        [HttpPost]
        [Route("{id}/cards")]
        [Transaction]
        public IHttpActionResult CreateCard(CreateCardCommand command)
        {
            return Ok(_cardAccountService.CreateCard(command));
        }

        [HttpGet]
        [Route("{id}/cards")]
        public IHttpActionResult Cards([FromUri] IdentityQuery<string> accountNo)
        {
            return Ok(_cardAccountService.GetAccountCards(accountNo));
        }
        
    }
}
