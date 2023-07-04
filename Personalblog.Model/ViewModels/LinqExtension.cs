﻿using System.Linq.Expressions;
using System.Reflection;

namespace Personalblog.Model.ViewModels
{
    public static class LinqExtension
    {
        public static IQueryable<T> OrderBy<T>(this IQueryable<T> query, string field, bool desc)
        {
            if (!string.IsNullOrWhiteSpace(field))
            {
                ParameterExpression p = Expression.Parameter(typeof(T));
                Expression key = Expression.Property(p, field);
                var propInfo = GetPropertyInfo(typeof(T), field);
                var expr = GetOrderExpression(typeof(T), propInfo);
                if (desc)
                {
                    var method = typeof(Queryable).GetMethods().FirstOrDefault(m => m.Name == "OrderByDescending" && m.GetParameters().Length == 2);
                    var genericMethod = method.MakeGenericMethod(typeof(T), propInfo.PropertyType);
                    return (IQueryable<T>)genericMethod.Invoke(null, new object[] { query, expr });
                }
                else
                {
                    var method = typeof(Queryable).GetMethods().FirstOrDefault(m => m.Name == "OrderBy" && m.GetParameters().Length == 2);
                    var genericMethod = method.MakeGenericMethod(typeof(T), propInfo.PropertyType);
                    return (IQueryable<T>)genericMethod.Invoke(null, new object[] { query, expr });
                }
            }
            return query;
        }
        /// <summary>
        /// 获取反射
        /// </summary>
        /// <param name="objType"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        private static PropertyInfo GetPropertyInfo(Type objType, string name)
        {
            var properties = objType.GetProperties();
            var matchedProperty = properties.FirstOrDefault(p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
            if (matchedProperty == null)
                throw new ArgumentException("对象不包含指定属性名");

            return matchedProperty;
        }
        /// <summary>
        /// 获取生成表达式
        /// </summary>
        /// <param name="objType"></param>
        /// <param name="pi"></param>
        /// <returns></returns>
        private static LambdaExpression GetOrderExpression(Type objType, PropertyInfo pi)
        {
            var paramExpr = Expression.Parameter(objType);
            var propAccess = Expression.PropertyOrField(paramExpr, pi.Name);
            var expr = Expression.Lambda(propAccess, paramExpr);
            return expr;
        }
    }
}
