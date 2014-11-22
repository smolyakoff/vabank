using System.Collections.Generic;
using VaBank.Services.Contracts.Common.Commands;
using VaBank.Services.Contracts.Common.Events;

namespace VaBank.Services.Processing
{
    public interface IProcessor<in TCommand> 
        where TCommand : IOperationCommand
    {
        IEnumerable<ApplicationEvent> Process(TCommand command);
    }
}
