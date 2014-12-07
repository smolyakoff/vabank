using System.Linq;
using System.Threading;
using Autofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VaBank.Common.Data.Repositories;
using VaBank.Core.Processing.Entities;
using VaBank.Services.Contracts.Accounting;
using VaBank.Services.Contracts.Accounting.Queries;
using VaBank.Services.Contracts.Common.Models;
using VaBank.Services.Contracts.Processing;
using VaBank.Services.Contracts.Processing.Commands;
using VaBank.Services.Contracts.Transfers;
using VaBank.Services.Contracts.Transfers.Commands;

namespace VaBank.Services.Tests
{
    [TestClass]
    public class CardTransferServiceTest : BaseTest
    {
        [TestMethod]
        [TestCategory("Development")]
        public void Can_Submit_Card_Transfer()
        {
            var user = AuthenticateTerminator();
            user.Profile.SmsConfirmationEnabled = false;
            var transferService = Scope.Resolve<ICardTransferClientService>();
            var cardAccountService = Scope.Resolve<ICardAccountService>();

            var cards = cardAccountService.GetUserCards(new CardQuery())
                .Where(x => x.AccountNo != null)
                .ToList();
            var command = new InterbankCardTransferCommand
            {
                FromCardId = cards.First(x => x.Owner.UserId == user.Id).CardId,
                ToCardNo = cards.First(x => x.Owner.UserId != user.Id).CardNo,
                ToCardExpirationDateUtc = cards[1].ExpirationDateUtc,
                Amount = 10
            };

            transferService.Transfer(command);
        }

        [TestMethod]
        [TestCategory("Development")]
        public void Can_Submit_And_Process_Card_Transfer()
        {
            var user = AuthenticateTerminator();
            user.Profile.SmsConfirmationEnabled = false;
            var transferService = Scope.Resolve<ICardTransferClientService>();
            var cardAccountService = Scope.Resolve<ICardAccountService>();
            var transferRepository = Scope.Resolve<IRepository<Transfer>>();
            var processingService = Scope.Resolve<IProcessingService>();

            var cards = cardAccountService.GetUserCards(new CardQuery())
                .Where(x => x.AccountNo != null)
                .ToList();
            var command = new InterbankCardTransferCommand
            {
                FromCardId = cards.First(x => x.Owner.UserId == user.Id).CardId,
                ToCardNo = cards.First(x => x.Owner.UserId != user.Id).CardNo,
                ToCardExpirationDateUtc = cards[1].ExpirationDateUtc,
                Amount = 10
            };

            var operation = transferService.Transfer(command);
            var command2 = new ProcessBankOperationCommand
            {
                BankOperationId = operation.Id
            };
            var nextState = processingService.ProcessBankOperation(command2);
            var transfer = transferRepository.Find(command2.BankOperationId);
            processingService.ProcessTransaction(new ProcessTransactionCommand
            {
                OperationId = command2.BankOperationId,
                TransactionId = transfer.Withdrawal.Id
            });
            nextState = processingService.ProcessBankOperation(command2);
            processingService.ProcessTransaction(new ProcessTransactionCommand
            {
                OperationId = command2.BankOperationId,
                TransactionId = transfer.Deposit.Id
            });
            nextState = processingService.ProcessBankOperation(command2);
            transfer = transferRepository.Find(command2.BankOperationId);
            Assert.AreEqual(ProcessStatus.Completed, transfer.Status);
        }
    }
}
