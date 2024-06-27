namespace Sdk.Db.PostgreSQL;

using Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

public static class SetupPostgreSQL
{
    public static PostgreSQLContext AddDatabase<TContext>(this IServiceCollection services, string? connectionString)
        where TContext : DbContext
    {
        var context = new PostgreSQLContext(services);
        context.AddPostgres<TContext>(connectionString, ServiceLifetime.Scoped, typeof(TContext));

        return context;
    }

    public static Migration MigrateDatabase<TContext>(this IServiceProvider serviceProvider)
        where TContext : DbContext
    {
        var migration = new Migration(serviceProvider);
        migration.Migrate<TContext>();

        return migration;
    }
}