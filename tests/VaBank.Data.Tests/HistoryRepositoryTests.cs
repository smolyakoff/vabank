using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Autofac;
using VaBank.Data.EntityFramework;
using VaBank.Core.Membership.Entities;
using VaBank.Data.EntityFramework.Common;

namespace VaBank.Data.Tests
{
    [TestClass]
    public class HistoryRepositoryTests : BaseTest
    {
        [TestMethod]
        public void Can_Get_History_Users()
        {
            var repository = Container.Resolve<HistoryRepository>();
            var users = repository.GetAll<HistoricalUser>(x => x.Id == Guid.Empty);
        }
    }
}
