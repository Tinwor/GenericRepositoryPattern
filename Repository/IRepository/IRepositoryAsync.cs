using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Repository.Infrastructure;

namespace Repository.IRepository
{
    public interface IRepositoryAsync<TEntity, in TKey> : IRepository<TEntity, TKey>
     where TEntity : class, IEntity<TKey>, IObjectState
    {
        DbContext DbContext { get; }

        Task<int> CountAsync();

        Task<IEnumerable<TEntity>> GetAllAsync();

        Task<TEntity> FindByIDAsync(TKey key);

        Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> predicate);

        Task<IEnumerable<TEntity>> FindAllAsync(Expression<Func<TEntity, bool>> predicate);

        Task UpdateAsync(TEntity updated, TKey key);


        Task<TEntity> AddOrUpdateAsync(TEntity updated);

        Task DeleteAsync(TEntity entity);

        Task DeleteAsync(TKey id);
    }
}