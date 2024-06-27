namespace Sdk.Db.Abstractions.Interfaces;

public interface IEntity
{
    Guid Id { get; }
    DateTime CreatedAt { get; }
    DateTime? UpdatedAt { get; }
    int Version { get; }

    void SetId(Guid id);
    void SetVersion(int version);
}