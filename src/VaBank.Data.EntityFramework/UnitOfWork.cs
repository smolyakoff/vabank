using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VaBank.Core;

namespace VaBank.Data.EntityFramework
{
    public class UnitOfWork: IUnitOfWork
    {
        private VaBankContext context;

        public UnitOfWork(VaBankContext context)
        {
            this.context = context;
        }

        public void Save()
        {
            context.SaveChanges();
        }
    }
}
