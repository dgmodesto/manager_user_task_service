namespace Sdk.Mediator;

using Abstractions.Interfaces;
using Abstractions.Notifications;
using Behaviors;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Notifications;
using System;

public static class Setup
{
    public static IServiceCollection AddMediatorSdk(this IServiceCollection services,
        Action<IMediatorSettings> setupAction)
    {
        IMediatorSettings settings = new MediatorSettings();
        setupAction.Invoke(settings);

        if (settings.MediatorHandlersAssembyType is null)
            throw new InvalidOperationException($"{nameof(settings.MediatorHandlersAssembyType)} is null");

        services.AddScoped<IMediatorHandler, InMemoryBus>();
        services.AddScoped<INotificationHandler<DomainNotification>, DomainNotificationHandler>();

        AssemblyScanner
            .FindValidatorsInAssemblyContaining(settings.ValidatorHandlerAssemblyType)
            .ForEach(result => services.AddScoped(result.InterfaceType, result.ValidatorType));

        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(FailFastBehavior<,>));

        services.AddMediatR(settings.MediatorHandlersAssembyType);

        return services;
    }
}