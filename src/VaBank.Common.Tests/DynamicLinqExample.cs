using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Linq.Expressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VaBank.Common.Tests.Fakes;

namespace VaBank.Common.Tests
{
    [TestClass]
    public class DynamicLinqExample
    {
        [TestMethod]
        public void Test_Dynamic_Linq()
        {
            var users = new List<FakeUser>
            {
                new FakeUser {Name = "John", Age = 25, BirthDate = new DateTime(1990, 1, 1)},
                new FakeUser {Name = "Bob", Age = 27, BirthDate = new DateTime(1987, 1, 3)}
            };
            var names = new List<string> {"John", "Alex"};
            var list = users.AsQueryable().Where("@1.Contains(name) AND birthDate > @0", new DateTime(1987,1,1), names).ToList();
        }
    }
}
