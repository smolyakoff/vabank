using System;
using System.Web.Http;
using VaBank.Common.Util;
using VaBank.Services.Contracts.Accounting;
using VaBank.Services.Contracts.Accounting.Queries;

namespace VaBank.UI.Web.Api.Customer
{
    [RoutePrefix("api/accounts/card")]
    public class CustomerCardAccountController : ApiController
    {
        private readonly ICardAccountService _cardAccountService;

        public CustomerCardAccountController(ICardAccountService cardAccountService)
        {
            if (cardAccountService == null)
            {
                throw new ArgumentNullException("cardAccountService");
            }
            _cardAccountService = cardAccountService;
        }

        [Route("{accountNo:length(13)}/statement")]
        [HttpGet]
        public IHttpActionResult Statement(string accountNo, DateTime from, DateTime to)
        {
            var query = new CardAccountStatementQuery
            {
                AccountNo = accountNo,
                DateRange = new Range<DateTime>(from.ToUniversalTime(), to.ToUniversalTime())
            };
            return Ok(_cardAccountService.GetCardAccountStatement(query));
        }
    }
}