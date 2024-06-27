namespace ManagerUserTaskApi.Application.Handlers;

using Domain.Models;
using Domain.Requests;
using Infrastructure.Database.Interfaces;
using ManagerUserTaskApi.Infrastructure.Cache;
using ManagerUserTaskApi.Infrastructure.Database.Entities;
using Mappers.Interfaces;
using Sdk.Mediator.Abstractions.Enums;
using Sdk.Mediator.Abstractions.Handlers;
using Sdk.Mediator.Abstractions.Interfaces;
using System.Text.Json;

public class GetUserTaskHandler : QueryHandler<GetUserTask, UserTaskModel>
{
    private readonly IUserTaskMapper _mapper;
    private readonly IUserTaskRepository _repository;
    private readonly ICacheService<UserTask> _cache;
    public GetUserTaskHandler(
        IMediatorHandler mediator,
        IUserTaskMapper mapper,
        IUserTaskRepository repository,
        ICacheService<UserTask> cache) : base(mediator)
    {
        _mapper = mapper;
        _repository = repository;
        _cache = cache;
    }

    protected override async Task<UserTaskModel?> Handle(GetUserTask request)
    {
        var entityCache = await _cache.GetStringAsync<UserTask>(request.Id.ToString());
        if (entityCache is null)
        {
            var entity = await _repository.GetByIdAsync(request.Id);
            if (entity is not null)
            {
                var expiry = TimeSpan.FromMinutes(2);
                _ = _cache.SetStringAsync(
                    request.Id.ToString(),
                    JsonSerializer.Serialize(entity),
                    expiry);
            }
            entityCache = entity;
        }

        if (entityCache is not null)
            return _mapper.Map(entityCache);

        await NotifyError(FailureReason.NotFound, "Task not found");
        return null;
    }
}