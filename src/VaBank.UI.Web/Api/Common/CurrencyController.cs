using System;
using System.Web.Http;
using VaBank.Services.Contracts.Processing;

namespace VaBank.UI.Web.Api.Common
{
    [RoutePrefix("api/currencies")]
    public class CurrencyController : ApiController
    {
        private readonly IExchangeRateService _exchangeRateService;

        public CurrencyController(IExchangeRateService exchangeRateService)
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
            return Ok(_exchangeRateService.GetSupportedCurrencies());
        }
    }
}
