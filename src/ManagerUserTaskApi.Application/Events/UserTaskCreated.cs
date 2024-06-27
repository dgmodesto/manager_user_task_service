namespace ManagerUserTaskApi.Application.Events;

using Infrastructure.Database.Entities;
using Sdk.Db.Audit;
using Sdk.Mediator.Abstractions.Records;

public class UserTaskCreated : AuditEvent<UserTask>
{
    public UserTaskCreated(UserTask entity, UserData user) : base(entity, user)
    {
    }
}