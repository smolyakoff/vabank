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
    public class CardAccountController : ApiController
    {
        private readonly IAccountManagementService _accountManagementService;

        public CardAccountController(IAccountManagementService accountManagementService)
        {
            if (accountManagementService == null)
            {
                throw new ArgumentNullException("accountManagementService");
            }
            _accountManagementService = accountManagementService;
        }

        [HttpGet]
        [Route("lookup")]
        public IHttpActionResult Lookup()
        {
            return Ok(_accountManagementService.GetAccountingLookup());
        }

        [HttpGet]
        [Route]
        public IHttpActionResult Get(AccountQuery query)
        {
            return Ok(_accountManagementService.GetCardAccounts(query));
        }

        [HttpPost]
        [Route]
        [Transaction]
        public IHttpActionResult Create(CreateCardAccountCommand command)
        {
            return Ok(_accountManagementService.CreateCardAccount(command));
        }

        [HttpPost]
        [Route("{id}/assign")]
        [Transaction]
        public IHttpActionResult AssignCard(SetCardAssignmentCommand command)
        {
            return Ok(_accountManagementService.SetCardAssignment(command));
        }

        [HttpPost]
        [Route("{id}/cards")]
        [Transaction]
        public IHttpActionResult CreateCard(CreateCardCommand command)
        {
            return Ok(_accountManagementService.CreateCard(command));
        }

        [HttpGet]
        [Route("{id}/cards")]
        public IHttpActionResult Cards([FromUri] IdentityQuery<string> accountNo)
        {
            return Ok(_accountManagementService.GetAccountCards(accountNo));
        }
        
    }
}
