using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Repository.Infrastructure;
using Repository.IRepository;
using Repository.IUnitOfWork;

namespace Repository.EF.Repository.Base
{
    public abstract class Repository<TEntity, TKey> : IRepositoryAsync<TEntity, TKey>
               where TEntity : class, IEntity<TKey>, IObjectState, new()
    {
        #region PRIVATE

        private readonly IDataContextAsync _dbContext;
        private readonly DbSet<TEntity> _dbSet;
        private readonly DbContext dbContext;

        #endregion PRIVATE

        #region PUBLIC
        public DbContext DbContext { get { return dbContext; } }
        #endregion PUBLIC

        #region CONSTRUCTOR

        public Repository(IDataContextAsync context, IUnitOfWorkAsync unitOfWork)
        {

            _dbContext = context;

            dbContext = context as DbContext;

            if (dbContext != null)
            {
                _dbSet = dbContext.Set<TEntity>();
            }

        }

        #endregion CONSTRUCTOR

        /// <summary>
        ///     Counts this instance.
        /// </summary>
        /// <returns></returns>
        public virtual int Count()
        {
            return _dbSet.AsNoTracking().Count();
        }

        /// <summary>
        ///     Counts the asynchronous.
        /// </summary>
        /// <returns></returns>
        public virtual async Task<int> CountAsync()
        {
            return await _dbSet.AsNoTracking().CountAsync();
        }

        /// <summary>
        ///     Finds the specified predicate.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns></returns>
        public virtual TEntity Find(Expression<Func<TEntity, bool>> predicate)
        {
            return _dbSet.Where(predicate).AsNoTracking().FirstOrDefault();
        }

        /// <summary>
        /// Finds all as queryable.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns></returns>
        public virtual IQueryable<TEntity> FindAllAsQueryable(Expression<Func<TEntity, bool>> predicate)
        {
            return _dbSet.Where(predicate).AsNoTracking().AsQueryable();
        }

        /// <summary>
        ///     Gets all.
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerable<TEntity> GetAll()
        {
            return _dbSet.AsNoTracking().ToList();
        }

        /// <summary>
        ///     Gets all as queryable.
        /// </summary>
        /// <returns></returns>
        public virtual IQueryable<TEntity> GetAllAsQueryable()
        {
            return _dbSet.AsNoTracking().AsQueryable();
        }

        /// <summary>
        ///     Finds the by identifier.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public virtual TEntity FindByID(TKey key)
        {
            return _dbSet.AsNoTracking().FirstOrDefault(x => x.Id.ToString() == key.ToString());
        }

        /// <summary>
        ///     Gets all asynchronous.
        /// </summary>
        /// <returns></returns>
        public virtual async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            List<TEntity> query = await _dbSet.AsNoTracking().ToListAsync();

            return query;
        }

        /// <summary>
        ///     Finds the by identifier asynchronous.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public virtual async Task<TEntity> FindByIDAsync(TKey key)
        {
            return await _dbSet.AsNoTracking().FirstOrDefaultAsync(x => x.Id.ToString() == key.ToString());
        }

        /// <summary>
        /// Finds all.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns></returns>
        public virtual IEnumerable<TEntity> FindAll(Expression<Func<TEntity, bool>> predicate)
        {
            return _dbSet.Where(predicate).AsNoTracking().ToList();
        }

        /// <summary>
        /// Finds the asynchronous.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns></returns>
        public virtual async Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> predicate)
        {
            try
            {
                var query = await _dbSet.AsNoTracking().FirstOrDefaultAsync(predicate);
                return query;

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Finds all asynchronous.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns></returns>
        public virtual async Task<IEnumerable<TEntity>> FindAllAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _dbSet.Where(predicate).AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// Updates the asynchronous.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public virtual async Task UpdateAsync(TEntity entity, TKey key)
        {
            entity.ObjectState = ObjectState.Modified;
            _dbSet.Attach(entity);
            _dbContext.SyncObjectState(entity);

        }

        /// <summary>
        ///     Adds the or update asynchronous.
        /// </summary>
        /// <param name="updated">The updated.</param>
        /// <returns></returns>
        public virtual async Task<TEntity> AddOrUpdateAsync(TEntity updated)
        {
            var existing = await _dbSet.AsNoTracking().FirstOrDefaultAsync(x => x.Id.ToString() == updated.Id.ToString());
            if (existing == null)
            {
                Add(updated);
                existing = updated;
            }
            else
            {
                Update(updated, updated.Id);
            }

            return existing;
        }

        /// <summary>
        ///     Adds the specified t.
        /// </summary>
        /// <param name="t">The t.</param>
        /// <returns></returns>
        public virtual void Add(TEntity t)
        {
            t.ObjectState = ObjectState.Added;

            _dbSet.Attach(t);
            _dbContext.SyncObjectState(t);

        }

        /// <summary>
        ///     Adds all.
        /// </summary>
        /// <param name="tList">The t list.</param>
        public virtual void AddAll(IEnumerable<TEntity> tList)
        {
            var list = tList as IList<TEntity> ?? tList.ToList();
            if (!list.Any()) return;

            foreach (var entity in list)
            {
                Add(entity);
            }
        }

        /// <summary>
        /// Updates the specified updated.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="key">The key.</param>
        public virtual void Update(TEntity entity, TKey key)
        {
            entity.ObjectState = ObjectState.Modified;
            _dbSet.Attach(entity);
            _dbContext.SyncObjectState(entity);
        }

        /// <summary>
        ///     Deletes the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public virtual void Delete(TEntity entity)
        {
            entity.ObjectState = ObjectState.Deleted;
            _dbSet.Attach(entity);
            _dbContext.SyncObjectState(entity);
        }

        /// <summary>
        ///     Deletes the specified entity.
        /// </summary>
        /// <param name="id">The identifier.</param>
        public virtual void Delete(TKey id)
        {
            var existing = _dbSet.AsNoTracking().FirstOrDefault(x => x.Id.ToString() == id.ToString());
            if (existing == null) return;

            existing.ObjectState = ObjectState.Deleted;
            _dbSet.Attach(existing);
            _dbContext.SyncObjectState(existing);
        }

        /// <summary>
        ///     Deletes the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        public virtual async Task DeleteAsync(TKey id)
        {
            var existing = await _dbSet.AsNoTracking().FirstOrDefaultAsync(x => x.Id.ToString() == id.ToString());
            if (existing == null) return;

            existing.ObjectState = ObjectState.Deleted;
            _dbSet.Attach(existing);
            _dbContext.SyncObjectState(existing);

        }

        /// <summary>
        ///     Deletes the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public virtual async Task DeleteAsync(TEntity entity)
        {
            entity.ObjectState = ObjectState.Deleted;
            _dbSet.Attach(entity);
            _dbContext.SyncObjectState(entity);
        }

        public virtual DbSet GetDbContext()
        {
            return _dbSet;
        }
    }
}