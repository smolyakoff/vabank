using System;
using VaBank.Common.Validation;
using VaBank.Core.Common;

namespace VaBank.Core.Processing.Entities
{
    public abstract class BankOperation : Entity<long>, IBankOperation
    { 
        protected BankOperation(OperationCategory category) : this()
        {
            Argument.NotNull(category, "category");
            Category = category;
        }

        protected BankOperation()
        {
            CreatedDateUtc = DateTime.UtcNow;
        }

        public virtual OperationCategory Category { get; protected set; }

        public virtual ProcessStatus Status { get; protected set; }

        public DateTime CreatedDateUtc { get; private set; }

        public DateTime? CompletedDateUtc { get; protected set; }

        public string ErrorMessage { get; protected set; }

        public void Fail(string message)
        {
            Argument.NotEmpty(message, "message");

            ErrorMessage = message;
            Status = ProcessStatus.Failed;
        }

        public void Complete()
        {
            CompletedDateUtc = DateTime.UtcNow;
            Status = ProcessStatus.Completed;
        }
    }
}
