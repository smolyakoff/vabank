using System;
using VaBank.Common.Data.Repositories;
using VaBank.Common.IoC;
using VaBank.Common.Validation;
using VaBank.Core.Accounting.Entities;
using VaBank.Core.Processing;
using VaBank.Core.Processing.Entities;
using VaBank.Core.Processing.Resources;
using VaBank.Core.Transfers.Entities;

namespace VaBank.Core.Transfers.Factories
{
    [Injectable]
    public class CardTransferFactory
    {
        private const string PersonalTransferOperation = "TRANSFER-CARD-PERSONAL";
        private const string InterbankTransferOperation = "TRANSFER-CARD-INTERBANK";

        private readonly IRepository<OperationCategory> _operationCategories;
        private readonly TransactionReferenceBook _transactionReferenceBook;
        private readonly BankSettings _settings;
        private readonly MoneyConverter _moneyConverter;

        public CardTransferFactory(IRepository<OperationCategory> operationCategories, 
            TransactionReferenceBook transactionReferenceBook,
            MoneyConverter moneyConverter)
        {
            Argument.NotNull(operationCategories, "operationCategories");
            Argument.NotNull(transactionReferenceBook, "transactionReferenceBook");
            Argument.NotNull(moneyConverter, "moneyConverter");
            _operationCategories = operationCategories;
            _transactionReferenceBook = transactionReferenceBook;
            _moneyConverter = moneyConverter;
            _settings = new BankSettings();
        }

        public CardTransfer Create(UserCard from, UserCard to, decimal amount)
        {
            Argument.NotNull(from, "from");
            Argument.NotNull(to, "to");

            //TODO: probably better move this code to some another class
            var operationCategoryCode = from.Owner.Id == to.Owner.Id
                ? PersonalTransferOperation 
                : InterbankTransferOperation;
            var operationCategory = _operationCategories.Find(operationCategoryCode);
            if (operationCategory == null)
            {
                throw new InvalidOperationException("Can't find operation category.");
            }
            var transfer = new CardTransfer(operationCategory, from, to, amount);
            var transactionName = _transactionReferenceBook.ForOperation(transfer);
            transfer.Withdrawal = transfer.From.Withdraw(from, transactionName.Code, transactionName.Description,
                _settings.Location, transfer.MoneyAmount, _moneyConverter);
            return transfer;
        }        
    }
}
