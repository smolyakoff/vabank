﻿using System;
using VaBank.Common.Data;
using VaBank.Common.Data.Repositories;
using VaBank.Common.Validation;
using VaBank.Core.Processing;
using VaBank.Core.Processing.Entities;
using VaBank.Services.Common;
using VaBank.Services.Contracts.Accounting.Models;
using VaBank.Services.Contracts.Common;
using VaBank.Services.Contracts.Processing;
using VaBank.Services.Contracts.Processing.Commands;
using VaBank.Services.Contracts.Processing.Models;
using VaBank.Services.Processing.Operations;
using VaBank.Services.Processing.Transactions;

namespace VaBank.Services.Processing
{
    public class ProcessingService : BaseService, IProcessingService
    {
        private readonly ProcessingServiceDependencies _deps;

        public ProcessingService(BaseServiceDependencies baseDepenendencies, ProcessingServiceDependencies processingServiceDependencies) 
            : base(baseDepenendencies)
        {
            Argument.NotNull(processingServiceDependencies, "processingServiceDependencies");
            processingServiceDependencies.EnsureIsResolved();

            _deps = processingServiceDependencies;
        }

        public OperationProcessingResult ProcessBankOperation(ProcessBankOperationCommand command)
        {
            try
            {
                var operation = _deps.BankOperations.SurelyFind(command.BankOperationId);
                var appOperationId = Operation.Id;
                var events = _deps.CentralProcessor.Process(new BankOperationProcessorCommand(appOperationId, operation));
                Commit();
                var operationModel = operation.ToModel<BankOperation, BankOperationModel>();
                return new OperationProcessingResult(operationModel, events);
            }
            catch (Exception ex)
            {
                throw new ServiceException("Can't process bank operation.", ex);
            }
        }

        public TransactionProcessingResult ProcessTransaction(ProcessTransactionCommand command)
        {
            try
            {
                var transaction = _deps.Transactions.SurelyFind(command.TransactionId);
                var operation = _deps.BankOperations.Find(command.OperationId);
                var events = _deps.CentralProcessor.Process(new TransactionProcessorCommand(Operation.Id, transaction, operation));
                Commit();
                var transactionModel = transaction.ToModel<TransactionModel>();
                return new TransactionProcessingResult(transactionModel, events);
            }
            catch (Exception ex)
            {
                throw new ServiceException("Can't process transaction.", ex);
            }
        }

        public CardTransactionModel GetCardTransaction(IdentityQuery<Guid> id)
        {
            EnsureIsValid(id);
            try
            {
                var query = DbQuery.For<CardTransaction>().FilterBy(x => x.Id == id.Id);
                var transaction = _deps.CardTransactions.QueryOne(query);
                return transaction == null ? null : transaction.ToModel<CardTransactionModel>();
            }
            catch (Exception ex)
            {
                throw new ServiceException("Can't get card transaction.", ex);
            }
        }
    }
}