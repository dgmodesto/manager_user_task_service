namespace ManagerUserTaskApi.Infrastructure.Database.Factories;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Sdk.Db.EventStore.DbContexts;

internal class EventStoreDbContextFactory : IDesignTimeDbContextFactory<EventStoreDbContext>
{
    public EventStoreDbContext CreateDbContext(string[] args)
    {
        var builder = new DbContextOptionsBuilder<BaseEventStoreDbContext<EventStoreDbContext>>();
        builder.UseNpgsql("");

        return new EventStoreDbContext(builder.Options);
    }
}