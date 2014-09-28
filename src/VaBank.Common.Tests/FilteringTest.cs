using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VaBank.Common.Filtration.Serialization;
using System.IO;
using VaBank.Common.Filtration;
using System.Collections.Generic;
using System.Linq.Expressions;
using VaBank.Common.Tests.Fakes;

namespace VaBank.Common.Tests
{
    [TestClass]
    public class FilteringTest
    {
        //TODO: move to file and use DeploymentItemAttribute
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

        [TestMethod]
        public void Can_Convert_Expression_With_In_Operator()
        {
            var filterString = "{\"type\": \"expression\",\"property\": \"name\",\"operator\": \"in\",\"value\": [\"John\", \"Jack\"]}";
            var filter = _jsonFilterDescriptorSerializer.Deserialize(filterString);
            var expression = filter.ToExpression<FakeUser>();
        }

        [TestMethod]
        public void Can_Parse_Valid_Filter_Query()
        {
            //TODO: move to file and use DeploymentItemAttribute
            const string query =
                "((timestampUtc gt \"2014-08-31T21:00:00.000Z\") and (timestampUtc lt \"2014-09-25T21:00:00.000Z\") and ((type eq null) or (type eq \"Generic\")) and ((level in [\"Debug\",\"Error\",\"Fatal\"]) or (level eq null)))";
            var converter = new FilterTypeConverter();
            var filter = converter.ConvertFrom(query);
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
}
