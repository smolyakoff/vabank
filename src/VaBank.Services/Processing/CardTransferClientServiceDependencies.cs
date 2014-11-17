using VaBank.Common.Data.Repositories;
using VaBank.Core.Accounting.Entities;
using VaBank.Core.Processing.Entities;
using VaBank.Core.Processing.Factories;
using VaBank.Services.Common;

namespace VaBank.Services.Processing
{
    public class CardTransferClientServiceDependencies : IDependencyCollection
    {
        public IQueryRepository<UserCard> UserCards { get; set; }

        public IRepository<CardTransfer> CardTransfers { get; set; }

        public CardTransferFactory CardTransferFactory { get; set; }
    }
}
