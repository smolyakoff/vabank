using System;
using VaBank.Common.Data.Repositories;
using VaBank.Common.IoC;
using VaBank.Core.Accounting.Entities;
using VaBank.Core.Processing;
using VaBank.Core.Processing.Entities;

namespace VaBank.Services.Processing.Operations
{
    [Injectable]
    internal class CardTransferProcessor : TransferProcessor
    {
        public CardTransferProcessor(BaseOperationProcessorDependencies baseDependencies, IRepository<Transfer> transferRepository) 
            : base(baseDependencies, transferRepository)
        {
        }

        protected override DateTime? PostponeDateOrNull(BankOperation operation)
        {
            return null;
        }

        protected override Transaction Deposit(Account account, Transfer transfer, string code, string description)
        {
            var cardTransfer = (CardTransfer) transfer;
            return account.Deposit(cardTransfer.DestinationCard, code, description, Settings.Location,
                new Money(transfer.Currency, transfer.Amount), this.MoneyConverter);
        }

        protected override Transaction Compensate(Account account, Transfer transfer, string code, string description)
        {
            var cardTransfer = (CardTransfer)transfer;
            return account.Deposit(cardTransfer.SourceCard, code, description, Settings.Location,
                new Money(transfer.Currency, transfer.Amount), this.MoneyConverter);
        }

        public override bool CanProcess(BankOperation operation)
        {
            return operation.Category.Code.StartsWith("TRANSFER-CARD");
        }
    }
}
