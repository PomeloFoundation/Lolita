using System;
using System.Linq;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Query;
using Remotion.Linq.Parsing.Structure;
using Pomelo.EntityFrameworkCore.Lolita;
using Pomelo.EntityFrameworkCore.Lolita.Delete;

namespace Microsoft.EntityFrameworkCore
{
    public static class DeleteExtensions
    {
        public static long Delete<TEntity>(this IQueryable<TEntity> self)
            where TEntity : class, new()
        {
            var queryCompiler = (QueryCompiler)ReflectionCommon.QueryCompilerOfEntityQueryProvider.GetValue(self.Provider);
            var nodeTypeProvider = (INodeTypeProvider)ReflectionCommon.NodeTypeProvider.GetValue(queryCompiler);
            var parser = (QueryParser)ReflectionCommon.CreateQueryParserMethod.Invoke(queryCompiler, new object[] { nodeTypeProvider });
            var queryModel = parser.GetParsedQuery(self.Expression);
            var database = (RelationalDatabase)ReflectionCommon.DataBaseOfQueryCompiler.GetValue(queryCompiler);
            var modelVisitor = (RelationalQueryModelVisitor)database.CreateVisitor(queryModel);
            modelVisitor.CreateQueryExecutor<TEntity>(queryModel);
            var qccf = (QueryCompilationContextFactory)ReflectionCommon.QueryCompilationContextFactoryOfDatabase.GetValue(database);
            var context = (DbContext)ReflectionCommon.DbContextOfQueryCompilationContextFactory.GetValue(qccf);

            var executor = context.GetService<ILolitaDeleteExecutor>();
            var sql = executor.GenerateSql(self, modelVisitor);
            Console.WriteLine(sql);
            return executor.Execute(context, sql);
        }
    }
}
