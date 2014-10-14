using System;
using VaBank.Core.Common;

namespace VaBank.Core.App
{
    public class OperationMarker : Entity
    {
        public OperationMarker(Guid id, string name, Guid? userId, string clientName)
        {
            //TODO: argument checking
            Id = id;
            Name = name;
            UserId = userId;
            ClientName = clientName;
        }

        protected OperationMarker()
        {
            
        }

        public Guid Id { get; private set; }

        public Guid? UserId { get; private set; }

        public string ClientName { get; private set; }

        public string Name { get; private set; }
    }
}
