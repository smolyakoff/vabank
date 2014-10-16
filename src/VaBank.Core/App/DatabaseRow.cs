using System.Collections.Generic;

namespace VaBank.Core.App
{
    public abstract class DatabaseRow
    {
        public abstract IEnumerable<KeyValuePair<string, object>> Values { get; }
    }
}s
