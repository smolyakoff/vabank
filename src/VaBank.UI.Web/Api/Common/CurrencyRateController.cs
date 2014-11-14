using System;
using System.Web.Http;
using VaBank.Services.Contracts.Processing;

namespace VaBank.UI.Web.Api.Common
{
    [RoutePrefix("api/exchange-rates")]
    public class CurrencyRateController : ApiController
    {
        private readonly ICurrencyRateService _currencyRateService;

        public CurrencyRateController(ICurrencyRateService currencyRateService)
        {
            if (currencyRateService == null)
            {
                throw new ArgumentNullException("currencyRateService");
            }
            _currencyRateService = currencyRateService;
        }

        [HttpGet]
        [Route]
        public IHttpActionResult GetAll()
        {
            return Ok(_currencyRateService.GetLocalCurrencyRates());
        }
    }
}
