using System;
using System.Collections.Generic;

namespace VaBank.Core.App
{
    public class VersionedDatabaseRow : DatabaseRow
    {
        public long Version { get; set; }

        public DateTime TimestampUtc { get; set; }

        public DatabaseOperation Action { get; set; }

        public override IEnumerable<KeyValuePair<string, object>> Values
        {
            get { throw new System.NotImplementedException(); }
        }
    }
}
