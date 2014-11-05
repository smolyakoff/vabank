namespace VaBank.Services.Contracts.Common.Events
{
    public interface IAuditedEvent : IOperationEvent
    {
        string Code { get; }

        string Description { get; }

        object Data { get; }
    }
}
