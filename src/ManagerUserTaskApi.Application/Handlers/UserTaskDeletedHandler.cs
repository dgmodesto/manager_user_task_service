namespace ManagerUserTaskApi.Application.Handlers;

using Events;
using Infrastructure.Database.Entities;
using Sdk.Db.Audit;
using Sdk.Db.EventStore.Interfaces;
using Sdk.Mediator.Abstractions.Interfaces;

public class UserTaskDeletedHandler : AuditEventHandler<UserTaskDeleted, UserTask>
{
    public UserTaskDeletedHandler(IMediatorHandler mediator, IEventStoreRepository eventStoreRepository) : base(mediator,
        eventStoreRepository)
    {
    }

    // uncomment this code snippet to add custom handlers for the event
    // public override Task PublishEvent(TaskDeleted @event)
    // {
    //     // e.g. notify in a message queue
    //     return base.PublishEvent(@event);
    // }
}