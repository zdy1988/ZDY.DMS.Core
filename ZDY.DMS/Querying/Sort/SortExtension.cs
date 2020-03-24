using System;
using System.Linq;
using System.Linq.Expressions;

namespace ZDY.DMS.Querying
{
    public static class SortExtension
    {
        public static IOrderedQueryable<T> Sort<T>(this IQueryable<T> source, string orderField, bool isAsc)
        {
            if (string.IsNullOrEmpty(orderField))
                throw new ArgumentOutOfRangeException("orderField", orderField, "基于字段的排序必须指定排序字段");

            string[] props = orderField.Split('.');
            string methodName = isAsc ? "OrderBy" : "OrderByDescending";
            Type type = typeof(T);
            ParameterExpression arg = Expression.Parameter(type, "x");
            Expression expr = arg;
            foreach (string prop in props)
            {
                System.Reflection.PropertyInfo pi = type.GetProperty(prop);
                expr = Expression.Property(expr, pi);
                type = pi.PropertyType;
            }
            Type delegateType = typeof(Func<,>).MakeGenericType(typeof(T), type);
            LambdaExpression lambda = Expression.Lambda(delegateType, expr, arg);

            object result = typeof(Queryable).GetMethods().Single(
                    method => method.Name == methodName
                            && method.IsGenericMethodDefinition
                            && method.GetGenericArguments().Length == 2
                            && method.GetParameters().Length == 2)
                    .MakeGenericMethod(typeof(T), type)
                    .Invoke(null, new object[] { source, lambda });
            return (IOrderedQueryable<T>)result;
        }
    }
}
