using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VaBank.Common.Filtration.Serialization;
using System.IO;
using VaBank.Common.Filtration;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace VaBank.Common.Tests
{
    [TestClass]
    public class FilteringTest
    {
        private const string Json = "{\"logic\": \"and\",\"type\": \"combiner\",\"filters\": [{\"type\": \"expression\",\"property\": \"name\",\"operator\": \"==\",\"value\": \"John\"},{\"type\": \"combiner\",\"logic\": \"and\",\"filters\": [{\"type\": \"expression\",\"property\": \"age\",\"operator\": \">\",\"value\": 5},{\"type\": \"expression\",\"property\": \"age\",\"operator\": \"<\",\"value\": 90}]}]}";
        private IJsonFilterDescriptorSerializer _jsonFilterDescriptorSerializer;

        public FilteringTest()
        {
            _jsonFilterDescriptorSerializer = new JsonFilterDescriptorSerializer();
        }
        
        [TestMethod]
        public void TestJsonToFilterDescriptorDeserialization()
        {
            var real = _jsonFilterDescriptorSerializer.Deserialize(Json);
            var expected = BuildFilterDescriptor();
            Assert.IsTrue(true);
        }

        [TestMethod]
        public void TestToFilterDescriptorExpression()
        {
            var descriptor = _jsonFilterDescriptorSerializer.Deserialize(Json);
            var real = descriptor.ToExpression<FakeUser>();
            Expression<Func<FakeUser, bool>> expected = x => x.Name == "John" && (x.Age > 5 && x.Age < 90);
            Assert.IsTrue(real.ToString().Equals(expected.ToString()));
        }

        private FilterDescriptor BuildFilterDescriptor()
        {
            var filterDescriptor = new FilterDescriptor();
            var context = new CombinerFilter { Logic = FilterLogic.And, Filters = new List<Filter>() };
            var expressionFilter = new ExpressionFilter { Operator = FilterOperator.Equality, Property = "name", Value = "John" };
            var combinerFilter = new CombinerFilter { Logic = FilterLogic.And, Filters = new List<Filter>() };
            context.Filters.Add(expressionFilter);
            context.Filters.Add(combinerFilter);

            expressionFilter = new ExpressionFilter { Operator = FilterOperator.GreaterThan, Property = "age", Value = "5" };
            combinerFilter.Filters.Add(expressionFilter);

            expressionFilter = new ExpressionFilter { Operator = FilterOperator.LessThan, Property = "age", Value = "90" };
            combinerFilter.Filters.Add(expressionFilter);
                        
            filterDescriptor.Context = context;
            return filterDescriptor;
        }
    }

    public class FakeUser
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }
}
