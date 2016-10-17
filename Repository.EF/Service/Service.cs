using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Transactions;
using AutoMapper;
using Logging;
using Repository.Infrastructure;
using Repository.IRepository;
using Repository.IService;
using Repository.IUnitOfWork;

namespace Repository.EF.Service
{
    public abstract class Service<TEntity, TDto, Tkey> : IServiceAsync<TEntity, TDto, Tkey>
               where TEntity : class, IObjectState, IEntity<Tkey>, new()
               where TDto : new()
    {
        #region Private Fields

        private readonly IRepositoryAsync<TEntity, Tkey> _repository;
        private readonly ILogger _logger;
        private readonly IUnitOfWorkAsync _uow;

        #endregion Private Fields

        #region Constructor

        public Service(IRepositoryAsync<TEntity, Tkey> repository, ILogger log, IUnitOfWorkAsync uow)
        {
            _repository = repository;
            _logger = log;
            _uow = uow;
        }

        #endregion Constructor

        /// <summary>
        /// Inserts the specified entity dto.
        /// </summary>
        /// <param name="entityDto">The entity dto.</param>
        /// <exception cref="System.NullReferenceException">errore nel cast da DTO a TEntity in Service --&gt; Insert</exception>
        public virtual void Insert(TDto entityDto)
        {
            try
            {
                var entity = CreateDataFromDTO(entityDto);
                if (entity == null)
                    throw new NullReferenceException("errore nel cast da DTO a TEntity in Service --> Insert");
                _uow.BeginTransaction();
                _repository.Add(entity);
                _uow.SaveChanges();
                _uow.Commit();
            }
            catch (Exception ex)
            {
                _uow.Rollback();
                _logger.LogException(ex);
                throw;
            }
        }

        /// <summary>
        /// Inserts all.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public void InsertAll(IEnumerable<TDto> entity)
        {
            foreach (var ent in entity)
            {
                this.Insert(ent);
            }
        }

        public TDto AddOrUpdate(TDto t)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Updates the void.
        /// </summary>
        /// <param name="updated">The updated.</param>
        /// <param name="key">The key.</param>
        public void UpdateVoid(TDto updated, Tkey key)
        {
            try
            {
                var entity = CreateDataFromDTO(updated);

                _uow.BeginTransaction();
                _repository.Update(entity, key);
                _uow.SaveChanges();
                _uow.Commit();
                _uow.ResetObjectState();
            }
            catch (Exception ex)
            {
                _uow.Rollback();
                _logger.LogException(ex);
                throw;
            }
        }

        /// <summary>
        /// Deletes the specified entity dto.
        /// </summary>
        /// <param name="entityDto">The entity dto.</param>
        /// <returns></returns>
        /// <exception cref="System.NullReferenceException">errore nel cast da DTO a TEntity in Service --&gt; Insert</exception>
        public virtual int Delete(TDto entityDto)
        {
            try
            {
                var entity = CreateDataFromDTO(entityDto);
                if (entity == null)
                    throw new NullReferenceException("errore nel cast da DTO a TEntity in Service --> Insert");
                _uow.BeginTransaction();
                _repository.Delete(entity);
                var rows = _uow.SaveChanges();
                _uow.Commit();
                return rows;
            }
            catch (Exception ex)
            {
                _logger.LogException(ex);
                _uow.Rollback();
                throw;
            }
        }

        /// <summary>
        ///     Counts this instance.
        /// </summary>
        /// <returns></returns>
        public virtual int Count()
        {
            try
            {
                return _repository.Count();
            }
            catch (Exception ex)
            {
                _logger.LogException(ex);
                throw;
            }
        }

        /// <summary>
        ///     Counts the asynchronous.
        /// </summary>
        /// <returns></returns>
        public virtual Task<int> CountAsync()
        {
            try
            {
                return _repository.CountAsync();
            }
            catch (Exception ex)
            {
                _logger.LogException(ex);
                throw;
            }
        }

        /// <summary>
        /// Finds the specified predicate.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public virtual TDto Find(Expression<Func<TEntity, bool>> predicate)
        {
            try
            {
                var query = _repository.Find(predicate);
                return CreateDTOFromData(query);
            }
            catch (Exception ex)
            {
                _logger.LogException(ex);

                throw;
            }
        }

        /// <summary>
        ///     Finds all as queryable.
        /// </summary>
        /// <param name="match">The match.</param>
        /// <returns></returns>
        public virtual IQueryable<TEntity> FindAllAsQueryable(Expression<Func<TEntity, bool>> match)
        {
            try
            {
                return _repository.FindAllAsQueryable(match);
            }
            catch (Exception ex)
            {
                _logger.LogException(ex);
                throw;
            }
        }

        /// <summary>
        ///     Gets all.
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerable<TDto> GetAll()
        {
            try
            {
                var query = _repository.GetAll();
                IEnumerable<TEntity> enumerable = query as IList<TEntity> ?? query.ToList();
                var dto = CreateDTOFromData(enumerable);

                return dto;
            }
            catch (Exception ex)
            {
                _logger.LogException(ex);
                throw;
            }
        }

        /// <summary>
        ///     Gets all as queryable.
        /// </summary>
        /// <returns></returns>
        public virtual IQueryable<TEntity> GetAllAsQueryable()
        {
            try
            {
                return _repository.GetAllAsQueryable();
            }
            catch (Exception ex)
            {
                _logger.LogException(ex);
                throw;
            }
        }

        /// <summary>
        ///     Gets the by identifier.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual TDto FindByID(Tkey key)
        {
            try
            {
                var query = _repository.FindByID(key);
                return CreateDTOFromData(query);
            }
            catch (Exception ex)
            {
                _logger.LogException(ex);
                throw;
            }
        }

        /// <summary>
        ///     Finds all.
        /// </summary>
        /// <param name="match">The match.</param>
        /// <returns></returns>
        public virtual IEnumerable<TDto> FindAll(Expression<Func<TEntity, bool>> match)
        {
            try
            {
                var query = _repository.FindAll(match);
                IEnumerable<TEntity> enumerable = query as IList<TEntity> ?? query.ToList();
                var dto = CreateDTOFromData(enumerable);

                return dto;
            }
            catch (Exception ex)
            {
                _logger.LogException(ex);
                throw;
            }
        }

        /// <summary>
        ///     Fulls the text search.
        /// </summary>
        /// <param name="search">The search.</param>
        /// <param name="skip">The skip.</param>
        /// <param name="take">The take.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public IEnumerable<TDto> FullTextSearch(string search, int skip = 0, int take = 0)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Adds the specified t.
        /// </summary>
        /// <param name="t">The t.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public virtual TDto Add(TDto t)
        {
            try
            {
                var entity = CreateDataFromDTO(t);

                _uow.BeginTransaction();
                _repository.Add(entity);
                _uow.SaveChanges();
                _uow.Commit();

                _uow.ResetObjectState();

                var dtoFromData = CreateDTOFromData(entity);

                return dtoFromData;
            }
            catch (Exception ex)
            {
                _uow.Rollback();
                _logger.LogException(ex);
                throw;
            }
        }

        /// <summary>
        /// Adds all.
        /// </summary>
        /// <param name="tList">The t list.</param>
        /// <returns></returns>
        public virtual IEnumerable<TDto> AddAll(IEnumerable<TDto> tList)
        {
            try
            {
                var entity = CreateDataFromDTO(tList);
                var enumerable = entity as IList<TEntity> ?? entity.ToList();

                _uow.BeginTransaction();
                _repository.AddAll(enumerable);
                _uow.SaveChanges();
                _uow.Commit();
                _uow.ResetObjectState();
                return CreateDTOFromData(enumerable);
            }
            catch (Exception ex)
            {
                _uow.Rollback();
                _logger.LogException(ex);
                throw;
            }
        }

        /// <summary>
        /// Adds all.
        /// </summary>
        /// <param name="tList">The t list.</param>
        /// <returns></returns>
        public virtual async Task InsertAllAsync(IEnumerable<TDto> tList)
        {
            try
            {
                var entity = CreateDataFromDTO(tList);
                var enumerable = entity as IList<TEntity> ?? entity.ToList();

                using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    _repository.AddAll(enumerable);
                    await _uow.SaveChangesAsync();

                    scope.Complete();
                }

                _uow.ResetObjectState();
            }
            catch (Exception ex)
            {
                _logger.LogException(ex);
                throw;
            }
        }

        /// <summary>
        /// Adds the or update asynchronous.
        /// </summary>
        /// <param name="t">The t.</param>
        /// <returns></returns>
        public virtual async Task<TDto> AddOrUpdateAsync(TDto t)
        {
            try
            {
                var entity = CreateDataFromDTO(t);

                using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    await _repository.AddOrUpdateAsync(entity);
                    await _uow.SaveChangesAsync();

                    scope.Complete();
                }

                _uow.ResetObjectState();

                return this.CreateDTOFromData(entity);
            }
            catch (Exception ex)
            {
                _logger.LogException(ex);
                throw;
            }
        }

        /// <summary>
        /// Updates the specified updated.
        /// </summary>
        /// <param name="updated">The updated.</param>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public virtual TDto Update(TDto updated, Tkey key)
        {
            try
            {
                var entity = CreateDataFromDTO(updated);

                _uow.BeginTransaction();
                _repository.Update(entity, key);
                _uow.SaveChanges();
                _uow.Commit();
                _uow.ResetObjectState();
                return CreateDTOFromData(entity);
            }
            catch (Exception ex)
            {
                _uow.Rollback();
                _logger.LogException(ex);
                throw;
            }
        }

        /// <summary>
        /// Updates the specified updated.
        /// </summary>
        /// <param name="updated">The updated.</param>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public virtual async Task UpdateVoidAsync(TDto updated, Tkey key)
        {
            try
            {
                var entity = CreateDataFromDTO(updated);

                using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    await _repository.UpdateAsync(entity, key);
                    await _uow.SaveChangesAsync();

                    scope.Complete();
                }

                _uow.ResetObjectState();
            }
            catch (Exception ex)
            {
                _logger.LogException(ex);
                throw;
            }
        }

        /// <summary>
        /// Inserts the asynchronous.
        /// </summary>
        /// <param name="t">The t.</param>
        /// <returns></returns>
        public virtual async Task InsertAsync(TDto t)
        {
            try
            {
                var entity = CreateDataFromDTO(t);

                using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    _repository.Add(entity);
                    await _uow.SaveChangesAsync();

                    scope.Complete();
                }

                _uow.ResetObjectState();
            }
            catch (Exception ex)
            {
                _logger.LogException(ex);
                throw;
            }
        }

        /// <summary>
        ///     Gets all asynchronous.
        /// </summary>
        /// <returns></returns>
        public virtual async Task<IEnumerable<TDto>> GetAllAsync()
        {
            try
            {
                var query = await _repository.GetAllAsync();
                return CreateDTOFromData(query);
            }
            catch (Exception ex)
            {
                _logger.LogException(ex);
                throw;
            }
        }

        /// <summary>
        ///     Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public virtual async Task<TDto> FindByIDAsync(Tkey id)
        {
            try
            {
                var query = await _repository.FindByIDAsync(id);
                return CreateDTOFromData(query);
            }
            catch (Exception ex)
            {
                _logger.LogException(ex);
                throw;
            }
        }

        /// <summary>
        ///     Finds the asynchronous.
        /// </summary>
        /// <param name="match">The match.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public virtual async Task<TDto> FindAsync(Expression<Func<TEntity, bool>> match)
        {
            try
            {
                var query = await _repository.FindAsync(match);

                return CreateDTOFromData(query);
            }
            catch (Exception ex)
            {
                _logger.LogException(ex);
                throw;
            }
        }

        /// <summary>
        ///     Finds all asynchronous.
        /// </summary>
        /// <param name="match">The match.</param>
        /// <returns></returns>
        public virtual async Task<IEnumerable<TDto>> FindAllAsync(Expression<Func<TEntity, bool>> match)
        {
            try
            {
                var query = await _repository.FindAllAsync(match);

                return CreateDTOFromData(query);
            }
            catch (Exception ex)
            {
                _uow.Rollback();
                _logger.LogException(ex);
                throw;
            }
        }

        /// <summary>
        /// Adds the asynchronous.
        /// </summary>
        /// <param name="t">The t.</param>
        /// <returns></returns>
        public virtual async Task<TDto> AddAsync(TDto t)
        {
            try
            {
                var enity = CreateDataFromDTO(t);

                //_uow.BeginTransaction();
                using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    _repository.Add(enity);
                    await _uow.SaveChangesAsync();

                    scope.Complete();
                }

                _uow.ResetObjectState();

                //_uow.Commit();
                return CreateDTOFromData(enity);
            }
            catch (Exception ex)
            {
                //_uow.Rollback();
                _logger.LogException(ex);
                throw;
            }
        }

        /// <summary>
        ///     Adds all asynchronous.
        /// </summary>
        /// <param name="tList">The t list.</param>
        /// <returns></returns>
        public virtual async Task<IEnumerable<TDto>> AddAllAsync(IEnumerable<TDto> tList)
        {
            try
            {
                var enity = CreateDataFromDTO(tList);
                var enumerable = enity as IList<TEntity> ?? enity.ToList();
                if (!enumerable.Any()) return null;

                using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    _repository.AddAll(enumerable);
                    await _uow.SaveChangesAsync();

                    scope.Complete();
                }

                _uow.ResetObjectState();

                return CreateDTOFromData(enumerable);
            }
            catch (Exception ex)
            {
                _logger.LogException(ex);
                throw;
            }
        }

        /// <summary>
        /// Updates the asynchronous.
        /// </summary>
        /// <param name="updated">The updated.</param>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public virtual async Task<TDto> UpdateAsync(TDto updated, Tkey key)
        {
            try
            {
                var entity = CreateDataFromDTO(updated);

                using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    await _repository.UpdateAsync(entity, key);
                    await _uow.SaveChangesAsync();

                    scope.Complete();
                }

                _uow.ResetObjectState();

                return CreateDTOFromData(entity);
            }
            catch (Exception ex)
            {
                _logger.LogException(ex);
                throw;
            }
        }

        /// <summary>
        /// Deletes the asynchronous.
        /// </summary>
        /// <param name="t">The t.</param>
        /// <returns></returns>
        public virtual async Task<int> DeleteAsync(TDto t)
        {
            try
            {
                var entity = CreateDataFromDTO(t);
                int rows;

                using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    await _repository.DeleteAsync(entity);
                    rows = await _uow.SaveChangesAsync();

                    scope.Complete();
                }

                _uow.ResetObjectState();

                return rows;
            }
            catch (Exception ex)
            {
                _logger.LogException(ex);
                throw;
            }
        }

        /// <summary>
        /// Deletes the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public virtual async Task<int> DeleteAsync(Tkey id)
        {
            try
            {
                int rows;

                using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    await _repository.DeleteAsync(id);
                    rows = await _uow.SaveChangesAsync();

                    scope.Complete();
                }

                _uow.ResetObjectState();

                return rows;
            }
            catch (Exception ex)
            {
                _uow.Rollback();
                _logger.LogException(ex);
                throw;
            }
        }

        #region Helpers Mapping Methods

        /// <summary>
        ///     Creates the TEntity type from dto with autompper.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        public virtual TEntity CreateDataFromDTO(TDto source)
        {
            try
            {
                Mapper.CreateMap<TDto, TEntity>();
                var result = Mapper.Map<TDto, TEntity>(source);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogException(ex);
                throw;
            }
        }

        /// <summary>
        ///     Creates the data from dto.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        public virtual IEnumerable<TEntity> CreateDataFromDTO(IEnumerable<TDto> source)
        {
            try
            {
                return source.Select(CreateDataFromDTO);
            }
            catch (Exception ex)
            {
                _logger.LogException(ex);
                throw;
            }
        }

        /// <summary>
        ///     Creates the dto from data.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        public virtual TDto CreateDTOFromData(TEntity source)
        {
            try
            {
                Mapper.CreateMap<TEntity, TDto>();
                var result = Mapper.Map<TEntity, TDto>(source);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogException(ex);
                throw;
            }
        }

        /// <summary>
        ///     Creates the dto from data.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        public virtual IEnumerable<TDto> CreateDTOFromData(IEnumerable<TEntity> source)
        {
            try
            {
                return source.Select(CreateDTOFromData);
            }
            catch (Exception ex)
            {
                _logger.LogException(ex);
                throw;
            }
        }

        #endregion Helpers Mapping Methods
    }
}