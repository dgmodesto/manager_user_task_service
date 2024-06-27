namespace ManagerUserTaskApi.Infrastructure.Services;

using Polly;
using Polly.Extensions.Http;
using System.Net;

/// <summary>
///     https://github.com/App-vNext/Polly/wiki/Polly-and-HttpClientFactory
/// </summary>
public static class Resilience
{
    public static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
    {
        var jitterer = new Random();

        return HttpPolicyExtensions
            .HandleTransientHttpError()
            .OrResult(msg => msg.StatusCode == HttpStatusCode.NotFound)
            .WaitAndRetryAsync(6,
                retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)) +
                                TimeSpan.FromMilliseconds(jitterer.Next(0, 1000)));
    }

    /// <summary>
    ///     https://learn.microsoft.com/en-us/azure/architecture/patterns/circuit-breaker
    ///     https://learn.microsoft.com/en-us/dotnet/architecture/microservices/implement-resilient-applications/implement-circuit-breaker-pattern
    /// </summary>
    public static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy()
    {
        return HttpPolicyExtensions
            .HandleTransientHttpError()
            .CircuitBreakerAsync(5, TimeSpan.FromSeconds(30));
    }
}