using System;
using System.Security.Claims;

namespace VaBank.Core.App
{
    public interface IOperationRepository
    {
        Operation Start(string name = null, ClaimsIdentity identity = null);

        void Stop(Operation operation);

        Operation Find(Guid operationId);
    }
}
