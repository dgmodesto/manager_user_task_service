namespace Sdk.Api;

using Interfaces;

public class ApiSettings : IApiSettings
{
    public Type? ValidatorHandlerAssemblyType { get; set; }
    public Type? MediatorHandlersAssembyType { get; set; }
    public string? ApiName { get; set; }
}