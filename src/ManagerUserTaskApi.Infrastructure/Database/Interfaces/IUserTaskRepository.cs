namespace ManagerUserTaskApi.Infrastructure.Database.Interfaces;

using Entities;
using Sdk.Db.Abstractions.Pagination;
using Sdk.Db.Audit.Interfaces;
using System.Linq.Expressions;

public interface IUserTaskRepository : IBaseRepositoryWithAudit<UserTask>
{
    Task<PaginatedList<UserTaskGroup>> ListUpcomingTasksGroupedByDatePagedAsync(
        Order order,
        Page page,
        Expression<Func<UserTask, bool>> filter,
        params Expression<Func<UserTask, object>>[] includeProperties);
}