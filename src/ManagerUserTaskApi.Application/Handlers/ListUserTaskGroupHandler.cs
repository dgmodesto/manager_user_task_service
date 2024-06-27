namespace ManagerUserTaskApi.Application.Handlers;

using Domain.Models;
using Domain.Requests;
using Infrastructure.Database.Interfaces;
using ManagerUserTaskApi.Domain.Extensions;
using ManagerUserTaskApi.Infrastructure.Database.Entities;
using Sdk.Db.Abstractions.Pagination;
using Sdk.Mediator.Abstractions.Enums;
using Sdk.Mediator.Abstractions.Handlers;
using Sdk.Mediator.Abstractions.Interfaces;
using Sdk.Mediator.Abstractions.Responses;
using System.Linq;
using System.Linq.Expressions;

public class ListUserTaskGroupHandler : QueryHandler<ListUserTaskGroup, Paged<UserTaskGroupModel>>
{
    private readonly IUserTaskRepository _repository;

    public ListUserTaskGroupHandler(
        IMediatorHandler mediator,
        IUserTaskRepository repository) : base(mediator)
    {
        _repository = repository;
    }

    protected override async Task<Paged<UserTaskGroupModel>?> Handle(ListUserTaskGroup request)
    {
        // add repository, filters, pagination, etc
        var filter = DefineFilters(request);

        var paginatedList = await _repository.ListUpcomingTasksGroupedByDatePagedAsync(
            new Order(request.OrderBy.Capitalize() ?? "Date", Sorting.Asc.Equals(request.Sorting)),
            new Page(request.Page, request.PageSize),
            filter,
            taskUser => taskUser.Date);

        var list = new List<UserTaskGroupModel>();

        if (paginatedList.Results is not null)
        {
            foreach (var entity in paginatedList.Results)
            {
                list.Add(MapUserTaskGroupModel(entity));
            }
        }

        var paged = new Paged<UserTaskGroupModel>
        {
            FilterBy = String.Join(",", filter.Parameters.Select(p => p.Name).ToList()),
            OrderBy = request.OrderBy,
            PageSize = request.PageSize,
            Sorting = request.Sorting,
            CurrentPage = paginatedList.CurrentPage,
            RecordsInPage = paginatedList.RecordsInPage,
            TotalPages = paginatedList.TotalPages,
            TotalRecords = paginatedList.TotalRecords,
            Records = list
        };

        return paged;
    }

    private UserTaskGroupModel MapUserTaskGroupModel(UserTaskGroup entity)
    {
        return new UserTaskGroupModel
        {
            Date = entity.Date,
            UserTasks = entity.UserTasks.Select(e => new UserTaskModel
            {
                Id = e.Id.ToString(),
                User = e.User,
                Date = e.Date,
                Description = e.Description,
                StartTime = e.StartTime,
                EndTime = e.EndTime,
                Subject = e.Subject,
                CreatedAt = e.CreatedAt,
                UpdatedAt = e.UpdatedAt,
                Version = e.Version,
            }).ToList(),
        };
    }

    private Expression<Func<UserTask, bool>> DefineFilters(ListUserTaskGroup request)
    {
        // Define the filter based on the request
        var parameter = Expression.Parameter(typeof(UserTask), "model");
        Expression<Func<UserTask, bool>> filter = userTask => userTask.Date > DateTime.Today.ToUniversalTime();


        return filter;
    }
}