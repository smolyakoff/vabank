using System.Collections.ObjectModel;

namespace VaBank.Core.App.Entities
{
    public abstract class DatabaseRow
    {
        public abstract ReadOnlyDictionary<string, object> Values { get; }
    }
}
