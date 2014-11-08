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

        [Route]
        [HttpGet]        
        public IHttpActionResult GetAllTodayRates()
        {
            return Ok(_currencyRateService.GetAllTodayRates());
        }

        [Route("{date:datetime}")]
        [HttpGet]
        public IHttpActionResult GetAllRates([FromUri]DateTime date)
        {
            return Ok(_currencyRateService.GetAllRates(date));
        }

        [Route("from/{buyingCurrencyISOName:alpha}/to/{sellingCurrencyISOName:alpha}")]
        [HttpGet]
        public IHttpActionResult GetTodayRate([FromUri]TodayCurrencyRateQuery query)
        {
            return Ok(_currencyRateService.GetTodayRate(query));
        }

        [Route("{date:datetime}/from/{buyingCurrencyISOName:alpha}/to/{sellingCurrencyISOName:alpha}")]
        [HttpGet]
        public IHttpActionResult GetRate([FromUri]CurrencyRateQuery query)
        {
            return Ok(_currencyRateService.GetRate(query));
        }
    }
}
