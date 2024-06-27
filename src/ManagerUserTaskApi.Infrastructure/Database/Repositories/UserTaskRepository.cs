namespace ManagerUserTaskApi.Infrastructure.Database.Repositories;

using Entities;
using Interfaces;
using Microsoft.EntityFrameworkCore;
using Sdk.Db.Abstractions.Extensions;
using Sdk.Db.Abstractions.Pagination;
using Sdk.Db.Audit;
using Sdk.Mediator.Abstractions.Interfaces;
using System.Data;
using System.Linq.Expressions;

public class UserTaskRepository : BaseRepositoryWithAudit<UserTask>, IUserTaskRepository
{
    public UserTaskRepository(ManagerUserTaskApiDbContext context, IMediatorHandler mediator) : base(context, mediator)
    {

    }


    public async Task<PaginatedList<UserTaskGroup>> ListUpcomingTasksGroupedByDatePagedAsync(Order order, Page page, Expression<Func<UserTask, bool>> filter, params Expression<Func<UserTask, object>>[] includeProperties)
    {
        var result = DbSet().Any()
                    ? await DbSet()
                       .Where(filter)
                       .Order(order)
                       !.Skip((page.Index - 1) * page.Quantity)
                       .Take(page.Quantity)
                       .OrderBy(t => t.Date)
                       .ThenBy(t => t.StartTime)
                       .GroupBy(t => t.Date)
                       .Select(g => new UserTaskGroup { Date = g.Key, UserTasks = g.ToList() })
                       .ToListAsync()
                    : null;

        var totalRecords = result?.Count();

        var pagination = new PaginationObject
        {
            Order = order,
            Page = page,
            TotalRecords = (int)totalRecords
        };

        return new PaginatedList<UserTaskGroup>(result?.AsQueryable(), pagination);
    }
}