using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using VaBank.Common.Events;
using VaBank.Common.Validation;

namespace VaBank.Services.Contracts.Processing.Models
{
    public abstract class ProcessingResult
    {
        protected ProcessingResult(IEnumerable<IEvent> transactionalEvents)
        {
            Argument.NotNull(transactionalEvents, "transactionalEvents");
            TransactionalEvents = new ReadOnlyCollection<IEvent>(transactionalEvents.ToList());
        }

        public IReadOnlyCollection<IEvent> TransactionalEvents { get; set; } 
    }
}
