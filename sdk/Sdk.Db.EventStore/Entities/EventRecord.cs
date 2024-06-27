namespace Sdk.Db.EventStore.Entities;

using System.ComponentModel.DataAnnotations;

public class EventRecord
{
    public EventRecord(Guid id, string entityType, string storeType, Guid userId, string userName, DateTime timeStamp,
        string data)
    {
        Id = id;
        EntityType = entityType;
        StoreType = storeType;
        UserId = userId;
        UserName = userName;
        TimeStamp = timeStamp;
        Data = data;
    }

    [Key]
    public virtual Guid Id { get; protected set; }

    public string EntityType { get; protected set; }

    public string StoreType { get; protected set; }

    public Guid UserId { get; protected set; }

    public string UserName { get; protected set; }

    public DateTime TimeStamp { get; protected set; }

    public string Data { get; protected set; }
}