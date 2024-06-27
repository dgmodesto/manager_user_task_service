namespace Sdk.Api.Interfaces;

public interface IHealthCheckOverride
{
    string LivenessRoute { get; set; }
    string ReadinessRoute { get; set; }
}