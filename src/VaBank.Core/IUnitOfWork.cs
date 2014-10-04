using System;

namespace VaBank.Core
{
    public interface IUnitOfWork : IDisposable
    {
        void Commit();
    }
}
