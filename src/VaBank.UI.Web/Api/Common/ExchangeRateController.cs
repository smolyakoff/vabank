using System;
using System.Web.Http;
using VaBank.Services.Contracts.Processing;

namespace VaBank.UI.Web.Api.Common
{
    [RoutePrefix("api/exchange-rates")]
    public class ExchangeRateController : ApiController
    {
        private readonly IExchangeRateService _exchangeRateService;

        public ExchangeRateController(IExchangeRateService exchangeRateService)
        {
            if (exchangeRateService == null)
            {
                throw new ArgumentNullException("exchangeRateService");
            }
            _exchangeRateService = exchangeRateService;
        }

        [HttpGet]
        [Route]
        public IHttpActionResult GetAll()
        {
            return Ok(_exchangeRateService.GetLocalCurrencyRates());
        }
    }
}
