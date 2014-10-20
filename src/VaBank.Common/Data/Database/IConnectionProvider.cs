using System.Data.Common;

namespace VaBank.Common.Data.Database
{
    public interface IConnectionProvider
    {
        DbConnection Connection { get; }
    }
}
