namespace Sdk.Db.Abstractions.Extensions;

using Microsoft.EntityFrameworkCore;
using Pagination;
using System.Linq.Expressions;
using System.Reflection;

public static class QueryableExtensions
{
    public static IQueryable<TEntity> IncludeProperties<TEntity>(this IQueryable<TEntity> entities,
        params Expression<Func<TEntity, object>>[] properties) where TEntity : class
    {
        if (properties != null)
            entities = properties.Aggregate(entities, (current, include) => current.Include(include));

        return entities;
    }

    public static IQueryable<TEntity>? Order<TEntity>(this IQueryable<TEntity> entities, Order order)
        where TEntity : class
    {
        if (entities == null || !entities.Any() || order == null || string.IsNullOrWhiteSpace(order.Property))
            return entities;

        var method = order.Crescent ? "OrderBy" : "OrderByDescending";
        var methodCallExpression = CreateMethodCallExpression(entities, method, order.Property);
        return entities.Provider.CreateQuery<TEntity>(methodCallExpression);
    }

    #region private methods

    private static MethodCallExpression CreateMethodCallExpression<TEntity>(IQueryable<TEntity> entities,
        string method, string property, LambdaExpression? lambdaExpression = null) where TEntity : class
    {
        var entityType = typeof(TEntity);
        MethodCallExpression methodCallExpression;

        if (lambdaExpression == null)
        {
            Type propertyType;
            lambdaExpression = CreateLambdaExpression<TEntity>(property, out propertyType);
            methodCallExpression = Expression.Call(typeof(Queryable), method, new[] { entityType, propertyType },
                entities.Expression, Expression.Quote(lambdaExpression));
        }
        else
        {
            methodCallExpression = Expression.Call(typeof(Queryable), "Where", new[] { entities.ElementType },
                entities.Expression, lambdaExpression);
        }

        return methodCallExpression;
    }

    private static LambdaExpression CreateLambdaExpression<TEntity>(string property, out Type propertyType)
        where TEntity : class
    {
        var parameter = Expression.Parameter(typeof(TEntity), "Entity");

        PropertyInfo propertyInfo;
        Expression accessProperty;

        if (property.Contains('.'))
        {
            var childProperties = property.Split('.');
            propertyInfo = typeof(TEntity).GetProperty(childProperties[0])!;
            accessProperty = Expression.MakeMemberAccess(parameter, propertyInfo);

            for (var i = 1; i < childProperties.Length; i++)
            {
                propertyInfo = propertyInfo.PropertyType.GetProperty(childProperties[i])!;
                accessProperty = Expression.MakeMemberAccess(accessProperty, propertyInfo);
            }
        }
        else
        {
            propertyInfo = typeof(TEntity).GetProperty(property)!;
            accessProperty = Expression.MakeMemberAccess(parameter, propertyInfo);
        }

        propertyType = propertyInfo.PropertyType;
        return Expression.Lambda(accessProperty, parameter);
    }

    #endregion
}