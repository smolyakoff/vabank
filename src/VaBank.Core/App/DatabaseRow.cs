using System.Collections.ObjectModel;

namespace VaBank.Core.App
{
    public abstract class DatabaseRow
    {
        public abstract ReadOnlyDictionary<string, object> Values { get; }
    }
}
