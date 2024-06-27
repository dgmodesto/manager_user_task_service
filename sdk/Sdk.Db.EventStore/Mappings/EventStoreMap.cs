namespace Sdk.Db.EventStore.Mappings;

using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class EventStoreMap : IEntityTypeConfiguration<EventRecord>
{
    public void Configure(EntityTypeBuilder<EventRecord> builder)
    {
        builder.HasKey(c => c.Id);
        builder.Property(c => c.StoreType).HasMaxLength(20).IsRequired().IsUnicode();
        builder.Property(c => c.UserId);
        builder.Property(c => c.UserName);
        builder.Property(c => c.TimeStamp);
        builder.Property(c => c.Data);
    }
}