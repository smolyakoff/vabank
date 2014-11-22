using VaBank.Core.Processing.Entities;

namespace VaBank.Services.Processing.Operations
{
    public interface IOperationProcessor : IProcessor<BankOperationProcessorCommand>
    {
        bool CanProcess(BankOperation operation);
    }
}
