using System.Collections.Generic;
using VaBank.Common.Events;
using VaBank.Common.Validation;

namespace VaBank.Services.Contracts.Processing.Models
{
    public class OperationProcessingResult : ProcessingResult
    {
        private readonly BankOperationModel _operation;

        public OperationProcessingResult(BankOperationModel operation, IEnumerable<IEvent> transactionalEvents) : base(transactionalEvents)
        {
            Argument.NotNull(operation, "operation");
            _operation = operation;
        }
    }
}
