using System.Linq.Expressions;

namespace TravelAgency.Data.Repository.Interfaces
{
    public interface IBaseRepository<TEntity, TKey>
    {
        TEntity? GetById(TKey id);

        ValueTask<TEntity?> GetByIdAsync(TKey id);

        TEntity? SingleOrDefault(Func<TEntity, bool> predicate);

        Task<TEntity?> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> predicate);

        TEntity? FirstOrDefault(Func<TEntity, bool> predicate);

        Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate);

        IEnumerable<TEntity> GetAll();

        Task<IEnumerable<TEntity>> GetAllAsync();

        IQueryable<TEntity> GetAllAttached();

        void Add(TEntity item);

        Task AddAsync(TEntity item);

        void AddRange(IEnumerable<TEntity> items);

        Task AddRangeAsync(IEnumerable<TEntity> items);

        bool Delete(TEntity entity);

        Task<bool> DeleteAsync(TEntity entity);

        bool HardDelete(TEntity entity);

        Task<bool> HardDeleteAsync(TEntity entity);

        bool Update(TEntity item);

        Task<bool> UpdateAsync(TEntity item);

        void SaveChanges();

        Task SaveChangesAsync();
    }
}
