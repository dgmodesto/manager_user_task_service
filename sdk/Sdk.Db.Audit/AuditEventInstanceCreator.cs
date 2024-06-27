namespace Sdk.Db.Audit;

using Abstractions.Entities;
using Mediator.Abstractions.Records;
using System.Linq.Expressions;

public static class AuditEventInstanceCreator
{
    public static object Create<TEntity, TEvent>(TEntity entity, UserData userData)
        where TEvent : AuditEvent<TEntity>
        where TEntity : Entity
    {
        var constructor = typeof(TEvent).GetConstructor(new[] { typeof(TEntity), typeof(UserData) });

        var parameter1 = Expression.Parameter(typeof(TEntity), "m");
        var parameter2 = Expression.Parameter(typeof(UserData), "u");

        var parameters = new List<Expression> { parameter1, parameter2 };
        var newExpression = Expression.New(constructor!, parameters);

        var creatorExpression =
            Expression.Lambda<Func<TEntity, UserData, TEvent>>(newExpression, parameter1,
                parameter2); // remover parameter1, parameter2
        var creator = creatorExpression.Compile();

        return creator(entity, userData)!;
    }
}