using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace Pomelo.EntityFrameworkCore.Lolita.Delete
{
    public interface ILolitaDeleteExecutor
    {
        string GenerateSql<TEntity>(IQueryable<TEntity> lolita) where TEntity : class, new();

        int Execute(DbContext db, string sql);

        Task<int> ExecuteAsync(DbContext db, string sql, CancellationToken cancellationToken = default(CancellationToken));
    }
}
