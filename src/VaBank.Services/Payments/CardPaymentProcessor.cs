using System;
using VaBank.Common.Data.Repositories;
using VaBank.Common.IoC;
using VaBank.Core.Accounting.Entities;
using VaBank.Core.Payments.Entities;
using VaBank.Core.Processing;
using VaBank.Core.Processing.Entities;
using VaBank.Services.Processing.Operations;

namespace VaBank.Services.Payments
{
    [Injectable]
    //TODO: move this to processing namespace
    internal class CardPaymentProcessor : TransferProcessor
    {
        public CardPaymentProcessor(BaseOperationProcessorDependencies baseDependencies, IRepository<Transfer> transferRepository)
            : base(baseDependencies, transferRepository)
        {
        }

        public override bool CanProcess(BankOperation operation)
        {
            return operation.Category.Code.StartsWith("PAYMENT-CARD");
        }

        protected override DateTime? PostponeDateOrNull(BankOperation operation)
        {
            return null;
        }

        protected override Transaction Deposit(Account account, Transfer transfer, string code, string description)
        {
            var payment = (CardPayment) transfer;
            return account.Deposit(payment.Order, payment.Card, code, description, Settings.Location,
                new Money(transfer.Currency, transfer.Amount), MoneyConverter);
        }

        protected override Transaction Compensate(Account account, Transfer transfer, string code, string description)
        {
            var payment = (CardPayment) transfer;
            return account.Deposit(payment.Order, payment.Card, code, description, Settings.Location,
                new Money(transfer.Currency, transfer.Amount), MoneyConverter);
        }
    }
}
