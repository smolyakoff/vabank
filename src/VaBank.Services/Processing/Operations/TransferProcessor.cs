using System;
using System.Collections.Generic;
using System.Linq;
using VaBank.Common.Data.Repositories;
using VaBank.Common.Validation;
using VaBank.Core.Accounting.Entities;
using VaBank.Core.Processing.Entities;
using VaBank.Services.Common;
using VaBank.Services.Contracts.Common.Events;
using VaBank.Services.Contracts.Processing.Events;
using VaBank.Services.Contracts.Processing.Models;

namespace VaBank.Services.Processing.Operations
{
    internal abstract class TransferProcessor : BaseOperationProcessor
    {
        protected readonly IRepository<Transfer> TransferRepository;

        protected TransferProcessor(
            BaseOperationProcessorDependencies baseDependencies, 
            IRepository<Transfer> transferRepository) : base(baseDependencies)
        {
            Argument.NotNull(transferRepository, "transferRepository");
            TransferRepository = transferRepository;
        }

        protected override IEnumerable<ApplicationEvent> ProcessPending(BankOperationProcessorCommand command)
        {
            var transfer = (Transfer) command.BankOperation;
            var appOperationId = command.OperationId;
            if (transfer.Withdrawal != null && 
                transfer.Withdrawal.Status == ProcessStatus.Pending &&
                transfer.Deposit == null)
            {
                return WhenWithdrawalPending(appOperationId, transfer);
            }
            if (transfer.Withdrawal != null && 
                transfer.Deposit == null &&
                transfer.Withdrawal.Status == ProcessStatus.Failed)
            {
                return WhenWithdrawalFailed(appOperationId, transfer);
            }
            if (transfer.Withdrawal != null &&
                transfer.Deposit == null &&
                transfer.Withdrawal.Status == ProcessStatus.Completed)
            {
                return WhenWithdrawalCompleted(appOperationId, transfer);
            }
            if (transfer.Deposit != null && 
                transfer.Deposit.Status == ProcessStatus.Failed)
            {
                return WhenDepositFailed(appOperationId, transfer);
            }
            if (transfer.Deposit != null &&
                transfer.Deposit.Status == ProcessStatus.Completed)
            {
                return WhenDepositCompleted(appOperationId, transfer);
            }
            return WhenStateIsInvalid(appOperationId, transfer);
        }

        protected abstract Transaction Deposit(Account account, Transfer transfer, string code, string description);

        protected abstract Transaction Compensate(Account account, Transfer transfer, string code, string description);

        private IEnumerable<ApplicationEvent> WhenStateIsInvalid(Guid appOperationId, Transfer transfer)
        {
            transfer.Fail(string.Format(Messages.TransferFailedUnknownReason, transfer.Id));
            TransferRepository.Update(transfer);
            Logger.Error("Transfer #{0} is in invalid state.", transfer.Id);
            return Enumerable.Empty<ApplicationEvent>();
        }

        protected virtual IEnumerable<ApplicationEvent> WhenWithdrawalPending(Guid appOperationId, Transfer transfer)
        {
            var transactionModel = transfer.Withdrawal.ToModel<Transaction, TransactionModel>();
            return new List<ApplicationEvent>()
            {
                new TransactionProgressEvent(appOperationId, transactionModel, transfer.Id)
            };
        }

        protected virtual IEnumerable<ApplicationEvent> WhenWithdrawalCompleted(Guid appOperationId, Transfer transfer)
        {
            var transactionName = TransactionReferenceBook.ForOperation(transfer);
            var depositTransaction = Deposit(transfer.To, transfer, transactionName.Code, transactionName.Description);
            transfer.SetDepositTransaction(depositTransaction);
            var transactionModel = depositTransaction.ToModel<Transaction, TransactionModel>();
            TransferRepository.Update(transfer);
            return new List<ApplicationEvent>()
            {
                new TransactionProgressEvent(appOperationId, transactionModel, transfer.Id)
            };
        }

        protected virtual IEnumerable<ApplicationEvent> WhenWithdrawalFailed(Guid appOperationId, Transfer transfer)
        {
            transfer.Fail(string.Format(Messages.TransferFailed, transfer.Withdrawal.ErrorMessage));
            TransferRepository.Update(transfer);
            return Enumerable.Empty<ApplicationEvent>();
        }

        protected virtual IEnumerable<ApplicationEvent> WhenDepositCompleted(Guid appOperationId, Transfer transfer)
        {
            transfer.Complete();
            TransferRepository.Update(transfer);
            return Enumerable.Empty<ApplicationEvent>();
        }

        protected virtual IEnumerable<ApplicationEvent> WhenDepositFailed(Guid appOperationId, Transfer transfer)
        {
            transfer.Fail(string.Format(Messages.TransferFailed, transfer.Deposit.ErrorMessage));
            var transactionName = TransactionReferenceBook.CompensationFor(transfer.Withdrawal);
            var compensationTransaction = Compensate(transfer.From, transfer, transactionName.Code, transactionName.Description);
            TransferRepository.Update(transfer);
            var transactionModel = compensationTransaction.ToModel<Transaction, TransactionModel>();
            return new List<ApplicationEvent>
            {
                new TransactionProgressEvent(appOperationId, transactionModel, transfer.Id)
            };
        } 
    }
}