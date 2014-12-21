using System;
using VaBank.Common.Data.Linq;
using VaBank.Common.Validation;
using VaBank.Core.Accounting.Entities;
using VaBank.Core.Common;

namespace VaBank.Core.Processing.Entities
{
    public abstract class BankOperation : Entity<long>, IBankOperation
    { 
        public static class Spec
        {
            public static LinqSpec<BankOperation> Finished = 
                LinqSpec.For<BankOperation>(x => x.Status == ProcessStatus.Completed || x.Status == ProcessStatus.Failed); 
        }

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
            if (Status != ProcessStatus.Pending)
            {
                throw new DomainException("Current state is already final.");
            }

            ErrorMessage = message;
            Status = ProcessStatus.Failed;
        }

        public void Complete()
        {
            if (Status != ProcessStatus.Pending)
            {
                throw new DomainException("Current state is already final.");
            }

            CompletedDateUtc = DateTime.UtcNow;
            Status = ProcessStatus.Completed;
        }
    }
}
