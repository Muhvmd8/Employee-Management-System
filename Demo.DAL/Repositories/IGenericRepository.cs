using System.Linq.Expressions;

namespace Demo.DAL.Repositories;
public interface IGenericRepository<TEntity> where TEntity : BaseEntity
{
    void Update(TEntity entity);
    void Add(TEntity entity);
    void Delete(TEntity entity);
    Task<IEnumerable<TEntity>> GetAllAsync(bool withTracking = false);
    Task<IEnumerable<TResult>> GetAllAsync<TResult>(Expression<Func<TEntity, TResult>> selector,
        Expression<Func<TEntity, bool>> predicate,
     params Expression<Func<TEntity, object>>[] includes);

    Task<TEntity?> GetByIdAsync(int id);
}
