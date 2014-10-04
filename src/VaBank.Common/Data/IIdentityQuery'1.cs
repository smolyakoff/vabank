using VaBank.Common.Data.Filtering;

namespace VaBank.Common.Data
{
    public interface IIdentityQuery<out T> : IFilterableQuery
    {
        T Id { get; }
    }
}
