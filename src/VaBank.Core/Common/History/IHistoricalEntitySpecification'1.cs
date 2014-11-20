using VaBank.Common.Data.Linq;

namespace VaBank.Core.Common.History
{
    public interface IHistoricalEntitySpecification<T>
    {
        LinqSpec<T> OriginalKey(params object[] keys);
    }
}
