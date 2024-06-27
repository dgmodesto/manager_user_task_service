namespace ManagerUserTaskApi.Application.Handlers;

using Domain.Requests;
using Events;
using Infrastructure.Database.Interfaces;
using Sdk.Mediator.Abstractions.Enums;
using Sdk.Mediator.Abstractions.Handlers;
using Sdk.Mediator.Abstractions.Interfaces;

public class DeleteUserTaskHandler : OneWayCommandHandler<DeleteUserTask>
{
    private readonly IUserTaskRepository _repository;

    public DeleteUserTaskHandler(IMediatorHandler mediator, IUserTaskRepository repository) : base(mediator)
    {
        _repository = repository;
    }

    protected override async Task Handle(DeleteUserTask request)
    {
        var entity = await _repository.GetByIdAsync(request.Id);

        if (entity is null)
        {
            await NotifyError(FailureReason.NotFound, "Task not found");
            return;
        }

        await _repository.DeleteAsync<UserTaskDeleted>(entity);
    }
}