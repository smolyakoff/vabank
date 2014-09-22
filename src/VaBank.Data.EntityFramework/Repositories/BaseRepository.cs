using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VaBank.Core.Repositories;

namespace VaBank.Data.EntityFramework.Repositories
{
    public class BaseRepository<TEntity, TKey>: IRepository<TEntity, TKey>
    {
    }
}
