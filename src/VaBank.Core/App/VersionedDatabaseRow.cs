using System;
using System.Collections.Generic;

namespace VaBank.Core.App
{
    public class VersionedDatabaseRow : DatabaseRow
    {
        private readonly Dictionary<string, object> _values;

        public VersionedDatabaseRow(Dictionary<string, object> values)
        {
            if (values == null)
                throw new ArgumentNullException("values");
            _values = values;
        }

        public long Version { get; set; }

        public DateTime TimestampUtc { get; set; }

        public DatabaseOperation Action { get; set; }

        public override IEnumerable<KeyValuePair<string, object>> Values
        {
            get { return _values; }
        }
    }
}
