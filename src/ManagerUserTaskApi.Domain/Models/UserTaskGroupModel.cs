using ManagerUserTaskApi.Domain.Abstractions;

namespace ManagerUserTaskApi.Domain.Models;

public class UserTaskGroupModel : ModelBase
{
    public DateTime Date { get; set; }
    public List<UserTaskModel> UserTasks { get; set; } = new List<UserTaskModel>();
}
