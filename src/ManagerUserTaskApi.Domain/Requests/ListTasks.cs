namespace ManagerUserTaskApi.Domain.Requests;

using Models;
using Sdk.Mediator.Abstractions.Requests;

public class ListTasks : PagedRequestBase<UserTaskModel>
{
}