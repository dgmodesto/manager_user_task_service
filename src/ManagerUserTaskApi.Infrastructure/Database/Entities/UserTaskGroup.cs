using Sdk.Db.Abstractions.Entities;

namespace ManagerUserTaskApi.Infrastructure.Database.Entities;

public class UserTaskGroup : Entity
{
    public DateTime Date { get; set; }
    public List<UserTask> UserTasks { get; set; } = new List<UserTask>();
}
