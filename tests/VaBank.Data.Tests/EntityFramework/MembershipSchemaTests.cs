using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VaBank.Core.Membership.Entities;

namespace VaBank.Data.Tests.EntityFramework
{
    [TestClass]
    public class MembershipSchemaTests : EntityFrameworkTest
    {
        [TestCategory("Development")]
        [TestMethod]
        public void Can_VaBank_Context_Save_User()
        {
            var context = Context;

            var user = User.Create(string.Format("Test_{0}", Guid.NewGuid().ToString()), "Admin", "1234567890!");
            var profile = new UserProfile(user.Id)
            {
                Email = "test@test.com",
                FirstName = "Test",
                LastName = "Test",
                PhoneNumber = "+375 29 3981330",
                PhoneNumberConfirmed = true,
                SecretPhrase = "What is the color of night?",
                SmsConfirmationEnabled = false,
                SmsNotificationEnabled = false
            };

            context.Set<User>().Add(user);
            context.Set<UserProfile>().Add(profile);
            context.SaveChanges();
        }
    }
}
