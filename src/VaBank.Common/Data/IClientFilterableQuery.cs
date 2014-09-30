using VaBank.Common.Data.Filtering;

namespace VaBank.Common.Data
{
    public interface IClientFilterableQuery : IFilterableQuery
    {
        void ApplyFilter(IFilter filter);
    }
}
