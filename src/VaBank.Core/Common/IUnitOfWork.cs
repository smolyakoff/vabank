using System;

namespace VaBank.Core.Common
{
    public interface IUnitOfWork : IDisposable
    {
        void Commit();
    }
}