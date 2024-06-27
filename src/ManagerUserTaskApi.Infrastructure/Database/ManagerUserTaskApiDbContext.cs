namespace ManagerUserTaskApi.Infrastructure.Database;

using Entities;
using Mappings;
using Microsoft.EntityFrameworkCore;

public class ManagerUserTaskApiDbContext : DbContext
{
    public ManagerUserTaskApiDbContext(DbContextOptions<ManagerUserTaskApiDbContext> options) : base(options)
    {
    }

    public virtual DbSet<UserTask>? UserTasks { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserTaskMap());

        base.OnModelCreating(modelBuilder);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        foreach (var entry in ChangeTracker.Entries()
                     .Where(entity => entity.Entity.GetType().GetProperty("CreatedAt") != null))
        {
            if (entry.State == EntityState.Added)
                entry.Property("CreatedAt").CurrentValue = DateTime.Now.ToUniversalTime();

            if (entry.State != EntityState.Modified)
                continue;

            entry.Property("UpdatedAt").CurrentValue = DateTime.Now.ToUniversalTime();
            entry.Property("CreatedAt").IsModified = false;
        }

        return base.SaveChangesAsync(cancellationToken);
    }
}