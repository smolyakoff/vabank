using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using VaBank.Common.Data.Filtering;
using VaBank.Common.Tests.Fakes;

namespace VaBank.Common.Tests
{
    [TestClass]
    [DeploymentItem("Files/filter.json", "Files")]
    public class DataTests
    {
        private readonly static List<Car> Cars = new List<Car>
        {
            new Car {Make = "Ford", Model = "Fusion", Year = 2011},
            new Car {Make = "Chrysler", Model = "Town & Country", Year = 2004},
            new Car {Make = "Audi", Model = "A6", Year = 2012},
            new Car {Make = "Porsche", Model = "Cayenne", Year = 2010}
        };
            
        [TestCategory("Production")]
        [TestMethod]
        public void Can_Deserialize_And_Execute_Filter_From_Json()
        {
            var filterJson = File.ReadAllText("Files/filter.json");
            var filter = JsonConvert.DeserializeObject<IFilter>(filterJson);

            Assert.IsNotNull(filter);
            Assert.IsInstanceOfType(filter, typeof(CombinedFilter));

            var expected = Cars.Where(x => new string[] {"Ford", "Dodge"}.Contains(x.Make) && x.Year > 2002).ToList();
            var actual = Cars.AsQueryable().Where(filter.ToExpression<Car>()).ToList();

            Assert.IsTrue(expected.SequenceEqual(actual));
        }
    }
}
