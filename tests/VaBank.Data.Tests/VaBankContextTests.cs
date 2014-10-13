using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VaBank.Core.Membership;

namespace VaBank.Data.Tests
{    
    [TestClass]
    public class VaBankContextTests
    {
        [TestCategory("Development")]
        [TestMethod]
        public void Can_VaBank_Context_Save_User()
        {
            var context = new VaBank.Data.EntityFramework.VaBankContext();

            var user = User.Create(string.Format("Test_{0}", Guid.NewGuid().ToString()), "Admin", "1234567890!");
            var profile = new UserProfile(user.Id)
            {
                Email = "test@test.com",
                FirstName = "Test",
                LastName = "Test",
                PhoneNumber = "+375 29 3981330",
                PhoneNumberConfirmed = true,
                SecretPhrase = "Test",
                SmsConfirmationEnabled = false,
                SmsNotificationEnabled = false                
            };

            context.Set<User>().Add(user);
            context.Set<UserProfile>().Add(profile);                        
            context.SaveChanges();
        }
    }
}
