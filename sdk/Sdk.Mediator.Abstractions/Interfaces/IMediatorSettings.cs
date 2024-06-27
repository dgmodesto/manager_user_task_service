namespace Sdk.Mediator.Abstractions.Interfaces;

public interface IMediatorSettings
{
    Type? ValidatorHandlerAssemblyType { get; set; }
    Type? MediatorHandlersAssembyType { get; set; }
}