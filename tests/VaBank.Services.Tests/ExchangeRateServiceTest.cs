using Autofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VaBank.Services.Contracts.Processing;

namespace VaBank.Services.Tests
{
    [TestClass]
    public class ExchangeRateServiceTest : BaseTest
    {
        [TestMethod]
        public void CanUpdate_Currency_Exchange_Rates()
        {
            var service = base.Scope.Resolve<ICurrencyRateService>();
            service.UpdateRates();
        }
    }
}
