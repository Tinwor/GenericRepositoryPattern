using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Repository.Infrastructure;

namespace Repository.IService
{
    public interface IService<TEntity, TDto, in TKey>
               where TEntity : class, IEntity<TKey>, new()
               where TDto : new()
    {
        TDto Add(TDto t);

        IEnumerable<TDto> AddAll(IEnumerable<TDto> tList);

        void Insert(TDto entity);

        void InsertAll(IEnumerable<TDto> entity);

        TDto AddOrUpdate(TDto t);

        TDto Update(TDto updated, TKey key);

        void UpdateVoid(TDto updated, TKey key);

        int Delete(TDto entity);

        int Count();

        TDto Find(Expression<Func<TEntity, bool>> predicate);

        IEnumerable<TDto> FindAll(Expression<Func<TEntity, bool>> predicate);

        IQueryable<TEntity> FindAllAsQueryable(Expression<Func<TEntity, bool>> predicate);

        IQueryable<TEntity> GetAllAsQueryable();

        IEnumerable<TDto> GetAll();

        TDto FindByID(TKey key);

    }
}