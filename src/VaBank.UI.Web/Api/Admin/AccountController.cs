using System;
using System.Web.Http;
using VaBank.Common.Data;
using VaBank.Services.Contracts.Accounting;
using VaBank.Services.Contracts.Accounting.Queries;

namespace VaBank.UI.Web.Api.Admin
{
    [RoutePrefix("api/accounts")]
    public class AccountController : ApiController
    {
        private readonly IAccountManagementService _accountManagementService;

        public AccountController(IAccountManagementService accountManagementService)
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

        [HttpGet]
        [Route("{id}/cards")]
        public IHttpActionResult Cards([FromUri] IdentityQuery<string> accountNo)
        {
            return Ok(_accountManagementService.GetAccountCards(accountNo));
        }
        
    }
}
