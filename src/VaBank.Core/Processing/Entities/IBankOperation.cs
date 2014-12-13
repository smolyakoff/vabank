using System;
using VaBank.Core.Accounting.Entities;

namespace VaBank.Core.Processing.Entities
{
    public interface IBankOperation
    {
        OperationCategory Category { get; }

        ProcessStatus Status { get; }

        DateTime CreatedDateUtc { get; }

        DateTime? CompletedDateUtc { get; }

        string ErrorMessage { get; }

        long Id { get; }
    }
}
