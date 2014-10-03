using System;
using System.Collections.Generic;
using System.Linq;

namespace VaBank.Common.Data.Filtering
{
    public static class FilteringExtensions
    {
        private static readonly Dictionary<FilterPropertyType, Type> TypeMapping
            = new Dictionary<FilterPropertyType, Type>
            {
                {FilterPropertyType.Auto, typeof (object)},
                {FilterPropertyType.Boolean, typeof (bool)},
                {FilterPropertyType.Byte, typeof (byte)},
                {FilterPropertyType.Short, typeof (short)},
                {FilterPropertyType.Int, typeof (int)},
                {FilterPropertyType.Long, typeof (long)},
                {FilterPropertyType.Float, typeof (float)},
                {FilterPropertyType.Double, typeof (double)},
                {FilterPropertyType.Decimal, typeof (decimal)},
                {FilterPropertyType.Char, typeof (char)},
                {FilterPropertyType.String, typeof (string)},
                {FilterPropertyType.DateTime, typeof (DateTime)},
                {FilterPropertyType.Guid, typeof (Guid)},
            };

        public static ExpressionFilter<T> StronglyTyped<T>(IFilter filter)
            where T : class
        {
            if (filter == null)
            {
                throw new ArgumentNullException("filter");
            }
            return new ExpressionFilter<T>(filter.ToExpression<T>());
        }

        public static IQueryable<T> Where<T>(this IQueryable<T> queryable, IFilter filter)
            where T : class
        {
            if (queryable == null)
            {
                throw new ArgumentNullException("queryable");
            }
            return queryable.Where(filter.ToExpression<T>());
        }

        public static IFilter And(this IFilter thisFilter, IFilter filter)
        {
            return Combine(thisFilter, filter, FilterLogic.And);
        }

        public static IFilter Or(this IFilter thisFilter, IFilter filter)
        {
            return Combine(thisFilter, filter, FilterLogic.Or);
        }


        public static IFilter Combine(this IFilter thisFilter, IFilter filter, FilterLogic logic)
        {
            if (thisFilter == null)
            {
                throw new ArgumentNullException("thisFilter");
            }
            if (filter == null)
            {
                throw new ArgumentNullException("filter");
            }
            var combined = new CombinedFilter {Logic = logic};
            combined.Filters.Add(thisFilter);
            combined.Filters.Add(filter);
            return combined;
        }

        internal static Type ToType(this FilterPropertyType propertyType)
        {
            return !TypeMapping.ContainsKey(propertyType)
                ? typeof (object)
                : TypeMapping[propertyType];
        }
    }
}