using System;
using System.Security.Claims;
using VaBank.Core.App.Entities;

namespace VaBank.Core.App.Repositories
{
    public interface IOperationRepository
    {
        Operation Start(string name = null, ClaimsIdentity identity = null);

        void Stop(Operation operation);

        Operation Find(Guid operationId);
    }
}
