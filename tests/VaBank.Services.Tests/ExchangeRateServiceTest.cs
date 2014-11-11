using Autofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VaBank.Services.Contracts.Processing;
using VaBank.Services.Processing;

namespace VaBank.Services.Tests
{
    [TestClass]
    public class ExchangeRateServiceTest : BaseTest
    {
        [TestMethod]
        [TestCategory("Development")]
        public void Can_Get_Rates_From_Nbrb()
        {
            var client = new NBRBServiceClient();
            var rates = client.GetLatestRates();

            client.Dispose();

            Assert.IsNotNull(rates);
            Assert.IsTrue(rates.Count > 0);
        }

        [TestMethod]
        [TestCategory("Development")]
        public void CanUpdate_Currency_Exchange_Rates()
        {
            var service = base.Scope.Resolve<ICurrencyRateService>();
            service.UpdateRates();
        }
    }
}
