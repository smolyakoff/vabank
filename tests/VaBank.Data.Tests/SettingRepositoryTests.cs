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
        public bool Equals(Man other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Age == other.Age && string.Equals(Name, other.Name);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Man) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Age*397) ^ Name.GetHashCode();
            }
        }

        public string Name { get; set; }

        public int Age { get; set; }

    }    
}
