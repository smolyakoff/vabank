using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VaBank.Core;
using VaBank.Core.Repositories;
using VaBank.Data.EntityFramework.Repositories;

namespace VaBank.Data.EntityFramework
{
    public class UnitOfWork: IUnitOfWork
    {
        private VaBankContext context;
        private IRepositoryFactory repositoryFactory;

        public UnitOfWork(VaBankContext context)
        {
            this.context = context;
            repositoryFactory = new RepositoryFactory(context);
        }

        public void Commit()
        {
            context.SaveChanges();
        }

        public IRepositoryFactory RepositoryFactory
        {
            get { return repositoryFactory; }
        }
    }
}
