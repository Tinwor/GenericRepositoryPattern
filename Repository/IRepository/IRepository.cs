using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using Repository.Infrastructure;

namespace Repository.IRepository
{
    public interface IRepository<TEntity, in TKey>
      where TEntity : class, IEntity<TKey>, IObjectState
    {
        void Delete(TEntity entity);

        void Delete(TKey id);

        int Count();

        TEntity Find(Expression<Func<TEntity, bool>> predicate);

        IQueryable<TEntity> FindAllAsQueryable(Expression<Func<TEntity, bool>> predicate);

        IEnumerable<TEntity> GetAll();

        IQueryable<TEntity> GetAllAsQueryable();

        TEntity FindByID(TKey key);

        IEnumerable<TEntity> FindAll(Expression<Func<TEntity, bool>> match);

        void Add(TEntity t);

        void AddAll(IEnumerable<TEntity> tList);

        void Update(TEntity updated, TKey key);

        DbSet GetDbContext();

    }
}