using System.Data.Entity;
using System.Linq;
using Autofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VaBank.Core.Membership.Entities;
using VaBank.Data.EntityFramework;
using VaBank.Services.Common.Security;
using VaBank.Services.Contracts.Common.Models;
using VaBank.Services.Contracts.Infrastructure.Secuirty;
using VaBank.Services.Contracts.Infrastructure.Sms;
using VaBank.Services.Tests.Fakes;

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

        [TestMethod]
        [TestCategory("Development")]
        public void SecurityCodeValidator_Returns_Error_For_Invalid_Code()
        {
            var context = Scope.Resolve<DbContext>();
            var user = AuthenticateTerminator();
            var profile = user.Profile;
            profile.SmsConfirmationEnabled = true;
            context.SaveChanges();

            var securityCodeService = Scope.Resolve<ISecurityCodeService>();
            var code = securityCodeService.GenerateSecurityCode();
           
            var validator = Scope.Resolve<CodeSecurityValidator>();
            var command = new TestSecurityCodeCommand
            {
                SecurityCode = new SecurityCodeModel {Code = "ABCDEF", Id = code.Id}
            };
            var result = validator.Validate(command);

            Assert.IsFalse(result.IsValid);
        }
    }
}
