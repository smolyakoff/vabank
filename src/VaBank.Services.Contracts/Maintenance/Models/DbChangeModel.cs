using System;
using System.Collections.Generic;

namespace VaBank.Services.Contracts.Maintenance.Models
{
    public class DbChangeModel
    {
        public long Version { get; set; }

        public DatabaseOperationModel Operation { get; set; }

        public DateTime TimestampUtc { get; set; }

        public List<KeyValuePair<string, object>> Values { get; set; } 
    }
}
