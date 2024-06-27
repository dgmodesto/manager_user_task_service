namespace ManagerUserTaskApi.Infrastructure.Database.Entities;

using Sdk.Db.Abstractions.Entities;

public class UserTask : Entity
{
    public string User { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public string Subject { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

}