using System.Collections;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using VaBank.Common.Validation;

namespace VaBank.Data.EntityFramework.Common
{
    public static class EntityFrameworkExtensions
    {
        public static DbQuery<T> SetIncluding<T>(this DbContext context, string[] includes)
            where T : class 
        {
            Argument.NotNull(context, "context");
            Argument.NotNull(includes, "includes");
            DbQuery<T> query = context.Set<T>();
            return includes.Aggregate(query, (current, include) => current.Include(include));
        }

        public static T Find<T>(this DbContext context, string[] includes, params object[] ids)
            where T : class
        {
            Argument.NotNull(context, "context");
            Argument.NotNull(includes, "includes");
            Argument.NotNull(ids, "ids");

            var entity = context.Set<T>().Find(ids);
            if (entity == null || includes.Length == 0)
            {
                return entity;
            }

            var type = typeof (T);
            var properties = type.GetProperties().Where(x => includes.Contains(x.Name)).ToList();
            var collectionProperties = properties.Where(x => typeof (IEnumerable).IsAssignableFrom(x.PropertyType)).ToList();
            var referenceProperties = properties.Except(collectionProperties).ToList();
            var entry = context.Entry(entity);
            collectionProperties.ForEach(p => entry.Collection(p.Name).Load());
            referenceProperties.ForEach(p => entry.Reference(p.Name).Load());
            return entity;
        }
    }
}
