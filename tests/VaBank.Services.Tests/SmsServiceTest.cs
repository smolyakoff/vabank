using Autofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VaBank.Services.Contracts.Infrastructure.Sms;

namespace VaBank.Services.Tests
{
    [TestClass]
    public class SmsServiceTest : BaseTest
    {
        [TestMethod]
        [TestCategory("Development")]
        public void CanSendSms_WithoutExceptions()
        {
            var smsService = Scope.Resolve<ISmsService>();
            var sms = new SendSmsCommand
            {
                RecipientPhoneNumber = "+375293593295",
                Text = "SMS Service Test"
            };
            smsService.SendSms(sms);
        }
    }
}
