using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VaBank.Core.Entities;
using VaBank.Core.Repositories;

namespace VaBank.Data.EntityFramework.Repositories
{
    public class RepositoryFactory: IRepositoryFactory
    {
        private VaBankContext context;
        private IRepository<Log, long> logRepository;

        public RepositoryFactory(VaBankContext context)
        {
            this.context = context;
        }
        public IRepository<Core.Entities.Log, long> LogRepository
        {
            get { return logRepository ?? (logRepository = new Repository<Log, long>(context)); }
        }
    }
}
