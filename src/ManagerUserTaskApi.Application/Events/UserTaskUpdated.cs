namespace ManagerUserTaskApi.Application.Events;

using Infrastructure.Database.Entities;
using Sdk.Db.Audit;
using Sdk.Mediator.Abstractions.Records;

public class UserTaskUpdated : AuditEvent<UserTask>
{
    public UserTaskUpdated(UserTask entity, UserData user) : base(entity, user)
    {
    }
}