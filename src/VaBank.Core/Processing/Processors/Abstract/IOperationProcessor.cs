using VaBank.Core.Processing.Entities;

namespace VaBank.Core.Processing.Processors.Abstract
{
    //Actually a state machine :)
    public interface IOperationProcessor<in TOperation>
        where TOperation : BankOperation
    {
        OperationProcessorResult Process(TOperation operation);
    }
}
