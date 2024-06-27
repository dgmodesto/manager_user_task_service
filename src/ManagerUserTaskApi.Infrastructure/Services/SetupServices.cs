namespace ManagerUserTaskApi.Infrastructure.Services;

using ManagerUserTaskApi.Infrastructure.Services.Authentication;
using Microsoft.Extensions.DependencyInjection;

public static class SetupServices
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddHttpClient<IAuthenticationService, AuthenticationService>()
            .AddPolicyHandler(Resilience.GetRetryPolicy())
            .AddPolicyHandler(Resilience.GetCircuitBreakerPolicy());

        return services;
    }
}