using System;

namespace VaBank.Core.App
{
    public abstract class Resource
    {
        protected Resource()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; protected set; }

        public string Location { get; protected set; }
    }
}
