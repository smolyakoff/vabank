using System.Collections.Generic;
using VaBank.Common.Events;
using VaBank.Common.Validation;

namespace VaBank.Services.Contracts.Processing.Models
{
    public class TransactionProcessingResult : ProcessingResult
    {
         readonly TransactionModel _transaction;

        public TransactionProcessingResult(TransactionModel transaction, IEnumerable<IEvent> transactionalEvents) : base(transactionalEvents)
        {
            Argument.NotNull(transaction, "transaction");
            _transaction = transaction;
        }

        public TransactionModel Transaction
        {
            get { return _transaction; }
        }
    }
}
