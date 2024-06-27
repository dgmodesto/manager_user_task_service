namespace ManagerUserTaskApi.Api;

using Application;
using Infrastructure.Database;
using ManagerUserTaskApi.Api.Config;
using Sdk.Api;
using Sdk.Api.Authentication;
using Sdk.Api.HealthCheck;
using Sdk.Crypto.Service;
using Sdk.Db.PostgreSQL;
using Sdk.Db.PostgreSQL.HealthChecks;

public static class SetupApi
{
    public static IServiceCollection AddDependencies(
        this IServiceCollection services,
        IConfiguration configuration,
        string? apiName = null)
    {
        // Attach mediator, json options for enums and api versioning with swagger
        services.AddApiSdk(a =>
        {
            a.ValidatorHandlerAssemblyType = typeof(SetupApplication);
            a.MediatorHandlersAssembyType = typeof(SetupApplication);
            a.ApiName = apiName;
        });

        services.AddApiAuthentication();

        services.AddApplication();

        LogConfig.ConfigureLogging();

        // Add databases
        var dbConnectionString = new CryptoService(Environment.GetEnvironmentVariable("Private_Key")).Decrypt(
            Environment.GetEnvironmentVariable("ConnectionStrings__netcore_template_DB")) ?? string.Empty;

        var esConnectionString = new CryptoService(Environment.GetEnvironmentVariable("Private_Key")).Decrypt(
            Environment.GetEnvironmentVariable("ConnectionStrings__event_store_DB")) ?? string.Empty;

        dbConnectionString = Environment.GetEnvironmentVariable("ConnectionStrings_open");

        services
            .AddDatabase<ManagerUserTaskApiDbContext>(dbConnectionString)
            .WithEventStore<EventStoreDbContext>(dbConnectionString)
            .AddRepositories();

        // add health checks
        services
            .AddHealthChecks()
            .AddSqlServerHealthCheck(dbConnectionString);
        services.AddHealthCheckConfiguration(configuration);

        return services;
    }

    public static WebApplication UseMigrations(this WebApplication app)
    {
        app.UseDatabaseMigrations();
        return app;
    }

}