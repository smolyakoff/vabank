using System;
using System.Collections.Generic;
using System.Linq;
using NLog;
using VaBank.Services.Contracts.Maintenance;
using VaBank.Services.Contracts.Maintenance.Queries;

namespace VaBank.Services.Maintenance
{
    internal class SystemLogLookup : SystemLogLookupModel
    {
        internal SystemLogLookup(IEnumerable<string> types)
        {
            Levels = new List<LogLevel>()
            {
                LogLevel.Debug,
                LogLevel.Trace,
                LogLevel.Info,
                LogLevel.Warn,
                LogLevel.Error,
                LogLevel.Fatal
            }.Select(x => x.ToString()).ToList();
            Types = types.ToList();
        }
    }
}
