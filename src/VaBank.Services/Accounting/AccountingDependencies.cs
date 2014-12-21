using VaBank.Common.Data.Repositories;
using VaBank.Core.Accounting.Entities;
using VaBank.Core.Accounting.Factories;
using VaBank.Core.Membership.Entities;
using VaBank.Core.Processing;
using VaBank.Core.Processing.Entities;
using VaBank.Core.Processing.Resources;
using VaBank.Services.Common;

namespace VaBank.Services.Accounting
{
    public class AccountingDependencies: IDependencyCollection
    {
        public IQueryRepository<Currency> Currencies { get; set; }

        public IPartialQueryRepository<UserCard> UserCards { get; set; }

        public IQueryRepository<CardAccount> CardAccounts { get; set; }

        public IQueryRepository<CardVendor> CardVendors { get; set; }

        public IRepository<User> Users { get; set; }

        public UserCardFactory UserCardFactory { get; set; }

        public CardAccountFactory CardAccountFactory { get; set; }

        public IQueryRepository<CardTransaction> CardTransactions { get; set; }

        public IQueryRepository<Transaction> Transactions { get; set; } 

        public MoneyConverter MoneyConverter { get; set; }

        public TransactionReferenceBook TransactionReferenceBook { get; set; }
    }
}
