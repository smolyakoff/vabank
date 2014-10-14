using System;

namespace VaBank.Core.App
{
    public interface IOperationMarkerRepository
    {
        OperationMarker Take(string name, Guid? userId, string clientId);

        void Release(OperationMarker marker);
    }
}
