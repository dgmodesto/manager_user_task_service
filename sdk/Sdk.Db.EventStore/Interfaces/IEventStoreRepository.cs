namespace Sdk.Db.EventStore.Interfaces;

using Abstractions.Pagination;
using Entities;
using System.Linq.Expressions;

public interface IEventStoreRepository
{
    Task<PaginatedList<EventRecord>> ListAllPaginatedAsync(Order order, Page page,
        Expression<Func<EventRecord, bool>> filter);

    Task<List<EventRecord>> ListByExpressionAsync(Expression<Func<EventRecord, bool>> expression);

    Task InsertAsync(EventRecord entity);
}