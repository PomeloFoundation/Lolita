using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Logging;
using Remotion.Linq.Parsing.Structure;
using Pomelo.EntityFrameworkCore.Lolita;

namespace Microsoft.EntityFrameworkCore
{
    public static class IQueryableExtensions
    {
        public static RelationalQueryModelVisitor CompileQuery<TEntity>(this IQueryable<TEntity> self)
        {
            var q = self as EntityQueryable<TEntity>;
            if (q == null)
            {
                return null;
            }
            var queryCompiler = (QueryCompiler)ReflectionCommon.QueryCompilerOfEntityQueryProvider.GetValue(self.Provider);
            var nodeTypeProvider = (INodeTypeProvider)ReflectionCommon.NodeTypeProvider.GetValue(queryCompiler);
            var parser = (QueryParser)ReflectionCommon.CreateQueryParserMethod.Invoke(queryCompiler, new object[] { nodeTypeProvider });
            var queryModel = parser.GetParsedQuery(self.Expression);
            var database = (RelationalDatabase)ReflectionCommon.DataBaseOfQueryCompiler.GetValue(queryCompiler);
            var modelVisitor = (RelationalQueryModelVisitor)database.CreateVisitor(queryModel);
            modelVisitor.CreateQueryExecutor<TEntity>(queryModel);
            return modelVisitor;
        }
    }
}
