using System.Linq.Expressions;

namespace Demo.DAL.Repositories;
public class GenericRepository<TEntity>(ApplicationDbContext context)
    : IGenericRepository<TEntity>
    where TEntity : BaseEntity
{
    protected readonly ApplicationDbContext _context = context;


    public async Task<TEntity?> GetByIdAsync(int id) => await _context.Set<TEntity>().FindAsync(id);

    // Get All
    public async Task<IEnumerable<TEntity>> GetAllAsync(bool withTracking = false)
        => withTracking ? await _context.Set<TEntity>().Where(d => !d.IsDeleted).ToListAsync() :
        await _context.Set<TEntity>().AsNoTracking().Where(d => !d.IsDeleted).ToListAsync();
    // Add
    public void Add(TEntity entity) => _context.Set<TEntity>().Add(entity);
    // Update
    public void Update(TEntity entity) => _context.Set<TEntity>().Update(entity);
    // Delete
    public void Delete(TEntity entity) => _context.Set<TEntity>().Remove(entity);

    public async Task<IEnumerable<TResult>> GetAllAsync<TResult>(Expression<Func<TEntity, TResult>> selector,
               Expression<Func<TEntity, bool>> predicate,
        params Expression<Func<TEntity, object>>[] includes)
    {
        // DbSet.Where().Include()


        IQueryable<TEntity> query = _context.Set<TEntity>();

        foreach (var include in includes)
            query.Include(include);
        //_context.ChangeTracker
        return await query
           .AsNoTracking()
           .Where(predicate)
           .Select(selector)
           .ToListAsync();
    }

    //public IQueryable<TEntity> GetAllQueryable() =>
    //     _context.Set<TEntity>().AsNoTracking().Where(d => !d.IsDeleted);
}


