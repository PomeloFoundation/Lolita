using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace Pomelo.EntityFrameworkCore.Lolita.Delete
{
    public interface ILolitaDeleteExecutor
    {
        string GenerateSql<TEntity>(IQueryable<TEntity> lolita, RelationalQueryModelVisitor visitor) where TEntity : class, new();

        long Execute(DbContext db, string sql);
    }
}
