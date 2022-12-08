using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace Pomelo.EntityFrameworkCore.Lolita.Delete
{
    public interface ILolitaDeleteExecutor
    {
        string GenerateSql<TEntity>(IQueryable<TEntity> lolita, out IEnumerable<object> parameters) where TEntity : class, new();

        int Execute(DbContext db, string sql, object[] parameters);

        Task<int> ExecuteAsync(DbContext db, string sql, object[] parameters, CancellationToken cancellationToken = default(CancellationToken));
    }
}
