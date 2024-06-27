namespace Sdk.Db.Abstractions.Repositories;

using Entities;
using Extensions;
using Interfaces;
using Microsoft.EntityFrameworkCore;
using Pagination;
using System.Linq.Expressions;

public abstract class BaseRepository<TEntity> : IBaseRepository<TEntity>
    where TEntity : Entity
{
    protected DbContext Context;

    protected BaseRepository(DbContext context)
    {
        Context = context;
    }

    public async Task<long> CountAllAsync()
    {
        return await DbSet().AsNoTracking().AnyAsync() ? await DbSet().AsNoTracking().CountAsync() : 0;
    }

    public async Task<long> CountByExpressionAsync(Expression<Func<TEntity, bool>> expression)
    {
        return await ExistsByExpressionAsync(expression) ? await DbSet().AsNoTracking().CountAsync(expression) : 0;
    }

    public async Task<List<TEntity>> DeleteByExpressionAsync(Expression<Func<TEntity, bool>> expression)
    {
        var items = await ListByExpressionAsync(expression);

        if (items.Count > 0)
        {
            items.ForEach(item => { Context.Entry(item).State = EntityState.Deleted; });
            DbSet().RemoveRange(items);
            await SaveChangesAsync();
        }

        return items;
    }

    public virtual async Task DeleteAsync(TEntity entity)
    {
        Context.Entry(entity).State = EntityState.Deleted;
        DbSet().Remove(entity);
        await SaveChangesAsync();
        Context.Entry(entity).State = EntityState.Detached;
    }

    public async Task<bool> ExistsByExpressionAsync(Expression<Func<TEntity, bool>> expression)
    {
        return await DbSet().AsNoTracking().AnyAsync(expression);
    }

    public async Task<TEntity?> GetByIdAsync(Guid id, params Expression<Func<TEntity, object>>[] includeProperties)
    {
        return await DbSet().Where(c => c.Id == id).AsNoTracking().IncludeProperties(includeProperties)
            .FirstOrDefaultAsync();
    }

    public async Task<TEntity?> GetByIdWithTrackingAsync(Guid id,
        params Expression<Func<TEntity, object>>[] includeProperties)
    {
        return await DbSet().Where(c => c.Id == id).IncludeProperties(includeProperties).FirstOrDefaultAsync();
    }

    public async Task<TEntity?> GetFirstByExpressionAsync(Expression<Func<TEntity, bool>> expression,
        bool withTracking = false)
    {
        if (withTracking)
            return await DbSet().FirstOrDefaultAsync(expression);

        return await DbSet().AsNoTracking().FirstOrDefaultAsync(expression);
    }

    public async Task<TEntity?> GetFirstByExpressionAsync(Expression<Func<TEntity, bool>> expression,
        params Expression<Func<TEntity, object>>[] includeProperties)
    {
        return await DbSet().AsNoTracking().IncludeProperties(includeProperties).FirstOrDefaultAsync(expression);
    }

    public async Task<TEntity?> GetSingleByExpressionAsync(Expression<Func<TEntity, bool>> expression,
        params Expression<Func<TEntity, object>>[] includeProperties)
    {
        return await DbSet().AsNoTracking().IncludeProperties(includeProperties).SingleOrDefaultAsync(expression);
    }

    public async Task InsertAsync(TEntity entity)
    {
        Context.Entry(entity).State = EntityState.Added;
        await SaveChangesAsync();
        Context.Entry(entity).State = EntityState.Detached;
    }

    public async Task InsertRangeAsync(IList<TEntity> entities)
    {
        await DbSet().AddRangeAsync(entities);
    }

    public async Task<List<TEntity>?> ListAllAsync()
    {
        return DbSet().Any() ? await Task.Run(() => DbSet().AsQueryable().AsNoTracking().ToList()) : null;
    }

    public async Task<IList<TEntity>?> ListAllAsync(params Expression<Func<TEntity, object>>[] includeProperties)
    {
        return DbSet().Any() ? await DbSet().IncludeProperties(includeProperties).ToListAsync() : null;
    }

    public async Task<PaginatedList<TEntity>> ListAllPagedAsync(Order order, Page page,
        Expression<Func<TEntity, bool>> filter)
    {
        var result = DbSet().Any()
            ? await Task.Run(() =>
                DbSet().Where(filter).Order(order)!.Skip((page.Index - 1) * page.Quantity).Take(page.Quantity)
                    .AsQueryable().AsNoTracking())
            : null;

        var totalRecords = await CountByExpressionAsync(filter);

        var pagination = new PaginationObject
        {
            Order = order,
            Page = page,
            TotalRecords = (int)totalRecords
        };

        return new PaginatedList<TEntity>(result?.AsQueryable(), pagination);
    }

    public async Task<List<TEntity>> ListByCommandAsync(string sqlRaw)
    {
        return await DbSet().FromSqlRaw(sqlRaw).ToListAsync();
    }

    public async Task<IList<TEntity>?> ListByExpressionAsync(Expression<Func<TEntity, bool>> expression,
        params Expression<Func<TEntity, object>>[] includeProperties)
    {
        return DbSet().Any(expression)
            ? await DbSet().Where(expression).IncludeProperties(includeProperties).ToListAsync()
            : null;
    }

    public async Task<List<TEntity>> ListByExpressionAsync(Expression<Func<TEntity, bool>> expression)
    {
        return await DbSet().Where(expression).AsNoTracking().ToListAsync();
    }

    public async Task<int> SaveChangesAsync()
    {
        return await Context.SaveChangesAsync();
    }

    public async Task UpdateAsync(TEntity entity)
    {
        Context.Entry(entity).State = EntityState.Modified;
        entity.SetVersion(entity.Version + 1);
        await Context.SaveChangesAsync();
        Context.Entry(entity!).State = EntityState.Detached;
    }

    public DbSet<TEntity> DbSet()
    {
        return Context.Set<TEntity>();
    }
}