using System.Linq.Expressions;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using TravelAgency.Data.Repository.Interfaces;

namespace TravelAgency.Data.Repository
{
    public abstract class BaseRepository<TEntity, TKey> : IBaseRepository<TEntity, TKey>
        where TEntity : class
    {
        protected readonly TravelAgencyDbContext _context;
        protected readonly DbSet<TEntity> _dbSet;

        protected BaseRepository(TravelAgencyDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<TEntity>(); 
        }
        public void Add(TEntity item)
        {
            _dbSet.Add(item);
            _context.SaveChanges();
        }

        public async Task AddAsync(TEntity item)
        {
            await _dbSet.AddAsync(item);
            await _context.SaveChangesAsync();
        }

        public void AddRange(IEnumerable<TEntity> items)
        {
            _dbSet.AddRange(items);
            _context.SaveChanges();
        }

        public async Task AddRangeAsync(IEnumerable<TEntity> items)
        {
            await _dbSet.AddRangeAsync(items);
            await _context.SaveChangesAsync();
        }

        public bool Delete(TEntity entity)
        {
            PerformSoftDeleteOfEntity(entity);

            return this.Update(entity);
        }

        public Task<bool> DeleteAsync(TEntity entity)
        {
            PerformSoftDeleteOfEntity(entity);

            return this.UpdateAsync(entity);
        }

        public TEntity? FirstOrDefault(Func<TEntity, bool> predicate)
        {
            return _dbSet.FirstOrDefault(predicate);
        }

        public Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return _dbSet.FirstOrDefaultAsync(predicate);
        }

        public IEnumerable<TEntity> GetAll()
        {
            return _dbSet.ToArray();
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            TEntity[] entities = await _dbSet.ToArrayAsync();

            return entities;
        }

        public IQueryable<TEntity> GetAllAttached()
        {
            return _dbSet.AsQueryable();
        }

        public TEntity? GetById(TKey id)
        {
            return _dbSet.Find(id);
        }

        public ValueTask<TEntity?> GetByIdAsync(TKey id)
        {
            return _dbSet.FindAsync(id);
        }

        public bool HardDelete(TEntity entity)
        {
            _dbSet.Remove(entity);
            int updateCount = _context.SaveChanges();

            return updateCount > 0;

        }

        public async Task<bool> HardDeleteAsync(TEntity entity)
        {
            _dbSet.Remove(entity);
            int updateCount = await _context.SaveChangesAsync();

            return updateCount > 0;
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public TEntity? SingleOrDefault(Func<TEntity, bool> predicate)
        {
            return _dbSet.SingleOrDefault(predicate);
        }

        public Task<TEntity?> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return _dbSet.SingleOrDefaultAsync(predicate);
        }

        public bool Update(TEntity item)
        {
            try
            {
                _dbSet.Attach(item);
                _dbSet.Entry(item).State = EntityState.Modified;
                _context.SaveChanges();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> UpdateAsync(TEntity item)
        {
            try
            {
                _dbSet.Attach(item);
                _dbSet.Entry(item).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private void PerformSoftDeleteOfEntity(TEntity entity)
        {
            PropertyInfo? isDeletedProperty =
                this.GetIsDeletedProperty(entity);
            if (isDeletedProperty == null)
            {
                throw new InvalidOperationException("Can't performe soft delete");
            }

            isDeletedProperty.SetValue(entity, true);
        }

        private PropertyInfo? GetIsDeletedProperty(TEntity entity)
        {
            return typeof(TEntity)
                .GetProperties()
                .FirstOrDefault(pi => pi.PropertyType == typeof(bool) &&
                                                 pi.Name == "IsDeleted");
        }
    }
}
