using System;
using VaBank.Common.Data.Repositories;
using VaBank.Common.IoC;
using VaBank.Common.Validation;
using VaBank.Core.Accounting.Entities;
using VaBank.Core.Processing.Entities;

namespace VaBank.Core.Processing.Factories
{
    [Injectable]
    public class CardTransferFactory
    {
        private const string PersonalTransferOperation = "TRANSFER-CARD-PERSONAL";
        private const string InterbankTransferOperation = "TRANSFER-CARD-INTERBANK";

        private readonly IRepository<OperationCategory> _operationCategories;

        public CardTransferFactory(IRepository<OperationCategory> operationCategories)
        {
            Argument.NotNull(operationCategories, "operationCategories");
            _operationCategories = operationCategories;
        }

        public CardTransfer Create(UserCard from, UserCard to, decimal amount)
        {
            Argument.NotNull(from, "from");
            Argument.NotNull(to, "to");

            var operationCategoryCode = from.Owner.Id == to.Owner.Id ? PersonalTransferOperation : InterbankTransferOperation;
            var operaionCategory = _operationCategories.Find(operationCategoryCode);

            if (operaionCategory == null)
                new InvalidOperationException("Can't find operation category.");

            return new CardTransfer(operaionCategory, from, to, amount);
        }        
    }
}
