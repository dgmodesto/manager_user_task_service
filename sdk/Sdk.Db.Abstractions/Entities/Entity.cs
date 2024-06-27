namespace Sdk.Db.Abstractions.Entities;

using Interfaces;
using System.ComponentModel.DataAnnotations;

/// <summary>
///     Base class for entities objects
/// </summary>
public abstract class Entity : IEntity
{
    [Key]
    public virtual Guid Id { get; protected set; }

    public DateTime CreatedAt { get; protected set; }

    public DateTime? UpdatedAt { get; protected set; }

    public int Version { get; protected set; }

    public void SetId(Guid id)
    {
        Id = id;
    }

    public void SetVersion(int version)
    {
        Version = version;
    }

    public override bool Equals(object? obj)
    {
        var compareTo = obj as Entity;

        if (ReferenceEquals(this, compareTo)) return true;
        if (ReferenceEquals(null, compareTo)) return false;

        return Id.Equals(compareTo.Id);
    }

    public override int GetHashCode()
    {
        return GetType().GetHashCode() * 907 + Id.GetHashCode();
    }

    public override string ToString()
    {
        return GetType().Name + " [Id=" + Id + "]";
    }
}