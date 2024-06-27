namespace Sdk.Db.Abstractions.Helpers;

using Attributes;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;

public static class QueryGenerator
{
    public static Expression<Func<TEntity, bool>>? UpdateGenerate<TEntity>(TEntity entity, LogicalOperator @operator)
    {
        var type = entity!.GetType();
        var properties = type.GetProperties();
        Expression<Func<TEntity, bool>>? func = null;

        var propertiesList = properties
            .Where(c => c.GetCustomAttributes(false).Any(a => a is PropertyValidationAttribute)).ToList();

        if (propertiesList.Any())
        {
            var primaryKey = properties.FirstOrDefault(c => c.GetCustomAttributes(false).Any(a => a is KeyAttribute));

            var parameterExpression = Expression.Parameter(typeof(TEntity), "e");

            Expression body = parameterExpression;
            Expression? left = null;
            Expression? right = null;

            if (primaryKey != null)
            {
                var primaryKeyValue = Expression.Constant(primaryKey.GetValue(entity, null));

                right = Expression.Property(body, primaryKey.Name);
                right = Expression.NotEqual(right, primaryKeyValue);

                left = right;
            }

            for (var i = 0; i < propertiesList.Count; i++)
            {
                var value = Expression.Constant(propertiesList[i].GetValue(entity, null));

                right = Expression.Property(body, propertiesList[i].Name);
                right = Expression.Equal(right, value);

                if (left != null)
                {
                    if (@operator == LogicalOperator.And)
                        right = Expression.And(left, right);
                    else
                        right = Expression.Or(left, right);

                    left = right;
                }
                else
                {
                    left = right;
                }
            }

            func = Expression.Lambda<Func<TEntity, bool>>(left!, parameterExpression);
        }

        return func;
    }
}