using System.Collections.Generic;

namespace VaBank.Core.Data
{
    public interface IQueryProcessor
    {
        IList<TEntity> QueryAll<TEntity>(IQuery<TEntity> query)
            where TEntity : class;

        IList<TModel> QueryAll<TEntity, TModel>(IQuery<TModel> query)
            where TEntity : class
            where TModel : class;

        //QueryPage()
        //QueryFirst()
        //Async methods
    }
}
