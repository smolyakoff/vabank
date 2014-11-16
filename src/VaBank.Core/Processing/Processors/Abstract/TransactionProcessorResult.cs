using System;

namespace VaBank.Core.Processing.Processors.Abstract
{
    public class TransactionProcessorResult
    {
        public bool NotifyOperationProcessor { get; private set; }
 
        public DateTime? RescheduledDateUtc { get; private set; }
    }
}
