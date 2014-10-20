using System;

namespace VaBank.Services.Contracts.Maintenance
{
    public interface IOperationService : IService
    {
        bool HasCurrent { get; }

        Guid Current { get; }

        void Stop(Guid operationId);
    }
}
