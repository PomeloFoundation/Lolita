using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.Extensions.Logging;
using Pomelo.EntityFrameworkCore.Lolita.Delete;

namespace Microsoft.EntityFrameworkCore
{
    public static class DeleteExtensions
    {
        public static string GenerateBulkDeleteSql<TEntity>(this IQueryable<TEntity> self, out IEnumerable<object> parameters)
            where TEntity : class, new()
        {
            var executor = self.GetService<ILolitaDeleteExecutor>();
            var sql = executor.GenerateSql(self, out var _parameters);
            parameters = _parameters;
            self.GetService<ILoggerFactory>().CreateLogger("Lolita Bulk Deleting").LogInformation(sql);
            return sql;
        }

        public static int Delete<TEntity>(this IQueryable<TEntity> self)
            where TEntity : class, new()
        {
            var executor = self.GetService<ILolitaDeleteExecutor>();
            var context = self.GetService<ICurrentDbContext>().Context;
            var sql = self.GenerateBulkDeleteSql(out var parameters);
            return executor.Execute(context, sql, parameters.ToArray());
        }

        public static Task<int> DeleteAsync<TEntity>(this IQueryable<TEntity> self, CancellationToken cancellationToken = default(CancellationToken))
            where TEntity : class, new()
        {
            var executor = self.GetService<ILolitaDeleteExecutor>();
            var context = self.GetService<ICurrentDbContext>().Context;
            var sql = self.GenerateBulkDeleteSql(out var parameters);
            return executor.ExecuteAsync(context, sql, parameters.ToArray(), cancellationToken: cancellationToken);
        }
    }
}
