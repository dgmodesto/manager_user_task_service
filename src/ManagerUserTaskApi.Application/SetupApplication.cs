namespace ManagerUserTaskApi.Application;

using Events;
using Handlers;
using Infrastructure.Database.Entities;
using ManagerUserTaskApi.Infrastructure.Cache;
using ManagerUserTaskApi.Infrastructure.Services;
using Mappers;
using Mappers.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Sdk.Db.Audit;

public static class SetupApplication
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddAuditEvents();
        services.AddMappers();
        services.AddServices();
        services.AddCache();
        return services;
    }

    private static IServiceCollection AddAuditEvents(this IServiceCollection services)
    {
        services.AddScoped<AuditEventHandler<UserTaskCreated, UserTask>, UserTaskCreatedHandler>();
        services.AddScoped<AuditEventHandler<UserTaskUpdated, UserTask>, UserTaskUpdatedHandler>();
        services.AddScoped<AuditEventHandler<UserTaskDeleted, UserTask>, UserTaskDeletedHandler>();

        return services;
    }

    private static IServiceCollection AddMappers(this IServiceCollection services)
    {
        services.AddScoped<IUserTaskMapper, UserTaskMapper>();
        services.AddScoped<IAuthMapper, AuthMapper>();

        return services;
    }
}