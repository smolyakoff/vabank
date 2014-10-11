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
            var profile = new UserProfile
            {
                Email = "john@gmail.com",
                FirstName = "John",
                UserId = Guid.NewGuid(),
                LastName = "Doe",
                PhoneNumber = "123-456-789",
                PhoneNumberConfirmed = true,
                SecretPhrase = "Testing",
                SmsConfirmationEnabled = false,
                SmsNotificationEnabled = false
            };
            var user = new User 
            { 
                Profile = profile,
                Id = profile.UserId,
                PasswordHash = "1234567890",
                PasswordSalt = "1234567890",
                UserName = "Test"
            };
            context.Set<User>().Add(user);
            context.Set<UserProfile>().Add(profile);                        
            context.SaveChanges();
        }
    }
}
