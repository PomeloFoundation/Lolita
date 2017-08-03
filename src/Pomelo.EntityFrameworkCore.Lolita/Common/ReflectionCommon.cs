using System.Linq;
using System.Reflection;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.EntityFrameworkCore.Storage;

namespace Pomelo.EntityFrameworkCore.Lolita
{
    internal static class ReflectionCommon
    {
        public static readonly FieldInfo QueryCompilerOfEntityQueryProvider = typeof(EntityQueryProvider).GetTypeInfo().DeclaredFields.Single(x => x.Name == "_queryCompiler");
        public static readonly PropertyInfo DatabaseOfQueryCompiler = typeof(QueryCompiler).GetTypeInfo().DeclaredProperties.Single(x => x.Name == "Database");
        public static readonly PropertyInfo DependenciesOfDatabase = typeof(Database).GetTypeInfo().DeclaredProperties.Single(x => x.Name == "Dependencies");
        public static readonly TypeInfo QueryCompilerTypeInfo = typeof(QueryCompiler).GetTypeInfo();
        public static readonly MethodInfo CreateQueryParserMethod = QueryCompilerTypeInfo.DeclaredMethods.Single(x => x.Name == "CreateQueryParser");
        public static readonly PropertyInfo NodeTypeProvider = QueryCompilerTypeInfo.DeclaredProperties.Single(x => x.Name == "NodeTypeProvider");
        public static readonly PropertyInfo QueriesOfRelationalQueryModelVisitor = typeof(RelationalQueryModelVisitor).GetTypeInfo().DeclaredProperties.Single(x => x.Name == "Queries");
        public static readonly PropertyInfo DependenciesOfQueryCompilerContextFactory = typeof(QueryCompilationContextFactory).GetTypeInfo().DeclaredProperties.Single(x => x.Name == "Dependencies");
    }
}
