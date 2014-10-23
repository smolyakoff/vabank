using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VaBank.Common.Data.Repositories;
using VaBank.Services.Common;

namespace VaBank.Services.Accounting
{
    public class AccountingRepositories: IDependencyCollection
    {
        public IQueryRepository<AccountingLookup> Lookups { get; set; }
    }
}
