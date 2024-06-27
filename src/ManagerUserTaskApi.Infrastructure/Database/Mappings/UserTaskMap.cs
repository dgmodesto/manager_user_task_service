namespace ManagerUserTaskApi.Infrastructure.Database.Mappings;

using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class UserTaskMap : IEntityTypeConfiguration<UserTask>
{
    public void Configure(EntityTypeBuilder<UserTask> builder)
    {
        builder.HasKey(c => c.Id);
        builder.Property(c => c.User).IsRequired().HasMaxLength(50);
        builder.Property(c => c.Date).IsRequired();
        builder.Property(c => c.StartTime).IsRequired();
        builder.Property(c => c.EndTime).IsRequired();
        builder.Property(c => c.Subject).HasMaxLength(300).IsRequired();
        builder.Property(c => c.Description).HasMaxLength(500).IsRequired();


        builder.Property(c => c.CreatedAt);
        builder.Property(c => c.Version).IsConcurrencyToken();
        builder.Property(c => c.UpdatedAt);
    }
}