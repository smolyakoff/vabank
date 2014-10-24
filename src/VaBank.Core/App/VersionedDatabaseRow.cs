using System;
using System.Collections.Generic;

namespace VaBank.Core.App
{
    public class VersionedDatabaseRow : DatabaseRow
    {
        private readonly Dictionary<string, object> _values;

        public VersionedDatabaseRow(long version, 
            DateTime timestampUtc,
            DatabaseOperation action)
        {
            _values = new Dictionary<string, object>();
        }

        public long Version { get; set; }

        public DateTime TimestampUtc { get; set; }

        public DatabaseOperation Action { get; set; }

        public VersionedDatabaseRow SetValue(string key, object value)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException("key");
            }
            _values[key] = value;
            return this;
        }

        public override IEnumerable<KeyValuePair<string, object>> Values
        {
            get { return _values; }
        }
    }
}
