using System;
using System.Web.Http;
using VaBank.Services.Contracts.Accounting;
using VaBank.Services.Contracts.Accounting.Queries;

namespace VaBank.UI.Web.Api.Admin
{
    [RoutePrefix("api/cards")]
    public class UserCardController : ApiController
    {
        private readonly IAccountManagementService _accountManagementService;

        public UserCardController(IAccountManagementService accountManagementService)
        {
            if (accountManagementService == null)
            {
                throw new ArgumentNullException("accountManagementService");
            }
            _accountManagementService = accountManagementService;
        }

        [HttpGet]
        [Route]
        public IHttpActionResult Get(CardQuery query)
        {
            return Ok(_accountManagementService.GetOwnedCards(query));
        }

    }
}
