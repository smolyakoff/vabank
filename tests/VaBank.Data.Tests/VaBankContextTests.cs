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
            var user = User.Create("Test", "Admin", "123456!m");
            var profile = new UserProfile(user.Id)
            {
                Email = "john@gmail.com",
                FirstName = "John",
                LastName = "Doe",
                PhoneNumber = "123-456-789",
                PhoneNumberConfirmed = true,
                SecretPhrase = "Testing",
                SmsConfirmationEnabled = false,
                SmsNotificationEnabled = false
            };
            user.Profile = profile;
            context.Set<User>().Add(user);                    
            context.SaveChanges();
        }
    }
}
