using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VaBank.Core.App;
using VaBank.Core.Membership;
using VaBank.Data.EntityFramework.App;

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

        [TestCategory("Development")]
        [TestMethod]
        public void Can_Use_OperationMarkers()
        {
            var context = new VaBank.Data.EntityFramework.VaBankContext();
            var repo = new OperationMarkerRepository(context);
            using (var transaction = context.Database.BeginTransaction())
            {
                
                var marker1 = repo.Take("TEST-CHANGE", null, null);

                var marker2 = repo.Get();
                var marker3 = repo.Get();

                var markers = new List<OperationMarker>() {marker1, marker2, marker3};
                Assert.AreEqual(1, markers.Select(x => x.Id).Distinct().Count());

                repo.Release(marker1);

                var marker4 = repo.Get();
                Assert.IsNull(marker4);
                transaction.Commit();
            }

            var marker5 = repo.Get();
            Assert.IsNull(marker5);
        }
    }
}
