using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace VaBank.Core.App.Entities
{
    public class VersionedDatabaseRow : DatabaseRow
    {
        private readonly Dictionary<string, object> _values;

        public VersionedDatabaseRow(long version, 
            DateTime timestampUtc,
            DatabaseOperation databaseOperation)
        {
            _values = new Dictionary<string, object>();
            Version = version;
            TimestampUtc = timestampUtc;
            DatabaseOperation = databaseOperation;
        }

        public long Version { get; private set; }

        public DateTime TimestampUtc { get; private set; }

        public DatabaseOperation DatabaseOperation { get; private set; }

        public VersionedDatabaseRow SetValue(string key, object value)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException("key");
            }
            _values[key] = value;
            return this;
        }

        public override ReadOnlyDictionary<string, object> Values
        {
            get { return new ReadOnlyDictionary<string, object>(_values); }
        }
    }
}
