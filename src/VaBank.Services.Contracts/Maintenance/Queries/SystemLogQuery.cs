﻿using VaBank.Common.Data.Filtering;

namespace VaBank.Services.Contracts.Maintenance.Queries
{
    public class SystemLogQuery : IClientFilterable
    {
        public SystemLogQuery()
        {
            ClientFilter = new AlwaysTrueFilter();
        }

        public IFilter ClientFilter { get; set; }
    }
}
