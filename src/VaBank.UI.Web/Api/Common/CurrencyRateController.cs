using System;
using System.Web.Http;
using VaBank.Services.Contracts.Processing;
using VaBank.Services.Contracts.Processing.Queries;

namespace VaBank.UI.Web.Api.Common
{
    [RoutePrefix("api/currencies/rates")]
    public class CurrencyRateController : ApiController
    {
        private readonly ICurrencyRateService _currencyRateService;

        public CurrencyRateController(ICurrencyRateService currencyRateService)
        {
            if (currencyRateService == null)
                throw new ArgumentNullException("currencyRateService");
            _currencyRateService = currencyRateService;
        }

        [Route("lookup/today")]
        [HttpGet]        
        public IHttpActionResult GetAllTodayRates()
        {
            return Ok(_currencyRateService.GetAllTodayRates());
        }

        [Route("lookup/{date:datetime}")]
        [HttpGet]
        public IHttpActionResult GetAllRates([FromUri]DateTime date)
        {
            return Ok(_currencyRateService.GetAllRates(date));
        }

        [Route()]
        [HttpGet]
        public IHttpActionResult Get(CurrencyRateQuery query)
        {
            return Ok(_currencyRateService.GetRate(query));
        }
    }
}
