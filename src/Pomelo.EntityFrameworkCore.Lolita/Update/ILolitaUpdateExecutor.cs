﻿using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace Pomelo.EntityFrameworkCore.Lolita.Update
{
    public interface ILolitaUpdateExecutor
    {
        string GenerateSql<TEntity>(LolitaSetting<TEntity> lolita, IQueryable<TEntity> query, out IEnumerable<object> parameters) where TEntity : class, new();

        int Execute(DbContext db, string sql, object[] param);

        Task<int> ExecuteAsync(DbContext db, string sql, object[] param, CancellationToken cancellationToken = default);
    }
}
