using ManagerUserTaskApi.Infrastructure.Database.Entities;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace ManagerUserTaskApi.Infrastructure.Cache;

public static class SetupCache
{
    public static IServiceCollection AddCache(this IServiceCollection services)
    {
        services.AddSingleton<IConnectionMultiplexer>(sp =>
        {
            var configuration = ConfigurationOptions.Parse(Environment.GetEnvironmentVariable("Redis") ?? string.Empty, true);
            return ConnectionMultiplexer.Connect(configuration);
        });

        services.AddTransient<ICacheService<UserTask>, CacheService<UserTask>>();

        return services;
    }

}
