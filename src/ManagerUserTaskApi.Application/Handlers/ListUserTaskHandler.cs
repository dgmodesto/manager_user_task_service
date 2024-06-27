namespace ManagerUserTaskApi.Application.Handlers;

using Domain.Models;
using Domain.Requests;
using Infrastructure.Database.Interfaces;
using Mappers.Interfaces;
using Sdk.Mediator.Abstractions.Enums;
using Sdk.Mediator.Abstractions.Handlers;
using Sdk.Mediator.Abstractions.Interfaces;
using Sdk.Mediator.Abstractions.Responses;

public class ListUserTaskHandler : QueryHandler<ListTasks, Paged<UserTaskModel>>
{
    private readonly IUserTaskMapper _mapper;
    private readonly IUserTaskRepository _repository;

    public ListUserTaskHandler(IMediatorHandler mediator, IUserTaskMapper mapper, IUserTaskRepository repository) : base(mediator)
    {
        _mapper = mapper;
        _repository = repository;
    }

    protected override async Task<Paged<UserTaskModel>?> Handle(ListTasks request)
    {
        // add repository, filters, pagination, etc
        var entities = await _repository.ListAllAsync();

        var list = new List<UserTaskModel>();

        if (entities is { })
            list.AddRange(entities.Select(entity => _mapper.Map(entity)));

        var paged = new Paged<UserTaskModel>
        {
            CurrentPage = 1,
            FilterBy = "prop",
            OrderBy = "prop",
            PageSize = 100000,
            Records = list,
            RecordsInPage = 0,
            Sorting = Sorting.Asc,
            TotalPages = 1,
            TotalRecords = entities is { } ? entities!.Count : 0
        };

        return paged;
    }
}