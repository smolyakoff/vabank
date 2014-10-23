using VaBank.Common.Data.Repositories;
using VaBank.Core.Accounting;
using VaBank.Services.Common;

namespace VaBank.Services.Accounting
{
    public class AccountingRepositories: IDependencyCollection
    {
        public IQueryRepository<Currency> Currencies { get; set; }
        public IQueryRepository<UserCard> UserCards { get; set; }
        public IQueryRepository<CardAccount> CardAccounts { get; set; }
    }
}
