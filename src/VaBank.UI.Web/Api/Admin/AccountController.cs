using System;
using System.Web.Http;
using VaBank.Services.Contracts.Accounting;

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
    }
}
