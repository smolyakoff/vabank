using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using PagedList;
using VaBank.Common.Data;
using VaBank.Common.Data.Filtering;
using VaBank.Common.Data.Filtering.Converters;
using VaBank.Common.Data.Linq;
using VaBank.Common.Data.Sorting;
using VaBank.Common.Data.Sorting.Converters;
using VaBank.Common.Tests.Fakes;

namespace VaBank.Common.Tests
{
    [TestClass]
    [DeploymentItem("Files\\", "Files")]
    public class DataTests
    {
        private readonly static List<Car> Cars = new List<Car>
        {
            new Car {Make = "Ford", Model = "Fusion", Year = 2011},
            new Car {Make = "Chrysler", Model = "Town & Country", Year = 2004},
            new Car {Make = "Dodge", Model = "Caravan", Year = 2001},
            new Car {Make = "Audi", Model = "A6", Year = 2012},
            new Car {Make = "Porsche", Model = "Cayenne", Year = 2010}
        };

        private readonly static List<LogEntry> Logs = new List<LogEntry>
        {
            new LogEntry {Level = "Debug", Type = "Generic", TimestampUtc = new DateTime(2012, 1, 1)},
            new LogEntry {Level = "Error", Type = null, TimestampUtc = new DateTime(2014, 9, 2)},
            new LogEntry {Level = "Debug", Type = null, TimestampUtc = new DateTime(2014, 9, 10)},
        };

        [TestCategory("Production")]
        [TestMethod]
        public void Can_Deserialize_And_Execute_Filter_From_Json()
        {
            var filterJson = File.ReadAllText("Files/filter.json");
            var filter = JsonConvert.DeserializeObject<IFilter>(filterJson, new JsonSerializerSettings()
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });

            Assert.IsNotNull(filter);
            Assert.IsInstanceOfType(filter, typeof(CombinedFilter));
            var expected = Cars.Where(x => new string[] { "Ford", "Dodge" }.Contains(x.Make) && x.Year > 2002).ToList();
            var actual = Cars.AsQueryable().Where(filter.ToExpression<Car>()).ToList();
            Assert.IsTrue(expected.SequenceEqual(actual));
        }

        [TestCategory("Production")]
        [TestMethod]
        public void Can_Deserialize_And_Execute_Sort_From_Json()
        {
            var sortJson = File.ReadAllText("Files/sort.json");
            var sort = JsonConvert.DeserializeObject<ISort>(sortJson, new JsonSerializerSettings()
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });

            Assert.IsNotNull(sort);
            Assert.IsInstanceOfType(sort, typeof(MultiSort));
            var expected = Cars.OrderBy(x => x.Make).ThenByDescending(x => x.Model).ToList();
            var actual = sort.ToDelegate<Car>()(Cars.AsQueryable()).ToList();
            Assert.IsTrue(expected.SequenceEqual(actual));
        }

        [TestCategory("Production")]
        [TestMethod]
        public void Can_Deserialize_And_Execute_Filter_From_Query_String()
        {
            var filterTxt = File.ReadAllText("Files/filter.txt");
            var converter = new QueryStringFilterConverter();
            var filter = (IFilter)converter.ConvertFrom(HttpUtility.UrlDecode(filterTxt));

            Assert.IsNotNull(filter);
            Assert.IsInstanceOfType(filter, typeof(DynamicLinqFilter));

            var expected = Logs.Where(
                x => x.TimestampUtc > new DateTime(2014, 9, 1) &&
                    x.TimestampUtc < new DateTime(2014, 9, 30) &&
                    new string[] { "Error", "Warning" }.Contains(x.Level) &&
                    x.Type == null
                ).ToList();
            var actual = Logs.AsQueryable().Where(filter.ToExpression<LogEntry>()).ToList();
            Assert.IsTrue(expected.SequenceEqual(actual));
        }

        [TestCategory("Production")]
        [TestMethod]
        public void Can_Deserialize_And_Execute_Sort_From_Query_String()
        {
            const string text = "year DESC, make ASC";
            var converter = new SortTypeConverter();
            var sort = (ISort)converter.ConvertFrom(text);

            Assert.IsNotNull(sort);
            Assert.IsInstanceOfType(sort, typeof(DynamicLinqSort));

            var expected = Cars.OrderByDescending(x => x.Year).ThenBy(x => x.Make).ToList();
            var actual = sort.ToDelegate<Car>()(Cars.AsQueryable()).ToList();

            Assert.IsTrue(expected.SequenceEqual(actual));
        }

        [TestCategory("Production")]
        [TestMethod]
        public void Can_Get_Used_Properties_From_Expression_With_Nested_Property_Access()
        {
            Expression<Func<Garage, bool>> exp = x => x.Car.Make == "Ford" && x.Color == "Green";
            var properties = PropertiesVisitor.GetUsedProperties<Garage>(exp).ToList();

            Assert.IsTrue(properties.Contains("Car.Make") && properties.Contains("Color"));
        }

        [TestMethod]
        [TestCategory("Production")]
        public void Can_Execute_Simple_Query()
        {
            var query = new NewAmericanCarsQuery();

            var cars = Cars.AsQueryable().QueryPage(query);

            Assert.IsNotNull(cars);
            Assert.IsInstanceOfType(cars, typeof(IPagedList<Car>));
            Assert.AreEqual(2, cars.TotalItemCount);
            Assert.AreEqual(2, cars.PageCount);


            var expected = Cars.Where(x => x.Year > 2002 && new string[] {"Dodge", "Ford", "Chrysler"}.Contains(x.Make))
                .OrderBy(x => x.Model)
                .Take(1);
            var actual = cars;
            Assert.IsTrue(expected.SequenceEqual(actual));
        }
    }
}
