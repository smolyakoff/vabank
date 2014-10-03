using System.Collections.Generic;
using VaBank.Common.Data.Filtering;
using VaBank.Common.Data.Paging;
using VaBank.Common.Data.Sorting;


namespace VaBank.Common.Tests.Fakes
{
    public class NewAmericanCarsQuery : IFilterableQuery, IPageableQuery
    {
        private readonly List<string> _american = new List<string>
        {
            "Dodge",
            "Chrysler",
            "Ford"
        };

        public NewAmericanCarsQuery()
        {
            Filter = new ExpressionFilter<Car>(x => x.Year > 2002).And(new ExpressionFilter<Car>(x => _american.Contains(x.Make)));
            Sort = new DynamicLinqSort("Model asc");
            PageSize = 1;
            PageNumber = 1;
        }


        public IFilter Filter { get; private set; }
        public bool InMemoryFiltering { get; private set; }
        public ISort Sort { get; private set; }
        public bool InMemorySorting { get; private set; }
        public int PageNumber { get; private set; }
        public int PageSize { get; private set; }
        public bool InMemoryPaging { get; private set; }
    }
}
