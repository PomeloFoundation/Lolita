using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Expressions;
using Microsoft.EntityFrameworkCore.Query;

namespace Pomelo.EntityFrameworkCore.Lolita.Update
{
    public interface ILolitaUpdateExecutor
    {
        string GenerateSql<TEntity>(LolitaSetting<TEntity> lolita, RelationalQueryModelVisitor visitor) where TEntity : class, new();

        long Execute(DbContext db, string sql, object[] param);
    }
}
