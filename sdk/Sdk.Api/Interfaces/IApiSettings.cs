namespace Sdk.Api.Interfaces;

public interface IApiSettings
{
    Type? ValidatorHandlerAssemblyType { get; set; }
    Type? MediatorHandlersAssembyType { get; set; }
    string? ApiName { get; set; }
}