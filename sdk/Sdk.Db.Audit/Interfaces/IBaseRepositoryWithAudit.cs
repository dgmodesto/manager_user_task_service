namespace Sdk.Db.Audit.Interfaces;

using Abstractions.Entities;
using Abstractions.Interfaces;
using System.Linq.Expressions;

/// <summary>
///     This interface contains the methods of data repositories, but it emits events for methods that manipulate the data.
/// </summary>
/// <typeparam name="TEntity">The entity that had its data manipulated</typeparam>
public interface IBaseRepositoryWithAudit<TEntity> : IBaseRepository<TEntity>
    where TEntity : Entity
{
    /// <summary>
    ///     Inserts the entity and emits an event containing the manipulated data and metadata of the user who manipulated
    ///     them.
    /// </summary>
    /// <typeparam name="TEvent">The event to be triggered</typeparam>
    /// <param name="entity">The entity to be inserted</param>
    /// <returns></returns>
    Task InsertAsync<TEvent>(TEntity entity)
        where TEvent : AuditEvent<TEntity>;

    /// <summary>
    ///     Inserts a list of entities and emits an event per entity with metadata of the user who manipulated them.
    /// </summary>
    /// <typeparam name="TEvent">The event to be triggered</typeparam>
    /// <param name="entities">List of entities to be inserted</param>
    /// <returns></returns>
    Task InsertRangeAsync<TEvent>(IList<TEntity> entities)
        where TEvent : AuditEvent<TEntity>;

    /// <summary>
    ///     Update an entity and an event containing the manipulated data and metadata of the user who manipulated them.
    /// </summary>
    /// <typeparam name="TEvent">The event to be triggered</typeparam>
    /// <param name="version">The version of the data being changed</param>
    /// <param name="entity">The entity to be updated</param>
    /// <returns></returns>
    Task UpdateAsync<TEvent>(TEntity entity)
        where TEvent : AuditEvent<TEntity>;

    /// <summary>
    ///     Delete an entity and an event containing the manipulated data and metadata of the user who manipulated them.
    /// </summary>
    /// <typeparam name="TEvent">The event to be triggered</typeparam>
    /// <param name="entity">The entity to be deleted</param>
    /// <returns></returns>
    Task DeleteAsync<TEvent>(TEntity entity)
        where TEvent : AuditEvent<TEntity>;

    /// <summary>
    ///     Delete all the entities returned by querying an expression and emits an event per entity with metadata of the user
    ///     who manipulated them.
    /// </summary>
    /// <typeparam name="TEvent">The event to be triggered</typeparam>
    /// <typeparam name="TResponse">The response model to be audited</typeparam>
    /// <param name="expression">The expression to query the database</param>
    /// <returns></returns>
    Task DeleteByExpressionAsync<TEvent>(Expression<Func<TEntity, bool>> expression)
        where TEvent : AuditEvent<TEntity>;
}