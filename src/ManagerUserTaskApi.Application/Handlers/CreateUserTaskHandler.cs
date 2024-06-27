namespace ManagerUserTaskApi.Application.Handlers;

using Domain.Models;
using Domain.Requests;
using Events;
using Infrastructure.Database.Interfaces;
using Mappers.Interfaces;
using Sdk.Mediator.Abstractions.Handlers;
using Sdk.Mediator.Abstractions.Interfaces;

public class CreateUserTaskHandler : TwoWayCommandHandler<CreateUserTask, UserTaskModel>
{
    private readonly IUserTaskMapper _mapper;
    private readonly IUserTaskRepository _repository;

    public CreateUserTaskHandler(IMediatorHandler mediator, IUserTaskMapper mapper, IUserTaskRepository repository) : base(mediator)
    {
        _mapper = mapper;
        _repository = repository;
    }

    protected override async Task<UserTaskModel?> Handle(CreateUserTask request)
    {
        // optionally add checks if already exists and handle accordingly, for example:
        // var exists = await _repository.ExistsByExpressionAsync(p => p.Description == request.Description);
        // if (exists)
        // {
        //     await NotifyError(FailureReason.Conflict, "Record already exists");
        //     return null;
        // }

        var entity = _mapper.Map(request);
        await _repository.InsertAsync<UserTaskCreated>(entity);
        return _mapper.Map(entity);
    }
}