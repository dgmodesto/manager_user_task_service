namespace Sdk.Db.EventStore.Repositories;

using Abstractions.Extensions;
using Abstractions.Pagination;
using DbContexts;
using Entities;
using Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

public class EventStoreRepository<T> : IEventStoreRepository
    where T : DbContext
{
    private readonly BaseEventStoreDbContext<T> _context;

    public EventStoreRepository(BaseEventStoreDbContext<T> context)
    {
        _context = context;
    }

    public async Task InsertAsync(EventRecord entity)
    {
        await DbSet().AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task<PaginatedList<EventRecord>> ListAllPaginatedAsync(Order order, Page page,
        Expression<Func<EventRecord, bool>> filter)
    {
        var result = DbSet().Any()
            ? await Task.Run(() =>
                DbSet().Where(filter).Order(order)!.Skip((page.Index - 1) * page.Quantity).AsQueryable().AsNoTracking())
            : null;

        var pagination = new PaginationObject
        {
            Order = order,
            Page = page
        };

        return new PaginatedList<EventRecord>(result?.AsQueryable(), pagination);
    }

    public async Task<List<EventRecord>> ListByExpressionAsync(Expression<Func<EventRecord, bool>> expression)
    {
        return await DbSet().Where(expression).AsNoTracking().ToListAsync();
    }

    public DbSet<EventRecord> DbSet()
    {
        return _context.Set<EventRecord>();
    }
}