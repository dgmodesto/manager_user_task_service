namespace Sdk.Db.Abstractions.Interfaces;

using Entities;
using Pagination;
using System.Linq.Expressions;

/// <summary>
///     This interface contains the methods of data repositories.
/// </summary>
/// <typeparam name="TEntity"></typeparam>
public interface IBaseRepository<TEntity>
    where TEntity : Entity
{
    Task<List<TEntity>?> ListAllAsync();

    Task<IList<TEntity>?> ListAllAsync(params Expression<Func<TEntity, object>>[] includeProperties);

    Task<PaginatedList<TEntity>> ListAllPagedAsync(Order order, Page page, Expression<Func<TEntity, bool>> filter);

    Task<List<TEntity>> ListByCommandAsync(string sqlRaw);

    Task<IList<TEntity>?> ListByExpressionAsync(Expression<Func<TEntity, bool>> expression,
        params Expression<Func<TEntity, object>>[] includeProperties);

    Task<List<TEntity>> ListByExpressionAsync(Expression<Func<TEntity, bool>> expression);

    Task<TEntity?> GetByIdAsync(Guid id, params Expression<Func<TEntity, object>>[] includeProperties);

    Task<TEntity?> GetByIdWithTrackingAsync(Guid id, params Expression<Func<TEntity, object>>[] includeProperties);

    Task<TEntity?> GetSingleByExpressionAsync(Expression<Func<TEntity, bool>> expression,
        params Expression<Func<TEntity, object>>[] includeProperties);

    Task<TEntity?> GetFirstByExpressionAsync(Expression<Func<TEntity, bool>> expression, bool withTracking = false);

    Task<TEntity?> GetFirstByExpressionAsync(Expression<Func<TEntity, bool>> expression,
        params Expression<Func<TEntity, object>>[] includeProperties);

    Task<bool> ExistsByExpressionAsync(Expression<Func<TEntity, bool>> expression);

    Task<long> CountAllAsync();

    Task<long> CountByExpressionAsync(Expression<Func<TEntity, bool>> expression);

    Task DeleteAsync(TEntity entity);

    Task<List<TEntity>> DeleteByExpressionAsync(Expression<Func<TEntity, bool>> expression);

    Task InsertAsync(TEntity entity);

    Task UpdateAsync(TEntity entity);

    Task InsertRangeAsync(IList<TEntity> entities);

    Task<int> SaveChangesAsync();
}