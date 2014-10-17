using System;
using System.Collections.Generic;

namespace VaBank.Services.Contracts.Maintenance.Models
{
    public class DbChangeModel
    {
        public long Version { get; set; }

        public DatabaseOperationModel Action { get; set; }

        public DateTime TimestampUtc { get; set; }

        public Dictionary<string, object> Values { get; set; } 
    }
}
