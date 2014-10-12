using Microsoft.VisualStudio.TestTools.UnitTesting;
using VaBank.Common.Security;

namespace VaBank.Common.Tests
{
    [TestClass]
    public class SecurityTests
    {
        [TestMethod]
        [TestCategory("Production")]
        public void When_Using_Same_Passwords_Then_Hash_Is_The_Same()
        {
            var password = Password.Create("mycoolpassword");
            var salt = password.PasswordSalt;

            var result = Password.Validate(password.PasswordHash, salt, "mycoolpassword");
            Assert.IsTrue(result);
        }
    }
}
