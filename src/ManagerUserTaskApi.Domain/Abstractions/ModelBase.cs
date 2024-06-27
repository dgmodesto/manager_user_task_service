namespace ManagerUserTaskApi.Domain.Abstractions;

public abstract class ModelBase
{
    public string? Id { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public int Version { get; set; }
}