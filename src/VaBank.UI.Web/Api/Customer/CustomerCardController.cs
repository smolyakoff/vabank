using System;
using System.Web.Http;
using VaBank.Common.Data;
using VaBank.Services.Contracts.Accounting;

namespace VaBank.UI.Web.Api.Customer
{
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

        [Route("api/users/{id}/cards")]
        public IHttpActionResult Get([FromUri]IdentityQuery<Guid> userId)
        {
            return Ok(_cardAccountManagementService.GetCustomerCards(userId));
        }
    }
}