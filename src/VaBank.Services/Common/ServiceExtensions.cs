using System;
using System.Linq;
using AutoMapper;
using VaBank.Core.Common;

namespace VaBank.Services.Common
{
    internal static class ServiceExtensions
    {
        public static void EnsureIsResolved(this IDependencyCollection dependencyCollection)
        {
            if (dependencyCollection == null)
            {
                throw new ArgumentException("Dependency collection is not resolved");
            }
            var properties = dependencyCollection.GetType().GetProperties();
            var values = properties.Select(x => x.GetValue(dependencyCollection));
            if (values.Any(x => x == null))
            {
                throw new ArgumentException("Not all dependencies are resolved");
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
