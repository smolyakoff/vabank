using System;
using System.Linq;
using AutoMapper;
using VaBank.Core.Common;

namespace VaBank.Services.Common
{
    internal static class ServiceExtensions
    {
        public static void EnsureIsResolved(this IRepositoryCollection collection)
        {
            if (collection == null)
            {
                throw new ArgumentException("Repository collection is not resolved");
            }
            var properties = collection.GetType().GetProperties();
            var values = properties.Select(x => x.GetValue(collection));
            if (values.Any(x => x == null))
            {
                throw new ArgumentException("Not all repositories are resolved");
            }
        }

        public static TModel ToClass<T, TModel>(this T obj)
        {
            return Mapper.Map<T, TModel>(obj);
        }

        public static TModel ToModel<TEntity, TModel>(this TEntity entity)
            where TEntity : Entity
        {
            return Mapper.Map<TEntity, TModel>(entity);
        }

        public static TEntity ToEntity<TModel, TEntity>(this TModel model)
            where TEntity : Entity
        {
            return Mapper.Map<TModel, TEntity>(model);
        }
    }
}
