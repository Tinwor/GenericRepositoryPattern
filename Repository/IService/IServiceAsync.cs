using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Repository.Infrastructure;

namespace Repository.IService
{
    public interface IServiceAsync<TEntity, TDto, in TKey> : IService<TEntity, TDto, TKey>
               where TEntity : class, IEntity<TKey>, new()
               where TDto : new()
    {
        Task InsertAsync(TDto t);

        Task InsertAllAsync(IEnumerable<TDto> tList);

        Task<TDto> AddAsync(TDto t);

        Task<IEnumerable<TDto>> AddAllAsync(IEnumerable<TDto> tList);

        Task<int> DeleteAsync(TDto t);

        Task<int> DeleteAsync(TKey id);

        Task<TDto> UpdateAsync(TDto updated, TKey key);

        Task UpdateVoidAsync(TDto updated, TKey key);

        Task<TDto> AddOrUpdateAsync(TDto t);

        Task<IEnumerable<TDto>> GetAllAsync();

        Task<TDto> FindByIDAsync(TKey key);

        Task<TDto> FindAsync(Expression<Func<TEntity, bool>> match);

        Task<IEnumerable<TDto>> FindAllAsync(Expression<Func<TEntity, bool>> match);

    }
}