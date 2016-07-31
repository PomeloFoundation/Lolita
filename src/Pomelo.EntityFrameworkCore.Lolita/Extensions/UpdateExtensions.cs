using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Internal;
using Remotion.Linq.Parsing.Structure;
using Pomelo.EntityFrameworkCore.Lolita;
using Pomelo.EntityFrameworkCore.Lolita.Update;

namespace Microsoft.EntityFrameworkCore
{
    public static class UpdateExtensions
    {
        private static string GenerateBulkUpdateSql<TEntity>(this LolitaSetting<TEntity> self)
            where TEntity : class, new()
        {
            var queryCompiler = (QueryCompiler)ReflectionCommon.QueryCompilerOfEntityQueryProvider.GetValue(self.Query.Provider);
            var nodeTypeProvider = (INodeTypeProvider)ReflectionCommon.NodeTypeProvider.GetValue(queryCompiler);
            var parser = (QueryParser)ReflectionCommon.CreateQueryParserMethod.Invoke(queryCompiler, new object[] { nodeTypeProvider });
            var queryModel = parser.GetParsedQuery(self.Query.Expression);
            var database = (RelationalDatabase)ReflectionCommon.DataBaseOfQueryCompiler.GetValue(queryCompiler);
            var modelVisitor = (RelationalQueryModelVisitor)database.CreateVisitor(queryModel);
            modelVisitor.CreateQueryExecutor<TEntity>(queryModel);
            var qccf = (QueryCompilationContextFactory)ReflectionCommon.QueryCompilationContextFactoryOfDatabase.GetValue(database);
            var context = (DbContext)ReflectionCommon.DbContextOfQueryCompilationContextFactory.GetValue(qccf);
            var executor = context.GetService<ILolitaUpdateExecutor>();
            var sql = executor.GenerateSql(self, modelVisitor);
            return sql;
        }

        public static int Update<TEntity>(this LolitaSetting<TEntity> self)
            where TEntity : class, new()
        {
            var executor = self.Query.GetService<ILolitaUpdateExecutor>();
            var context = self.Query.GetService<ICurrentDbContext>().Context;
            return executor.Execute(context, self.GenerateBulkUpdateSql(), self.Parameters.ToArray());
        }

        public static Task<int> UpdateAsync<TEntity>(this LolitaSetting<TEntity> self, CancellationToken cancellationToken = default(CancellationToken))
            where TEntity : class, new()
        {
            var executor = self.Query.GetService<ILolitaUpdateExecutor>();
            var context = self.Query.GetService<ICurrentDbContext>().Context;
            return executor.ExecuteAsync(context, self.GenerateBulkUpdateSql(), cancellationToken, self.Parameters.ToArray());
        }
    }
}
