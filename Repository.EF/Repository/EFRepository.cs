using Repository.EF.Repository.Base;
using Repository.Infrastructure;
using Repository.IUnitOfWork;

namespace Repository.EF.Repository
{
    public class EFRepository<TEntity> : Repository<TEntity, int>
      where TEntity : class, IEntity, IObjectState, new()
    {
        public EFRepository(IDataContextAsync context, IUnitOfWorkAsync unitOfWork)
            : base(context, unitOfWork)
        {
        }
    }
}