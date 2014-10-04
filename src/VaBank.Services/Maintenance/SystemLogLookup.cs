using System;
using System.Collections.Generic;
using System.Linq;
using NLog;
using VaBank.Services.Contracts.Admin.Maintenance;

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
                LogLevel.Debug,
                LogLevel.Fatal
            }.Select(x => x.ToString()).ToList();
            Types = types.ToList();
        }
    }
}
