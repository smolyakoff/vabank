using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VaBank.Common.Serialization;

namespace VaBank.Common.Tests
{
    [TestClass]
    public class UtilTests
    {
        [TestMethod]
        [TestCategory("Production")]
        public void Can_Serialize_Xml_Using_Json_Net()
        {
            var o = new {Name = "John"};
            var xml = JsonNetXml.SerializeObject(o);

            Assert.IsTrue(!string.IsNullOrEmpty(xml));
        }

        [TestMethod]
        [TestCategory("Production")]
        public void Can_Serialize_Generic_Lists_As_Xml()
        {
            var o = new { Name = "John", Phones = new List<string> {"phone1", "phone2"} };
            var xml = JsonNetXml.SerializeObject(o);

            Assert.IsTrue(!string.IsNullOrEmpty(xml));
        }
    }
}
