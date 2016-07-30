using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Reflection;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Query.Expressions;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Query;
using Remotion.Linq.Parsing.Structure;
using Pomelo.EntityFrameworkCore.Lolita;
using Pomelo.EntityFrameworkCore.Lolita.Update;

namespace Microsoft.EntityFrameworkCore
{
    public static class UpdateExtensions
    {
        public static long Update<TEntity>(this LolitaSetting<TEntity> self)
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
            Console.WriteLine(sql);
            Console.WriteLine("==== Parameters ====");
            for (var i = 0; i < self.Parameters.Count; i++)
                Console.WriteLine($"{{{i}}} = {self.Parameters[i].ToString()}");
            return executor.Execute(context, sql, self.Parameters.ToArray());
        }
    }
}
