namespace ManagerUserTaskApi.Infrastructure.Database.Factories;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

public class ManagerUserTaskApiDbContextFactory : IDesignTimeDbContextFactory<ManagerUserTaskApiDbContext>
{
    public ManagerUserTaskApiDbContext CreateDbContext(string[] args)
    {
        var builder = new DbContextOptionsBuilder<ManagerUserTaskApiDbContext>();
        builder.UseNpgsql("");

        return new ManagerUserTaskApiDbContext(builder.Options);
    }
}