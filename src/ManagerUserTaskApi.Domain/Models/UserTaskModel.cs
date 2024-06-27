namespace ManagerUserTaskApi.Domain.Models;

using Abstractions;

public class UserTaskModel : ModelBase
{
    public string User { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public string Subject { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}