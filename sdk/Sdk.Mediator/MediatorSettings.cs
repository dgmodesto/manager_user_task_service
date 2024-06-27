namespace Sdk.Mediator;

using Abstractions.Interfaces;

internal class MediatorSettings : IMediatorSettings
{
    public Type? ValidatorHandlerAssemblyType { get; set; }
    public Type? MediatorHandlersAssembyType { get; set; }
}