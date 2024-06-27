namespace Sdk.Db.PostgreSQL.HealthChecks;

using Microsoft.Extensions.DependencyInjection;

public static class SetupHealthCheck
{
    public static IHealthChecksBuilder AddSqlServerHealthCheck(
        this IHealthChecksBuilder builder,
        string connectionString)
    {
        builder.AddNpgSql(npgsqlConnectionString: connectionString);

        return builder;
    }
}