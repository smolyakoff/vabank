using VaBank.Common.Data;
using VaBank.Common.Data.Filtering;

namespace VaBank.Services.Contracts.Common.Queries
{
    public class IdentityQuery<T> : IIdentityQuery<T>
    {
        private IFilter _filter;

        public T Id { get; set; }

        public IFilter Filter
        {
            get {return _filter ?? new DynamicLinqFilter("Id == @0", Id);}
            set { _filter = value; }
        }

        public bool InMemoryFiltering
        {
            get { return false; }
        }
    }
}
