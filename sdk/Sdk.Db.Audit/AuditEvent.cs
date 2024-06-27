namespace Sdk.Db.Audit;

using Abstractions.Entities;
using Mediator.Abstractions.Events;
using Mediator.Abstractions.Records;

public class AuditEvent<TEntity> : BaseEvent
    where TEntity : Entity
{
    public AuditEvent(TEntity entity, UserData user)
    {
        Entity = entity;
        User = user;
    }

    public TEntity Entity { get; set; }

    public UserData User { get; set; }
}