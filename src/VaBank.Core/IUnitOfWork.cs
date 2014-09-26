using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VaBank.Core.Repositories;

namespace VaBank.Core
{
    public interface IUnitOfWork
    {
        IRepositoryFactory RepositoryFactory { get; }
        void Commit();
    }
}
