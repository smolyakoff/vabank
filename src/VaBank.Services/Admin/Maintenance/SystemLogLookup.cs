using System;
using System.Collections.Generic;
using System.Linq;
using VaBank.Services.Contracts.Admin.Maintenance;

namespace VaBank.Services.Admin.Maintenance
{
    internal class SystemLogLookup : SystemLogLookupModel
    {
        internal SystemLogLookup(IEnumerable<string> types)
        {
            Levels = Enum.GetValues(typeof (SystemLogLevelModel)).Cast<SystemLogLevelModel>().ToList();
            Types = types.ToList();
        }
    }
}
