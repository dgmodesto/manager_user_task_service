namespace Sdk.Db.Audit;

using Abstractions.Entities;
using EventStore.Entities;
using EventStore.Interfaces;
using Mediator.Abstractions.Handlers;
using Mediator.Abstractions.Interfaces;
using System.Text.Json;
using System.Text.Json.Serialization;

/// <summary>
///     This class receives and handles all events of type AuditEvent of actions that manipulated information in the
///     database.
/// </summary>
/// <typeparam name="TEvent">The source event</typeparam>
/// <typeparam name="TEntity">The target entity of the change</typeparam>
public abstract class AuditEventHandler<TEvent, TEntity> : BaseEventHandler<TEvent>
    where TEvent : AuditEvent<TEntity>
    where TEntity : Entity
{
    private readonly IEventStoreRepository _eventStoreRepository;

    protected AuditEventHandler(IMediatorHandler mediator, IEventStoreRepository eventStoreRepository) : base(mediator)
    {
        _eventStoreRepository = eventStoreRepository;
    }

    protected override async Task Handle(TEvent @event)
    {
        var serialized = JsonSerializer.Serialize(@event.Entity, new JsonSerializerOptions
        {
            WriteIndented = false,
            ReferenceHandler = ReferenceHandler.IgnoreCycles,
        });

        var entityType = @event.Entity.GetType().Name;
        var storeType = @event.GetType().Name.Replace(entityType, "").Replace("Event", "");

        var eventStore = new EventRecord(
            Guid.NewGuid(),
            entityType,
            storeType,
            @event.User.Id,
            @event.User.Name,
            DateTime.Now.ToUniversalTime(),
            serialized
        );

        await _eventStoreRepository.InsertAsync(eventStore);
        await PublishEvent(@event);
    }

    /// <summary>
    ///     Override this method to do other necessary actions with the event that has been logged for audit
    /// </summary>
    /// <param name="event"></param>
    /// <returns></returns>
    public virtual async Task PublishEvent(TEvent @event)
    {
        await Task.CompletedTask;
    }
}