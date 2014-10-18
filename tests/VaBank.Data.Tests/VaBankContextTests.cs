using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VaBank.Core.App;
using VaBank.Core.Membership;
using VaBank.Data.EntityFramework;
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
            /*var context = new VaBank.Data.EntityFramework.VaBankContext();

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
            context.SaveChanges();*/
        }

        [TestCategory("Development")]
        [TestMethod]
        public void Can_Start_And_Finish_Operations()
        {
            /*var context = new VaBankContext();
            var repo = new OperationRepository(context);
            using (var transaction = context.Database.BeginTransaction())
            {

                var op1 = repo.Start("TEST", null);

                var op2 = repo.GetCurrent();
                var op3 = repo.GetCurrent();

                var ops = new List<Operation>() { op1, op2, op3 };
                Assert.AreEqual(1, ops.Select(x => x.Id).Distinct().Count());

                repo.Stop(op1);

                var op4 = repo.GetCurrent();
                Assert.IsNull(op4);
                transaction.Commit();
            }

            var op5 = repo.GetCurrent();
            Assert.IsNull(op5);*/
        }
    }
}
