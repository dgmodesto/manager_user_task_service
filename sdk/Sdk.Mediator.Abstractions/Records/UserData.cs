namespace Sdk.Mediator.Abstractions.Records;

public record UserData
{
    public UserData(Guid id, string name)
    {
        Id = id;
        Name = name;
    }

    public Guid Id { get; init; }

    public string Name { get; init; }
}