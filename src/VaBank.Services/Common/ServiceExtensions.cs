using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;
using AutoMapper;
using VaBank.Common.Data;
using VaBank.Common.Data.Repositories;
using VaBank.Core.Common;
using VaBank.Services.Common.Exceptions;

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

        public static T SurelyFind<T>(this IRepository<T> repository, params object[] keys)
            where T : class
        {
            var entity = repository.Find(keys);
            if (entity == null)
            {
                throw NotFound.ExceptionFor<T>(keys);
            }
            return entity;
        }

        public static DbQuery<TEntity> ToDbQuery<TEntity, T>(this IIdentityQuery<T> id)
            where TEntity: Entity<T>
        {
            if (id == null)
            {
                throw new ArgumentNullException("id");
            }
            return id.ToDbQuery<TEntity, T>(x => x.Id);
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

        public static IEnumerable<TDestination> Map<TSource, TDestination>(this IEnumerable<TSource> enumerable)
        {
            return enumerable == null 
                ? null 
                : enumerable.Select(Mapper.Map<TSource, TDestination>);
        }
    }
}
