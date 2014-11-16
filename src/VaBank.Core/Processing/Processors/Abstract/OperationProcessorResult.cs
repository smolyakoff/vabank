using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using VaBank.Common.Validation;
using VaBank.Core.Common;
using VaBank.Core.Processing.Entities;

namespace VaBank.Core.Processing.Processors.Abstract
{
    public class OperationProcessorResult
    {
        public static OperationProcessorResult Empty()
        {
            return new OperationProcessorResult();
        }

        private OperationProcessorResult(DateTime rescheduledDateUtc)
        {
            Argument.EnsureIsValid<FutureDateValidator, DateTime>(rescheduledDateUtc, "rescheduledDateUtc");
            RescheduledDateUtc = rescheduledDateUtc;
        }

        private OperationProcessorResult()
        {
            StartedTransactions = new ReadOnlyCollection<Transaction>(new List<Transaction>());
        }

        public DateTime? RescheduledDateUtc { get; private set; }

        public IReadOnlyCollection<Transaction> StartedTransactions { get; private set; }
    }
}
