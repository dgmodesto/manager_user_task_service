namespace Sdk.Api;

using Interfaces;

public class HealthCheckOverride : IHealthCheckOverride
{
    public string LivenessRoute { get; set; } = "/healthCheck/liveness";
    public string ReadinessRoute { get; set; } = "/healthCheck/readiness";
}