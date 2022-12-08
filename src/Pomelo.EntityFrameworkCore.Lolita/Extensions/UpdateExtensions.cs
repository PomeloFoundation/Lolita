using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Logging;
using Pomelo.EntityFrameworkCore.Lolita;
using Pomelo.EntityFrameworkCore.Lolita.Update;

namespace Microsoft.EntityFrameworkCore
{
    public static class UpdateExtensions
    {
        public static string GenerateBulkUpdateSql<TEntity>(this LolitaSetting<TEntity> self, out IEnumerable<object> parameters)
            where TEntity : class, new()
        {
            var query = self.Query;
            var executor = self.Query.GetService<ILolitaUpdateExecutor>();
            var sql = executor.GenerateSql(self, query, out var _parameters);
            parameters = self.Parameters.Concat(_parameters);
            self.GetService<ILoggerFactory>().CreateLogger("Lolita Bulk Updating").LogInformation(sql);
            return sql;
        }

        public static int Update<TEntity>(this LolitaSetting<TEntity> self)
            where TEntity : class, new()
        {
            var executor = self.Query.GetService<ILolitaUpdateExecutor>();
            var context = self.Query.GetService<ICurrentDbContext>().Context;
            var sql = self.GenerateBulkUpdateSql(out var parameters);
            return executor.Execute(context, sql, parameters.ToArray());
        }

        public static Task<int> UpdateAsync<TEntity>(this LolitaSetting<TEntity> self, CancellationToken cancellationToken = default(CancellationToken))
            where TEntity : class, new()
        {
            var executor = self.Query.GetService<ILolitaUpdateExecutor>();
            var context = self.Query.GetService<ICurrentDbContext>().Context;
            var sql = self.GenerateBulkUpdateSql(out var parameters);
            return executor.ExecuteAsync(context, sql, parameters.ToArray(), cancellationToken);
        }
    }
}
