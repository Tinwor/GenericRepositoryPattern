using Repository.EF.Repository.Base;
using Repository.Infrastructure;
using Repository.IUnitOfWork;

namespace Repository.EF.Repository
{
    public class EFRepositoryString<TEntity> : Repository<TEntity, string>
          where TEntity : class, IEntity<string>, IObjectState, new()
    {
        public EFRepositoryString(IDataContextAsync context, IUnitOfWorkAsync unitOfWork)
            : base(context, unitOfWork)
        {
        }
    }
}