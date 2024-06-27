namespace ManagerUserTaskApi.Application.Events;

using Infrastructure.Database.Entities;
using Sdk.Db.Audit;
using Sdk.Mediator.Abstractions.Records;

public class UserTaskDeleted : AuditEvent<UserTask>
{
    public UserTaskDeleted(UserTask entity, UserData user) : base(entity, user)
    {
    }
}