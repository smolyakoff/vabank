using System;
using System.Linq;
using System.Linq.Expressions;
using AutoMapper;
using VaBank.Common.Data;
using VaBank.Common.Data.Filtering;
using VaBank.Core.Common;
using VaBank.Services.Contracts.Common.Queries;

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

        public static TModel Map<TModel>(this Entity entity)
        {
            return Mapper.Map<Entity, TModel>(entity);
        }

        public static IdentityQuery<TKey> SetConcreteFilter<TEntity, TKey>(this IdentityQuery<TKey> query, Expression<Func<TEntity, TKey>> keySelector)
            where TEntity : Entity

        {
            if (query == null)
            {
                throw new ArgumentNullException("query");
            }
            if (keySelector == null)
            {
                throw new ArgumentNullException("query");
            }
            var memberAccess = (MemberExpression)keySelector.Body;
            var filter = string.Format("{0} == @0", memberAccess.Member.Name);
            query.Filter = new DynamicLinqFilter(filter, query.Id);
            return query;
        }
    }
}
