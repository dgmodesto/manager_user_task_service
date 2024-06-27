namespace ManagerUserTaskApi.Infrastructure.Database;

using Microsoft.EntityFrameworkCore;
using Sdk.Db.EventStore.DbContexts;

public class EventStoreDbContext : BaseEventStoreDbContext<EventStoreDbContext>
{
    public EventStoreDbContext(DbContextOptions<BaseEventStoreDbContext<EventStoreDbContext>> options) : base(options)
    {
    }
}