namespace ManagerUserTaskApi.Application.Handlers;

using Domain.Requests;
using Events;
using Infrastructure.Database.Interfaces;
using Mappers.Interfaces;
using Sdk.Mediator.Abstractions.Enums;
using Sdk.Mediator.Abstractions.Handlers;
using Sdk.Mediator.Abstractions.Interfaces;

public class UpdateUserTaskHandler : OneWayCommandHandler<UpdateUserTask>
{
    private readonly IUserTaskMapper _mapper;
    private readonly IUserTaskRepository _repository;

    public UpdateUserTaskHandler(IMediatorHandler mediator, IUserTaskMapper mapper, IUserTaskRepository repository) : base(mediator)
    {
        _mapper = mapper;
        _repository = repository;
    }

    protected override async Task Handle(UpdateUserTask request)
    {
        var entity = await _repository.GetByIdAsync(request.Id);
        if (entity is null)
        {
            await NotifyError(FailureReason.NotFound, "Task not found");
            return;
        }

        // optionally add checks if already exists and handle accordingly, for example:
        // var exists = await _repository.ExistsByExpressionAsync(p => p.Description == request.Description
        //                                                             && p.Id != request.Id);
        //
        // if (exists)
        // {
        //     await NotifyError(FailureReason.Conflict, "Record already exists");
        //     return;
        // }

        var updatedEntity = _mapper.Map(request);
        await _repository.UpdateAsync<UserTaskUpdated>(updatedEntity);
    }
}