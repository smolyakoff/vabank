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
    public class CardAccountController : ApiController
    {
        private readonly ICardAccountManagementService _cardAccountManagementService;

        public CardAccountController(ICardAccountManagementService cardAccountManagementService)
        {
            if (cardAccountManagementService == null)
            {
                throw new ArgumentNullException("cardAccountManagementService");
            }
            _cardAccountManagementService = cardAccountManagementService;
        }

        [HttpGet]
        [Route("lookup")]
        public IHttpActionResult Lookup()
        {
            return Ok(_cardAccountManagementService.GetAccountingLookup());
        }

        [HttpGet]
        [Route]
        public IHttpActionResult Get(AccountQuery query)
        {
            return Ok(_cardAccountManagementService.GetCardAccounts(query));
        }

        [HttpPost]
        [Route]
        [Transaction]
        public IHttpActionResult Create(CreateCardAccountCommand command)
        {
            return Ok(_cardAccountManagementService.CreateCardAccount(command));
        }

        [HttpPost]
        [Route("{id}/assign")]
        [Transaction]
        public IHttpActionResult AssignCard(SetCardAssignmentCommand command)
        {
            return Ok(_cardAccountManagementService.SetCardAssignment(command));
        }

        [HttpPost]
        [Route("{id}/cards")]
        [Transaction]
        public IHttpActionResult CreateCard(CreateCardCommand command)
        {
            return Ok(_cardAccountManagementService.CreateCard(command));
        }

        [HttpGet]
        [Route("{id}/cards")]
        public IHttpActionResult Cards([FromUri] IdentityQuery<string> accountNo)
        {
            return Ok(_cardAccountManagementService.GetAccountCards(accountNo));
        }
        
    }
}
