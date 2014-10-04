using System;
using System.Data.Entity;

namespace VaBank.Data.EntityFramework.Repositories
{
    public abstract class BaseRepository
    {
        protected DbContext Context;

        protected BaseRepository(DbContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context", "DbContext can't be null");
            Context = context;
        }
    }
}
