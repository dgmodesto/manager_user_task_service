namespace Sdk.Db.PostgreSQL;

using EventStore.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

public sealed class PostgreSQLContext
{
    private readonly IServiceCollection _services;

    public PostgreSQLContext(IServiceCollection services)
    {
        _services = services;
    }

    public IServiceCollection WithEventStore<TContext>(string connectionString)
        where TContext : DbContext
    {
        _services.AddEntityFrameworkNpgsql().AddDbContext<BaseEventStoreDbContext<TContext>>(opt =>
        {
            opt.UseNpgsql(connectionString);
            opt.EnableSensitiveDataLogging();
            opt.EnableDetailedErrors();
        }, ServiceLifetime.Singleton, ServiceLifetime.Singleton);

        AddPostgres<TContext>(connectionString, ServiceLifetime.Singleton, typeof(TContext));

        return _services;
    }

    public void AddPostgres<TContext>(string? connectionString, ServiceLifetime service, Type migrationsAssemblyType)
        where TContext : DbContext
    {
        if (string.IsNullOrEmpty(connectionString))
            throw new ArgumentNullException(nameof(connectionString));

        if (migrationsAssemblyType is null)
            throw new ArgumentNullException(nameof(migrationsAssemblyType));

        var assembly = AppDomain.CurrentDomain.Load(migrationsAssemblyType.Assembly.GetName()!);

        _services.AddEntityFrameworkNpgsql().AddDbContext<TContext>(opt =>
        {
            opt.UseNpgsql(connectionString, m => m.MigrationsAssembly(assembly.ToString()));
            opt.EnableSensitiveDataLogging();
            opt.EnableDetailedErrors();
        }, service, service);
    }
}