using System;

namespace VaBank.Services.Contracts.Common.Commands
{
    public interface IOperationCommand
    {
        Guid OperationId { get; }
    }
}
