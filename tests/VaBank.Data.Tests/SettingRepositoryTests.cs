using Autofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using VaBank.Core.App;
using VaBank.Core.App.Repositories;
using VaBank.Core.Maintenance;

namespace VaBank.Data.Tests
{
    [TestClass]    
    public class SettingRepositoryTests : BaseTest
    {
        private readonly ISettingRepository _settings;

        public SettingRepositoryTests()
        {
            _settings = Container.Resolve<ISettingRepository>();
        }

        [TestCategory("Development")]
        [TestMethod]
        public void Can_Set_Man_Setting()
        {
            var expected = new Man { Name = "Zakhar", Age = 21 };
            _settings.Set<Man>(Keys.ManSettingName, expected);
            var actual = _settings.GetOrDefault<Man>(Keys.ManSettingName);
            Assert.AreEqual(expected, actual);
        }

        private static class Keys
        {
            public const string ManSettingName = "Test_Man";
        }
    }

    internal class ManSetting
    {
        public Man Man { get; set; }        
    }

    internal class Man : IEquatable<Man>
    {
        public string Name { get; set; }

        public int Age { get; set; }

        public override bool Equals(object obj)
        {
            return Equals((Man)obj);
        }

        public bool Equals(Man other)
        {
            return other.Age == Age && other.Name == Name;
        }
    }    
}
