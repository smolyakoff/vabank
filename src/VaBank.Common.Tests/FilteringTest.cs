using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VaBank.Common.Filtration.Serialization;
using System.IO;

namespace VaBank.Common.Tests
{
    [TestClass]
    public class FilteringTest
    {
        private string _testingJsonPath = @"D:\test.json";
        private string _json;

        public FilteringTest()
        {
            _json = File.ReadAllText(_testingJsonPath);
        }

        [TestMethod]
        public void TestJsonToFilterLogicConverter()
        {
            var serializer = new JsonFilterDescriptorSerializer();
            var obj = serializer.Deserialize(_json);

            File.WriteAllText(@"D:\test_out.json", serializer.Serialize(obj));
        }
    }
}
