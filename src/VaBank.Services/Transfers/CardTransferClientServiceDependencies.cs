using VaBank.Common.Data.Repositories;
using VaBank.Core.Accounting.Entities;
using VaBank.Core.Processing;
using VaBank.Core.Processing.Entities;
using VaBank.Core.Transfers.Entities;
using VaBank.Core.Transfers.Factories;
using VaBank.Services.Common;

namespace VaBank.Services.Transfers
{
    public class CardTransferClientServiceDependencies : IDependencyCollection
    {
        public IQueryRepository<UserCard> UserCards { get; set; }

        public IRepository<CardTransfer> CardTransfers { get; set; }

        public IRepository<UserBankOperation> UserBankOperations { get; set; }

        public CardTransferFactory CardTransferFactory { get; set; }

        public CardTransferSettings CardTransferSettings { get; set; }
    }
}
