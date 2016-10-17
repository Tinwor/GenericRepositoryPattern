using System.Threading;
using System.Threading.Tasks;

namespace Repository
{
    public interface IDataContextAsync : IDataContext
    {
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);

        Task<int> SaveChangesAsync();
    }
}