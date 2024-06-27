namespace Sdk.Db.EventStore.DbContexts;

using Entities;
using Mappings;
using Microsoft.EntityFrameworkCore;

public class BaseEventStoreDbContext<T> : DbContext
    where T : DbContext
{
    public BaseEventStoreDbContext(DbContextOptions<BaseEventStoreDbContext<T>> options) : base(options)
    {
    }

    public virtual DbSet<EventRecord>? EventStore { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new EventStoreMap());
        base.OnModelCreating(modelBuilder);
    }
}