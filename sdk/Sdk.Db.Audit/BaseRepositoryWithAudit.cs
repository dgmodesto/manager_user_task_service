namespace Sdk.Db.Audit;

using Abstractions.Entities;
using Abstractions.Repositories;
using Interfaces;
using Mediator.Abstractions.Enums;
using Mediator.Abstractions.Interfaces;
using Mediator.Abstractions.Notifications;
using Mediator.Abstractions.Records;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

public abstract class BaseRepositoryWithAudit<TEntity> : BaseRepository<TEntity>, IBaseRepositoryWithAudit<TEntity>
    where TEntity : Entity
{
    private readonly IMediatorHandler _mediator;

    protected BaseRepositoryWithAudit(DbContext context, IMediatorHandler mediator) : base(context)
    {
        _mediator = mediator;
    }

    public async Task DeleteAsync<TEvent>(TEntity entity)
        where TEvent : AuditEvent<TEntity>
    {
        await DeleteAsync(entity);
        await Audit<TEvent>(entity);
    }

    public async Task DeleteByExpressionAsync<TEvent>(Expression<Func<TEntity, bool>> expression)
        where TEvent : AuditEvent<TEntity>
    {
        var removedItems = await DeleteByExpressionAsync(expression);

        if (removedItems is { } && removedItems.Count > 0)
            foreach (var item in removedItems)
                await Audit<TEvent>(item);
    }

    public async Task InsertAsync<TEvent>(TEntity entity)
        where TEvent : AuditEvent<TEntity>
    {
        await InsertAsync(entity);
        await Audit<TEvent>(entity);
    }

    public async Task InsertRangeAsync<TEvent>(IList<TEntity> entities)
        where TEvent : AuditEvent<TEntity>
    {
        if (entities.Count > 0)
        {
            await InsertRangeAsync(entities);

            foreach (var item in entities) await Audit<TEvent>(item);
        }
    }

    public async Task UpdateAsync<TEvent>(TEntity entity)
        where TEvent : AuditEvent<TEntity>
    {
        try
        {
            await UpdateAsync(entity);

            var updatedEntity = await GetByIdAsync(entity.Id);
            await Audit<TEvent>(updatedEntity!);
        }
        catch (DbUpdateConcurrencyException ce)
        {
            const string errorMessage = "Concurrency Exception! The reported version is not the latest.";
            Console.WriteLine(ce);
            await _mediator.RaiseEvent(new DomainNotification(
                FailureReason.BadRequest,
                FailureReason.BadRequest.ToString(),
                errorMessage));
        }
    }

    private async Task Audit<TEvent>(TEntity entity)
        where TEvent : AuditEvent<TEntity>
    {
        var userData = new UserData(Guid.NewGuid(), "Teste"); // get from token
        await _mediator.RaiseEvent((AuditEventInstanceCreator.Create<TEntity, TEvent>(entity, userData) as TEvent)!);
    }
}